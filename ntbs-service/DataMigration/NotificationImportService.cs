using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Hangfire.Server;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.DataMigration
{

    public interface INotificationImportService
    {
        Task<IList<ImportResult>> ImportByLegacyIdsAsync(PerformContext context, string requestId, List<string> notificationId);
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

        public NotificationImportService(INotificationMapper notificationMapper,
                             INotificationRepository notificationRepository,
                             INotificationImportRepository notificationImportRepository,
                             IPostcodeService postcodeService,
                             IImportLogger logger,
                             IMigrationRepository migrationRepository)
        {
            _notificationMapper = notificationMapper;
            _notificationRepository = notificationRepository;
            _notificationImportRepository = notificationImportRepository;
            _postcodeService = postcodeService;
            _logger = logger;
            _migrationRepository = migrationRepository;
        }


        public async Task<IList<ImportResult>> ImportByDateAsync(PerformContext context, string requestId, DateTime rangeStartDate, DateTime rangeEndDate)
        {
            _logger.LogInformation(context, requestId, $"Request to import by Date started");
            _logger.LogInformation(context, requestId, $"Importing notifications in range from {rangeStartDate.Date} to {rangeEndDate.Date}");

            var notificationsGroups = await _notificationMapper.GetNotificationsGroupedByPatient(rangeStartDate, rangeEndDate);

            // Filter out notifications that already exist in ntbs database
            var notificationsGroupsToImport = new List<List<Notification>>();
            foreach (var notificationsGroup in notificationsGroups)
            {
                var legacyIds = notificationsGroup.Select(x => x.LegacyId).ToList();
                var legacyIdsToImport = FilterOutImportedIds(context, requestId, legacyIds);
                if (legacyIdsToImport.Count() == notificationsGroup.Count())
                {
                    notificationsGroupsToImport.Add(notificationsGroup);
                }
                else if (legacyIdsToImport.Count() != 0)
                {
                    _logger.LogFailure(context, requestId, $"Invalid state. Some notifications already exist in NTBS database. Manual intervention needed");
                }
            }

            var importResults = new List<ImportResult>();
            if (notificationsGroupsToImport.Count() > 0)
            {
                // Validate and Import valid notifications
                importResults = notificationsGroupsToImport
                    .Select(notificationsGroup => ValidateAndImportNotificationGroupAsync(context, requestId, notificationsGroup))
                    .Select(t => t.Result)
                    .ToList();
            }

            _logger.LogInformation(context, requestId, $"Request to import by Date finished");
            return importResults;
        }

        public async Task<IList<ImportResult>> ImportByLegacyIdsAsync(PerformContext context, string requestId, List<string> legacyIds)
        {
            _logger.LogInformation(context, requestId, $"Request to import by Id started");

            // Filtering out notifications that already exist in ntbs database.
            var legacyIdsToImport = FilterOutImportedIds(context, requestId, legacyIds);

            var importResults = new List<ImportResult>();
            if (legacyIdsToImport.Count() > 0)
            {
                var notificationsGroupsToImport = await _notificationMapper.GetNotificationsGroupedByPatient(legacyIdsToImport);
                // Validate and Import valid notifications
                importResults = notificationsGroupsToImport
                    .Select(notificationsGroup => ValidateAndImportNotificationGroupAsync(context, requestId, notificationsGroup))
                    .Select(t => t.Result)
                    .ToList();
            }

            _logger.LogInformation(context, requestId, $"Request to import by Id finished");
            return importResults;
        }

        private async Task<ImportResult> ValidateAndImportNotificationGroupAsync(PerformContext context, string requestId, List<Notification> notifications)
        {
            var patientName = notifications.FirstOrDefault().FullName;
            var importResult = new ImportResult(patientName);

            LookupAndAssignPostcode(notifications);

            _logger.LogInformation(context, requestId, $"{notifications.Count} notifications found to import for {patientName}");

            bool isAnyNotificationInvalid = false;
            foreach (var notification in notifications)
            {
                var linkedNotificationId = notification.LegacyId;
                _logger.LogInformation(context, requestId, $"Validating notification with Id={linkedNotificationId}");

                var validationErrors = GetValidationErrors(notification);
                if (validationErrors.Count() == 0)
                {
                    _logger.LogInformation(context, requestId, $"No validation errors found");
                    importResult.AddValidNotification(linkedNotificationId);
                }
                else
                {
                    isAnyNotificationInvalid = true;
                    importResult.AddValidationErrorsMessages(linkedNotificationId, validationErrors.ToList());
                    _logger.LogWarning(context, requestId, $"{validationErrors.Count()} validation errors found for notification with Id={linkedNotificationId}:");
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

            _logger.LogSuccess(context, requestId, $"Importing {notifications.Count()} valid notifications");
            var savedNotifications = await _notificationImportRepository.AddLinkedNotificationsAsync(notifications);
            await _migrationRepository.MarkNotificiationsAsImportedAsync(savedNotifications);


            var newIdsString = string.Join(" ,", savedNotifications.Select(x => x.NotificationId));
            _logger.LogSuccess(context, requestId, $"Imported notifications have following Ids: {newIdsString}");

            _logger.LogInformation(context, requestId, $"Finished importing notification for {patientName}");
            return importResult;
        }

        private List<string> FilterOutImportedIds(PerformContext context, string requestId, List<string> legacyIds)
        {
            var legacyIdsToImport = new List<string>();
            foreach (var legacyId in legacyIds)
            {
                if (_notificationRepository.NotificationWithLegacyIdExists(legacyId))
                {
                    _logger.LogWarning(context, requestId, $"Notification with Id={legacyId} already exists in NTBS database");
                }
                else
                {
                    legacyIdsToImport.Add(legacyId);
                }
            };

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

        private IEnumerable<ValidationResult> GetValidationErrors(Notification notification)
        {
            var validationsResults = new List<ValidationResult>();

            NotificationHelper.SetShouldValidateFull(notification);

            validationsResults.AddRange(ValidateObject(notification));
            validationsResults.AddRange(ValidateObject(notification.PatientDetails));
            validationsResults.AddRange(ValidateObject(notification.ClinicalDetails));
            validationsResults.AddRange(ValidateObject(notification.TravelDetails));
            validationsResults.AddRange(ValidateObject(notification.VisitorDetails));

            return validationsResults;
        }

        private IEnumerable<ValidationResult> ValidateObject<T>(T objectToValidate)
        {
            var validationContext = new ValidationContext(objectToValidate);
            var validationsResults = new List<ValidationResult>();

            Validator.TryValidateObject(objectToValidate, validationContext, validationsResults, true);

            return validationsResults;
        }
    }
}
