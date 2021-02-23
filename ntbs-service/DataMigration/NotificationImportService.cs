using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Server;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration.Exceptions;
using ntbs_service.Jobs;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Sentry;
using Serilog;

namespace ntbs_service.DataMigration
{

    public interface INotificationImportService
    {
        [ExpirationTimeTwoWeeks]
        Task<IList<ImportResult>> ImportByLegacyIdsAsync(PerformContext context, string requestId, List<string> legacyIds);
        /// <summary>
        /// Import notifications (and their linked ones) with notification dates in range [rangeStartDate, rangeEndDate)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestId"></param>
        /// <param name="rangeStartDate"></param>
        /// <param name="rangeEndDate"></param>
        /// <returns></returns>
        [ExpirationTimeTwoWeeks]
        Task<IList<ImportResult>> ImportByDateAsync(PerformContext context, string requestId, DateTime rangeStartDate, DateTime rangeEndDate);
    }

    public class NotificationImportService : INotificationImportService
    {

        private readonly INotificationMapper _notificationMapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationImportRepository _notificationImportRepository;
        private readonly IImportLogger _logger;
        private readonly IMigrationRepository _migrationRepository;
        private readonly IMigratedNotificationsMarker _migratedNotificationsMarker;
        private readonly ISpecimenService _specimenService;
        private readonly INotificationClusterService _notificationClusterService;
        private readonly IImportValidator _importValidator;

        public NotificationImportService(INotificationMapper notificationMapper,
                             INotificationRepository notificationRepository,
                             INotificationImportRepository notificationImportRepository,
                             IImportLogger logger,
                             IHub sentryHub,
                             IMigrationRepository migrationRepository,
                             IMigratedNotificationsMarker migratedNotificationsMarker,
                             ISpecimenService specimenService,
                             IImportValidator importValidator,
                             INotificationClusterService notificationClusterService)
        {
            sentryHub.ConfigureScope(s =>
            {
                s.SetTag("context", "migration"); 
            });

            _notificationMapper = notificationMapper;
            _notificationRepository = notificationRepository;
            _notificationImportRepository = notificationImportRepository;
            _logger = logger;
            _migrationRepository = migrationRepository;
            _migratedNotificationsMarker = migratedNotificationsMarker;
            _specimenService = specimenService;
            _importValidator = importValidator;
            _notificationClusterService = notificationClusterService;
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
                    _logger.LogError(context, requestId, "Invalid state. Some notifications already exist in NTBS database. Manual intervention needed");
                }
                // The last option is that ALL notifications in the group were filtered out - that's OK! It means we
                // have already imported this group - this will show up in the import errors.
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

            _logger.LogInformation(context, requestId, $"{notifications.Count} notifications found to import for {patientName}");
            
            // Verify that no repeated NotificationIds have returned
            var ids = notifications.Select(n => n.LegacyId).ToList();
            if (ids.Distinct().Count() != ids.Count)
            {
                var errorMessage = $"Duplicate records found ({String.Join(',', ids)}) - aborting import for {patientName}";
                importResult.AddGroupError(errorMessage);
                _logger.LogImportFailure(context, requestId, errorMessage);
                return importResult;
            }

            bool isAnyNotificationInvalid = false;
            foreach (var notification in notifications)
            {
                var linkedNotificationId = notification.LegacyId;
                _logger.LogInformation(context, requestId, $"Validating notification with Id={linkedNotificationId}");

                var validationErrors = await _importValidator.CleanAndValidateNotification(context,
                    requestId,
                    notification);
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
                _logger.LogImportFailure(context, requestId, $"Terminating importing notifications for {patientName} due to validation errors");
                return importResult;
            }

            _logger.LogSuccess(context, requestId, $"Importing {notifications.Count} valid notifications");
            try
            {
                var savedNotifications = await _notificationImportRepository.AddLinkedNotificationsAsync(notifications);
                await _migratedNotificationsMarker.MarkNotificationsAsImportedAsync(savedNotifications);
                importResult.NtbsIds = savedNotifications.ToDictionary(x => x.LegacyId, x => x.NotificationId);
                await ImportReferenceLabResultsAsync(context, requestId, savedNotifications, importResult);
                await UpdateClusterInformation(context, requestId, savedNotifications);

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
                _logger.LogImportFailure(context, requestId, message: $"Failed to save notification for {patientName} or mark it as imported ", e);
                importResult.AddGroupError($"{e.Message}: {e.StackTrace}");
            }
            return importResult;
        }

        /// <summary>
        /// We have to run the reference lab result matches after the notifications have been imported into the main db,
        /// since the matches are stored externally - we need to know what the generated NTBS ids are beforehand.
        /// </summary>
        private async Task ImportReferenceLabResultsAsync(PerformContext context,
            string requestId,
            IList<Notification> notifications,
            ImportResult importResult)
        {
            var legacyIds = notifications.Select(n => n.ETSID);
            var matches = await _migrationRepository.GetReferenceLaboratoryMatches(legacyIds);
            foreach (var (legacyId, referenceLaboratoryNumber) in matches)
            {
                var notificationId = notifications.Single(n => n.ETSID == legacyId).NotificationId;
                var success = await _specimenService.MatchSpecimenAsync(notificationId,
                    referenceLaboratoryNumber,
                    AuditService.AuditUserSystem,
                    isMigrating: true);
                if (!success)
                {
                    var error = $"Failed to set the specimen match for Notification: {notificationId}, reference lab number: {referenceLaboratoryNumber}. " +
                                $"The notification is already imported, manual intervention needed!";
                    _logger.LogError(context, requestId, error);
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

        private async Task UpdateClusterInformation(
            PerformContext context,
            string requestId,
            List<Notification> savedNotifications)
        {
            foreach (var notification in savedNotifications)
            {
                var ntbsNotificationId = notification.NotificationId;
                if (!string.IsNullOrWhiteSpace(notification.ETSID)
                    && int.TryParse(notification.ETSID, out var etsNotificationId))
                {
                    var clusterData = await _notificationClusterService.GetNotificationClusterValue(etsNotificationId);
                    await UpdateClusterInformation(context, requestId, notification, clusterData);
                    await _notificationClusterService.SetNotificationClusterValue(etsNotificationId, ntbsNotificationId);
                }
            }
        }

        private async Task UpdateClusterInformation(
            PerformContext context,
            string requestId,
            Notification notification,
            NotificationClusterValue clusterData)
        {
            if (clusterData != null)
            {
                notification.ClusterId = clusterData.ClusterId;
                await _notificationRepository.SaveChangesAsync(
                    NotificationAuditType.SystemEdited,
                    AuditService.AuditUserSystem);
                _logger.LogSuccess(context, requestId, $"Imported notification with ID {notification.NotificationId} into cluster with ID {clusterData.ClusterId}");
            }
        }
    }
}
