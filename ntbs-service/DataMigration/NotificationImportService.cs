using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Server;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration.Exceptions;
using ntbs_service.Helpers;
using ntbs_service.Jobs;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Sentry;
using Serilog;

namespace ntbs_service.DataMigration
{

    public interface INotificationImportService
    {
        [ExpirationTimeTwoWeeks]
        Task<IList<ImportResult>> ImportByLegacyIdsAsync(PerformContext context, string requestId, List<string> legacyIds);
        [ExpirationTimeTwoWeeks]
        Task<IList<ImportResult>> ImportByDateAsync(PerformContext context, string requestId, DateTime rangeStartDate, DateTime rangeEndDate);
    }

    public class NotificationImportService : INotificationImportService
    {

        private readonly INotificationMapper _notificationMapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationImportRepository _notificationImportRepository;
        private readonly IPostcodeService _postcodeService;
        private readonly IImportLogger _logger;
        private readonly IMigrationRepository _migrationRepository;
        private readonly ISpecimenService _specimenService;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public NotificationImportService(INotificationMapper notificationMapper,
                             INotificationRepository notificationRepository,
                             INotificationImportRepository notificationImportRepository,
                             IPostcodeService postcodeService,
                             IImportLogger logger,
                             IHub sentryHub,
                             IMigrationRepository migrationRepository,
                             ISpecimenService specimenService,
                             IReferenceDataRepository referenceDataRepository)
        {
            sentryHub.ConfigureScope(s =>
            {
                s.SetTag("context", "migration"); 
            });

            _notificationMapper = notificationMapper;
            _notificationRepository = notificationRepository;
            _notificationImportRepository = notificationImportRepository;
            _postcodeService = postcodeService;
            _logger = logger;
            _migrationRepository = migrationRepository;
            _specimenService = specimenService;
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IList<ImportResult>> ImportByDateAsync(PerformContext context, string requestId, DateTime rangeStartDate, DateTime rangeEndDate)
        {
            _logger.LogInformation(context, requestId, "Request to import by Date started");
            _logger.LogInformation(context, requestId, $"Importing notifications in range from {rangeStartDate.Date} to {rangeEndDate.Date}");

            var notificationsGroupsToImport = await _notificationMapper.GetNotificationsGroupedByPatient(context, requestId, rangeStartDate, rangeEndDate);

            var importResults = await ImportNotificationGroupsAsync(context, requestId, notificationsGroupsToImport);

            _logger.LogInformation(context, requestId, "Request to import by Date finished");
            return importResults;
        }

        public async Task<IList<ImportResult>> ImportByLegacyIdsAsync(PerformContext context, string requestId, List<string> legacyIds)
        {
            _logger.LogInformation(context, requestId, "Request to import by Id started");

            var notificationsGroupsToImport = await _notificationMapper.GetNotificationsGroupedByPatient(context, requestId, legacyIds);

            var importResults = await ImportNotificationGroupsAsync(context, requestId, notificationsGroupsToImport);

            _logger.LogInformation(context, requestId, "Request to import by Id finished");
            return importResults;
        }

        private async Task<List<ImportResult>> ImportNotificationGroupsAsync(PerformContext context, string requestId, IEnumerable<IList<Notification>> notificationsGroups)
        {
            // Filter out notifications that already exist in ntbs database
            var notificationsGroupsToImport = new List<List<Notification>>();
            foreach (var notificationsGroup in notificationsGroups)
            {
                var legacyIds = notificationsGroup.Select(x => x.LegacyId).ToList();
                var legacyIdsToImport = await FilterOutImportedIdsAsync(context, requestId, legacyIds);
                if (legacyIdsToImport.Count == notificationsGroup.Count)
                {
                    notificationsGroupsToImport.Add(notificationsGroup.ToList());
                }
                else if (legacyIdsToImport.Count != 0)
                {
                    _logger.LogFailure(context, requestId, "Invalid state. Some notifications already exist in NTBS database. Manual intervention needed");
                }
            }

            var importResults = new List<ImportResult>();
            if (notificationsGroupsToImport.Any())
            {
                // Validate and Import valid notifications
                importResults = notificationsGroupsToImport
                    .Select(notificationsGroup => ValidateAndImportNotificationGroupAsync(context, requestId, notificationsGroup))
                    .Select(t => t.Result)
                    .ToList();
            }

            return importResults;
        }
        
        private async Task<ImportResult> ValidateAndImportNotificationGroupAsync(PerformContext context, string requestId, List<Notification> notifications)
        {
            var patientName = notifications.First().PatientDetails.FullName;
            var importResult = new ImportResult(patientName);

            LookupAndAssignPostcode(notifications);

            _logger.LogInformation(context, requestId, $"{notifications.Count} notifications found to import for {patientName}");
            
            // Verify that no repeated NotificationIds have returned
            var ids = notifications.Select(n => n.LegacyId).ToList();
            if (ids.Distinct().Count() != ids.Count)
            {
                var errorMessage = $"Duplicate records found ({String.Join(',', ids)}) - aborting import for {patientName}";
                importResult.AddGroupError(errorMessage);
                _logger.LogFailure(context, requestId, errorMessage);
                return importResult;
            }

            bool isAnyNotificationInvalid = false;
            foreach (var notification in notifications)
            {
                var linkedNotificationId = notification.LegacyId;
                _logger.LogInformation(context, requestId, $"Validating notification with Id={linkedNotificationId}");

                CleanData(notification, linkedNotificationId, context, requestId);

                var validationErrors = (await GetValidationErrors(notification)).ToList();
                if (!validationErrors.Any())
                {
                    _logger.LogInformation(context, requestId, "No validation errors found");
                    importResult.AddValidNotification(linkedNotificationId);
                }
                else
                {
                    isAnyNotificationInvalid = true;
                    importResult.AddValidationErrorsMessages(linkedNotificationId, validationErrors);
                    _logger.LogWarning(context, requestId, $"{validationErrors.Count} validation errors found for notification with Id={linkedNotificationId}:");
                    foreach (var validationError in validationErrors)
                    {
                        _logger.LogWarning(context, requestId, validationError.ErrorMessage);
                    }
                }
            }

            if (isAnyNotificationInvalid)
            {
                _logger.LogFailure(context, requestId, $"Terminating importing notifications for {patientName} due to validation errors");
                return importResult;
            }

            _logger.LogSuccess(context, requestId, $"Importing {notifications.Count} valid notifications");
            try
            {
                var savedNotifications = await _notificationImportRepository.AddLinkedNotificationsAsync(notifications);
                await _migrationRepository.MarkNotificationsAsImportedAsync(savedNotifications);
                importResult.NtbsIds = savedNotifications.ToDictionary(x => x.LegacyId, x => x.NotificationId);
                await ImportReferenceLabResultsAsync(savedNotifications, importResult);

                var newIdsString = string.Join(" ,", savedNotifications.Select(x => x.NotificationId));
                _logger.LogSuccess(context, requestId, $"Imported notifications have following Ids: {newIdsString}");

                _logger.LogInformation(context, requestId, $"Finished importing notification for {patientName}");
            }
            catch (MarkingNotificationsAsImportedFailedException e)
            {
                Log.Error(e, e.Message);
                _logger.LogWarning(context, requestId, message: e.Message);
                importResult.AddGroupError($"{e.Message}: {e.StackTrace}");
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                _logger.LogFailure(context, requestId, message: $"Failed to save notification for {patientName} or mark it as imported ", e);
                importResult.AddGroupError($"{e.Message}: {e.StackTrace}");
            }
            return importResult;
        }

        /// <summary>
        /// There are instances of validation issues in the legacy data where we've resolved to remove the offending
        /// data points.
        /// As this is a data-lossy action, we want to perform it here (rather than at sql script level), to ensure that
        /// it is recorded in the migration log
        /// </summary>
        private void CleanData(Notification notification,
            string notificationId,
            PerformContext context,
            string requestId)
        {
            var missingDateResults = notification.TestData.ManualTestResults
                .Where(result => !result.TestDate.HasValue)
                .ToList();
            missingDateResults.ForEach(result =>
            {
                var missingDateMessage =
                    $"Notification {notificationId} had test results without a date set. The notification will be imported without this test record.";
                _logger.LogWarning(context, requestId, missingDateMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            var dateInFutureResults = notification.TestData.ManualTestResults
                .Where(result => result.TestDate > DateTime.Today)
                .ToList();
            dateInFutureResults.ForEach(result =>
            {
                var dateInFutureMessage =
                    $"Notification {notificationId} had test results with date set in future. The notification will be imported without this test record.";
                _logger.LogWarning(context, requestId, dateInFutureMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            var missingResults = notification.TestData.ManualTestResults
                .Where(result => result.Result == null)
                .ToList();
            missingResults.ForEach(result =>
            {
                var missingResultMessage =
                    $"Notification {notificationId} had test results without a result recorded. The notification will be imported without this test record.";
                _logger.LogWarning(context, requestId, missingResultMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            // After filtering out invalid tests, we might have none left
            if (!notification.TestData.ManualTestResults.Any())
            {
                notification.TestData.HasTestCarriedOut = false;
            }

            if (ValidateObject(notification.ContactTracing).Any())
            {
                var message =
                    $"Notification {notificationId} invalid contact tracing figures. The notification will be imported without contact tracing data.";
                _logger.LogWarning(context, requestId, message);
                notification.ContactTracing = new ContactTracing();
            }
        }

        /// <summary>
        /// We have to run the reference lab result matches after the notifications have been imported into the main db,
        /// since the matches are stored externally - we need to know what the generated NTBS ids are beforehand.
        /// </summary>
        private async Task ImportReferenceLabResultsAsync(IList<Notification> notifications, ImportResult importResult)
        {
            var legacyIds = notifications.Select(n => n.ETSID);
            var matches = await _migrationRepository.GetReferenceLaboratoryMatches(legacyIds);
            foreach (var (legacyId, referenceLaboratoryNumber) in matches)
            {
                var notificationId = notifications.Single(n => n.ETSID == legacyId).NotificationId;
                var success = await _specimenService.MatchSpecimenAsync(notificationId, referenceLaboratoryNumber, "SYSTEM", isMigrating: true);
                if (!success)
                {
                    var error = $"Failed to set the specimen match for Notification: {notificationId}, reference lab number: {referenceLaboratoryNumber}";
                    importResult.AddNotificationError(legacyId, error);
                }
            }
        }

        private async Task<List<string>> FilterOutImportedIdsAsync(PerformContext context, string requestId, List<string> legacyIds)
        {
            var legacyIdsToImport = new List<string>();
            foreach (var legacyId in legacyIds)
            {
                if (await _notificationRepository.NotificationWithLegacyIdExistsAsync(legacyId))
                {
                    _logger.LogWarning(context, requestId, $"Notification with Id={legacyId} already exists in NTBS database");
                }
                else
                {
                    legacyIdsToImport.Add(legacyId);
                }
            }
            return legacyIdsToImport;
        }

        private void LookupAndAssignPostcode(List<Notification> notifications)
        {
            var postcodes = _postcodeService.FindPostcodes(notifications.Select(x => x.PatientDetails.Postcode).ToList());
            notifications.ForEach(n =>
            {
                var normalisedPostcode = n.PatientDetails.Postcode?.Replace(" ", "").ToUpper();
                var lookedUpPostcode = postcodes.FirstOrDefault(p => p.Postcode == normalisedPostcode);
                n.PatientDetails.PostcodeToLookup = lookedUpPostcode?.Postcode;
                n.PatientDetails.PostcodeLookup = lookedUpPostcode;
            });
        }

        private async Task<IEnumerable<ValidationResult>> GetValidationErrors(Notification notification)
        {
            var validationsResults = new List<ValidationResult>();

            NotificationHelper.SetShouldValidateFull(notification);

            validationsResults.AddRange(ValidateObject(notification));
            validationsResults.AddRange(ValidateObject(notification.PatientDetails));
            validationsResults.AddRange(ValidateObject(notification.ClinicalDetails));
            validationsResults.AddRange(ValidateObject(notification.TravelDetails));
            validationsResults.AddRange(ValidateObject(notification.VisitorDetails));
            validationsResults.AddRange(ValidateObject(notification.ComorbidityDetails));
            validationsResults.AddRange(ValidateObject(notification.ImmunosuppressionDetails));
            validationsResults.AddRange(ValidateObject(notification.SocialRiskFactors));
            validationsResults.AddRange(ValidateObject(notification.HospitalDetails));
            validationsResults.AddRange(ValidateObject(notification.ContactTracing));
            validationsResults.AddRange(ValidateObject(notification.PatientTBHistory));
            validationsResults.AddRange(ValidateObject(notification.TestData));
            validationsResults.AddRange(notification.TestData.ManualTestResults.SelectMany(ValidateObject));
            validationsResults.AddRange(notification.SocialContextAddresses.SelectMany(ValidateObject));
            validationsResults.AddRange(notification.SocialContextVenues.SelectMany(ValidateObject));
            validationsResults.AddRange(notification.TreatmentEvents.SelectMany(ValidateObject));
            validationsResults.AddRange(ValidateObject(notification.MBovisDetails));
            validationsResults.AddRange(notification.MBovisDetails.MBovisAnimalExposures.SelectMany(ValidateObject));
            validationsResults.AddRange(notification.MBovisDetails.MBovisExposureToKnownCases.SelectMany(ValidateObject));
            validationsResults.AddRange(notification.MBovisDetails.MBovisOccupationExposures.SelectMany(ValidateObject));
            validationsResults.AddRange(notification.MBovisDetails.MBovisUnpasteurisedMilkConsumptions.SelectMany(ValidateObject));

            validationsResults.AddRange(await ValidateAndSetCaseManager(notification.HospitalDetails));

            return validationsResults;
        }

        private IEnumerable<ValidationResult> ValidateObject<T>(T objectToValidate)
        {
            var validationContext = new ValidationContext(objectToValidate);
            var validationsResults = new List<ValidationResult>();

            Validator.TryValidateObject(objectToValidate, validationContext, validationsResults, true);

            return validationsResults;
        }

        private async Task<IEnumerable<ValidationResult>> ValidateAndSetCaseManager(HospitalDetails details)
        {
            var validationsResults = new List<ValidationResult>();

            if (string.IsNullOrEmpty(details.CaseManagerUsername))
            {
                return validationsResults;
            }

            var caseManager =
                await _referenceDataRepository.GetCaseManagerByUsernameAsync(details.CaseManagerUsername);
            if (caseManager != null)
            {
                return validationsResults;
            }

            var message = $"Case manager {details.CaseManagerUsername} is not present in NTBS database";
            validationsResults.Add(new ValidationResult(message, new[] {nameof(details.CaseManagerUsername)}));

            return validationsResults;
        }
    }
}
