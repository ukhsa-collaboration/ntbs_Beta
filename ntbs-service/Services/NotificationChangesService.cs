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
        private const double MaxTimeBetweenEventsInTheSameGroupInSecond = 0.5;
        private readonly IAuditService _auditService;
        private readonly IUserRepository _userRepository;
        private Dictionary<string, string> UsernameDictionary { get; set; }

        public NotificationChangesService(IAuditService auditService, IUserRepository userRepository)
        {
            _auditService = auditService;
            _userRepository = userRepository;
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
        private static IEnumerable<AuditLog> GetCanonicalLogs(List<AuditLog> group)
        {
            // The initial creation (as well as import) has a record for each sub-model
            // - we only want to mark creation with a single log
            if (group.Any(log => log.AuditDetails == NotificationAuditType.Added.ToString()
                                 || log.AuditDetails == NotificationAuditType.Imported.ToString()))
            {
                yield return group.Find(log => log.EntityType == nameof(Notification));
                yield break;
            }

            // Clinical details page contains "site" models, which create their own audit entries
            if (group.Any(log => log.EntityType == nameof(NotificationSite)))
            {
                // This could be done as part of other updates to the clinical details page..
                var clinicalDetailsUpdate = group.Find(log => log.EntityType == nameof(ClinicalDetails));
                if (clinicalDetailsUpdate != null)
                    yield return clinicalDetailsUpdate;
                // .. or, if sites are the only edited items, we want to "fake" a clinical details log entry
                else
                {
                    var fakeClinicalDetailsLog = group.First().Clone();
                    fakeClinicalDetailsLog.EntityType = nameof(ClinicalDetails);
                    fakeClinicalDetailsLog.EventType = "Update";
                    yield return fakeClinicalDetailsLog;
                }

                yield break;
            }

            // Social risks page contains sub-models, which create their own audit entries
            var socialRisksUpdate = group.Find(log => log.EntityType == nameof(SocialRiskFactors));
            if (socialRisksUpdate != null)
            {
                yield return socialRisksUpdate;
                yield break;
            }

            // Travel and visitor details get saved together, but it actually makes sense to show them independently
            if (group.Count == 2
                && group.Any(log => log.EntityType == nameof(TravelDetails))
                && group.Any(log => log.EntityType == nameof(VisitorDetails)))
            {
                foreach (var item in group)
                {
                    yield return item;
                }

                yield break;
            }

            // Transferring creates a flurry of related events.
            // For each of these events we just want to have a single item in the history view 
            var transferAlertLog = group.Find(log => log.EntityType == nameof(TransferAlert)
                                                     || log.EntityType == nameof(TransferRejectedAlert));
            if (transferAlertLog != null)
            {
                // - Request creates just the one:
                //   - TransferAlert - Insert
                if (transferAlertLog.EventType == "Insert")
                {
                    yield return transferAlertLog;
                }
                // - Acceptance creates:
                //   - TreatmentEvent - Insert
                //   - TreatmentEvent - Insert
                //   - PreviousTbService - Insert
                //   - HospitalDetails - Update
                //   - TransferAlert - Update
                else if (group.Any(log => log.EntityType == nameof(TreatmentEvent)))
                {
                    yield return transferAlertLog;
                }
                // - Rejection creates:
                //   - TransferAlert - Update
                //   - TransferRejectedAlert - Insert
                else
                {
                    var rejectionAlertLog = group.Find(log => log.EntityType == nameof(TransferRejectedAlert));
                    // but we need to filter out the subsequent closure of the rejection alert!
                    if (rejectionAlertLog.EventType == "Insert")
                        yield return rejectionAlertLog;
                }

                yield break;
            }

            // Denotification edits the DenotificationDetails, as well as the status on Notification itself.
            if (group.Any(log => log.AuditDetails == NotificationAuditType.Denotified.ToString()))
            {
                yield return group.Find(log => log.EntityType == nameof(Notification));
                yield break;
            }

            // Notification submission, as well as changes to notification date
            // also create a treatment start change - no need for it though, as it only duplicates the info and is not
            // directly edited by the user
            if (group.Any(log => log.AuditDetails == NotificationAuditType.Notified.ToString())
                || group.Any(log => log.AuditDetails == NotificationAuditType.Edited.ToString()
                                    && (log.AuditData?.Contains(@"""ColumnName"":""NotificationDate""") ?? false)))
            {
                // There could be hospital details update records here, too, so we yield all but the treatment event
                // update
                foreach (var log in group)
                {
                    if (log.EntityType != nameof(TreatmentEvent))
                        yield return log;
                }

                yield break;
            }

            // We want this check at the end, since some of the group checks above may have a single member but still
            // require manual intervention (e.g. notification sites)
            if (group.Count == 1)
            {
                yield return group.First();
                yield break;
            }

            Log.Warning("Could not figure out canonical history item for group of logs on notification");
            foreach (var log in group)
            {
                yield return log;
            }
        }

        private static IEnumerable<AuditLog> SkipDraftEdits(IReadOnlyCollection<AuditLog> auditLogs)
        {
            // Imported notifications don't have the draft stage, so don't skip anything
            if (auditLogs.Any(log => log.AuditDetails == NotificationAuditType.Imported.ToString()))
                return auditLogs;
            
            var createAuditLog = auditLogs.Take(1);
            var auditLogsFromSubmissionOnwards = auditLogs
                .SkipWhile(log => log.AuditDetails != NotificationAuditType.Notified.ToString());

            return createAuditLog.Concat(auditLogsFromSubmissionOnwards);
        }

        private static string MapAction(AuditLog log)
        {
            if (log.EntityType == nameof(TransferAlert))
                return log.EventType == "Insert" ? "requested" : "accepted";
            if (log.EntityType == nameof(TransferRejectedAlert))
                return "rejected";

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
            if (log.EntityType == nameof(TransferAlert) || log.EntityType == nameof(TransferRejectedAlert))
                return "Transfer";
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (GetNotificationAuditType(log))
            {
                case NotificationAuditType.Added:
                    return "Draft";
                case NotificationAuditType.SystemEdited when log.EntityType == "Specimen":
                    return "Specimen";
                case NotificationAuditType.SystemEdited
                    when log.AuditData != null && log.AuditData.Contains("\"ColumnName\":\"ClusterId\""):
                    return "Cluster membership";
                default:
                    return MapSubjectBasedOnModel(log.EntityType);
            }
        }

        private static string MapSubjectBasedOnModel(string logEntityType)
        {
            var displayName = Type.GetType($"ntbs_service.Models.Entities.{logEntityType}", true).GetDisplayName();
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ApplicationException($"No display name found for model {logEntityType}");

            return displayName;
        }

        private string MapUsername(AuditLog log)
        {
            return log.AuditUser == AuditService.AuditUserSystem
                ? "NTBS"
                : UsernameDictionary.GetValueOrDefault(log.AuditUser, log.AuditUser);
        }

        private static NotificationAuditType GetNotificationAuditType(AuditLog log)
        {
            // Some non-manual data updates didn't specify the audit details (cluster updates/specimen matches)
            // This aims to be a catch for those (even if we fix that going forwards)
            var auditType = log.AuditDetails == null
                ? NotificationAuditType.SystemEdited
                : Enum.Parse<NotificationAuditType>(log.AuditDetails);
            return auditType;
        }
    }
}
