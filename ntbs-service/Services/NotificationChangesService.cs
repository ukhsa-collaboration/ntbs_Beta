using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.TagHelpers;
using Serilog;

namespace ntbs_service.Services
{
    public interface INotificationChangesService
    {
        Task<IEnumerable<NotificationHistoryListItemModel>> GetChangesList(int notificationId);
    }

    public class NotificationChangesService : INotificationChangesService
    {
        // This number has been somewhat arbitrarily picked to be
        // - large enough that the audits created as part of a single user-generated request are grouped together, but
        // - small enough that user quickly editing different pages isn't likely to complete two saves within it 
        private const double MaxTimeBetweenEventsInTheSameGroupInSecond = 1.5;
        private readonly IAuditService _auditService;
        private readonly IUserRepository _userRepository;
        private readonly ILogService _logService;
        private Dictionary<string, string> UsernameDictionary { get; set; }

        public NotificationChangesService(IAuditService auditService, IUserRepository userRepository, ILogService logService)
        {
            _auditService = auditService;
            _userRepository = userRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<NotificationHistoryListItemModel>> GetChangesList(int notificationId)
        {
            UsernameDictionary = await _userRepository.GetUsernameDictionary();

            var auditLogs = (await _auditService.GetWriteAuditsForNotification(notificationId))
                .OrderBy(log => log.AuditDateTime)
                .GroupByConsecutive((prev, next) => (next.AuditDateTime - prev.AuditDateTime).TotalSeconds <= MaxTimeBetweenEventsInTheSameGroupInSecond)
                .SelectMany(GetCanonicalLogs)
                .ToList();

            // We do not detail out the changes that happened before submission  to avoid unnecessary page clutter
            var auditLogsToShow = SkipDraftEdits(auditLogs);

            return auditLogsToShow.Select(log => new NotificationHistoryListItemModel
            {
                Action = MapAction(log),
                Date = log.AuditDateTime,
                Subject = MapSubject(log),
                Username = MapUsername(log),
                UserId = log.AuditUser == AuditService.AuditUserSystem ? null : log.AuditUser
            });
        }

        /// <summary>
        /// This method is designed to extract a single record out of potentially multiple records that are the result
        /// of a single action.
        /// This happens e.g. when a single page save actually modifies multiple models.
        ///
        /// In its current form, the "canonical" events produced by this method are enough to identify the type of the
        /// event. As we implement more detailed options for the events, it will need to do more to combine the data
        /// from the multiple events, rather than just picking a "recognizable" one to act as the summary for the group. 
        /// </summary>
        private IEnumerable<AuditLog> GetCanonicalLogs(List<AuditLog> group)
        {
            // The initial creation (as well as import) has a record for each sub-model
            // - we only want to mark creation with a single log
            if (group.Any(log => GetNotificationAuditType(log) == NotificationAuditType.Added
                                 || GetNotificationAuditType(log) == NotificationAuditType.Imported))
            {
                return new List<AuditLog> {group.Find(log => log.EntityType == nameof(Notification))};
            }

            // Clinical details page contains "site" models, which create their own audit entries
            if (group.Any(log => log.EntityType == nameof(NotificationSite)))
            {
                // This could be done as part of other updates to the clinical details page..
                var clinicalDetailsUpdate = group.Find(log => log.EntityType == nameof(ClinicalDetails));
                if (clinicalDetailsUpdate != null)
                {
                    return new List<AuditLog> {clinicalDetailsUpdate};
                }
                // .. or, if sites are the only edited items, we want to "fake" a clinical details log entry
                var fakeClinicalDetailsLog = group.First().Clone();
                fakeClinicalDetailsLog.EntityType = nameof(ClinicalDetails);
                fakeClinicalDetailsLog.EventType = "Update";
                return new List<AuditLog> {fakeClinicalDetailsLog};
            }

            // Comorbidities page contains an Immunosuppression model, which creates its own audit entry
            // These are nicely separate concepts, so we can actually have them be detailed out on their own.
            if (group.Any(log => log.EntityType == nameof(ImmunosuppressionDetails)))
            {
                return group;
            }

            // Social risks page contains sub-models, which create their own audit entries
            var socialRisksUpdate = group.Find(log => log.EntityType == nameof(SocialRiskFactors));
            if (socialRisksUpdate != null)
            {
                return new List<AuditLog> {socialRisksUpdate};
            }

            // Travel and visitor details get saved together, but it actually makes sense to show them independently
            if (group.Count == 2
                && group.Any(log => log.EntityType == nameof(TravelDetails))
                && group.Any(log => log.EntityType == nameof(VisitorDetails)))
            {
                return group;
            }

            // Transferring creates a flurry of related events.
            // For each of these events we just want to have a single item in the history view 

            // Rejection creates:
            //   - TransferAlert - Update
            //   - TransferRejectedAlert - Insert
            if (group.Any(log => log.EntityType == nameof(TransferRejectedAlert)))
            {
                var rejectionAlertLog = group.Find(log => log.EntityType == nameof(TransferRejectedAlert));
                // but we need to filter out the subsequent closure of the rejection alert!
                if (rejectionAlertLog.EventType == "Insert")
                {
                    rejectionAlertLog.ActionString = "rejected";
                    return new List<AuditLog> {rejectionAlertLog};
                }

                return new List<AuditLog>();
            }

            var transferAlertLog = group.Find(log => log.EntityType == nameof(TransferAlert));
            if (transferAlertLog != null)
            {
                // Request creates just the one:
                //   - TransferAlert - Insert
                if (transferAlertLog.EventType == "Insert")
                {
                    transferAlertLog.ActionString = "requested";
                    return new List<AuditLog> {transferAlertLog};
                }
                // Acceptance creates:
                //   - TreatmentEvent - Insert
                //   - TreatmentEvent - Insert
                //   - PreviousTbService - Insert
                //   - HospitalDetails - Update
                //   - TransferAlert - Update
                if (group.Any(log => log.EntityType == nameof(TreatmentEvent)))
                {
                    transferAlertLog.ActionString = "accepted";
                    return new List<AuditLog> {transferAlertLog};
                }
                // Transfer cancellation:
                //  - TransferAlert - Update
                transferAlertLog.ActionString = "cancelled";
                return new List<AuditLog> {transferAlertLog};
            }

            // Denotification edits the DenotificationDetails, as well as the status on Notification itself.
            if (group.Any(log => GetNotificationAuditType(log) == NotificationAuditType.Denotified))
            {
                return new List<AuditLog> {group.Find(log => log.EntityType == nameof(Notification))};
            }

            // Notification submission, as well as changes to notification date
            // also create a treatment start change - no need for it though, as it only duplicates the info and is not
            // directly edited by the user
            if (group.Any(log => GetNotificationAuditType(log) == NotificationAuditType.Notified)
                || group.Any(log => GetNotificationAuditType(log) == NotificationAuditType.Edited
                                    && (log.AuditData?.Contains(@"""ColumnName"":""NotificationDate""") ?? false)))
            {
                // There could be hospital details update records here, too, so we yield all but the treatment event
                // update
                return group.Where(log => log.EntityType != nameof(TreatmentEvent));
            }

            // Adding treatment started information creates both clinical details treatment event updates. We want to
            // show both, as the change is done on the clinical details page but seen on the treatment events section
            if(group.Count == 2
               && group.Any(log => log.EntityType == nameof(TreatmentEvent))
               && group.Any(log => log.EntityType == nameof(ClinicalDetails)))
            {
                return group;
            }

            // We want this check at the end, since some of the group checks above may have a single member but still
            // require manual intervention (e.g. notification sites)
            if (group.Count == 1)
            {
                return new List<AuditLog> {group.First()};
            }

            _logService.LogWarning("Could not figure out canonical history item for group of logs.");
            foreach (var log in group)
            {
                Log.Debug($"Log in group: {log}");
            }
            return group;
        }

        private static IEnumerable<AuditLog> SkipDraftEdits(IReadOnlyCollection<AuditLog> auditLogs)
        {
            // Imported notifications don't have the draft stage, so don't skip anything
            if (auditLogs.Any(log => GetNotificationAuditType(log) == NotificationAuditType.Imported))
            {
                return auditLogs;
            }

            var createAuditLog = auditLogs.Take(1);
            var auditLogsFromSubmissionOnwards = auditLogs
                .SkipWhile(log => GetNotificationAuditType(log) != NotificationAuditType.Notified);

            return createAuditLog.Concat(auditLogsFromSubmissionOnwards);
        }

        private static string MapAction(AuditLog log)
        {
            if (log.ActionString != null)
            {
                return log.ActionString;
            }

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (GetNotificationAuditType(log))
            {
                case NotificationAuditType.Notified:
                    return "submitted";
                case NotificationAuditType.Added:
                    return "created";
                case NotificationAuditType.Imported:
                    return "imported";
                case NotificationAuditType.Denotified:
                    return "denotified";
                case NotificationAuditType.Closed:
                    return "closed";
                default:
                    switch (log.EventType)
                    {
                        case "Update":
                            return "updated";
                        case "Insert":
                            return "added";
                        case "Delete":
                            return "deleted";
                        case "Match":
                            return "matched";
                        case "Unmatch":
                            return "unmatched";
                        default:
                            return log.EntityType;
                    }
            }
        }

        private static string MapSubject(AuditLog log)
        {
            switch (log.EntityType)
            {
                case nameof(TransferAlert):
                case nameof(TransferRejectedAlert):
                    return "Transfer";
                case "Specimen":
                    return "Specimen";
                default:
                    switch (GetNotificationAuditType(log))
                    {
                        case NotificationAuditType.Added:
                            return "Draft";
                        case NotificationAuditType.SystemEdited
                            when log.AuditData != null && log.AuditData.Contains(@"""ColumnName"":""ClusterId"""):
                            return "Cluster membership";
                        default:
                            return MapSubjectBasedOnModel(log.EntityType);
                    }
            }
        }

        private static string MapSubjectBasedOnModel(string logEntityType)
        {
            var displayName = Type.GetType($"ntbs_service.Models.Entities.{logEntityType}", true).GetDisplayName();
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ApplicationException($"No display name found for model {logEntityType}");
            }

            return displayName;
        }

        private string MapUsername(AuditLog log)
        {
            return log.AuditUser == null || log.AuditUser == AuditService.AuditUserSystem
                ? "NTBS"
                : UsernameDictionary.GetValueOrDefault(log.AuditUser, log.AuditUser);
        }

        private static NotificationAuditType GetNotificationAuditType(AuditLog log)
        {
            // Some non-manual data updates didn't specify the audit details (cluster updates/specimen matches)
            // This aims to be a catch for those (even if we fix that going forwards)
            return log.AuditDetails == null
                ? NotificationAuditType.SystemEdited
                : Enum.Parse<NotificationAuditType>(log.AuditDetails);
        }
    }
}
