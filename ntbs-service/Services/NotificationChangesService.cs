using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.TagHelpers;
using Serilog;

namespace ntbs_service.Services
{
    public interface INotificationChangesService
    {
        Task<IEnumerable<NotificationHistoryListItemModel>> GetChangesList(int notificationId);
    }

    class NotificationChangesService : INotificationChangesService
    {
        private readonly IAuditService _auditService;
        private Dictionary<string, string> UsernameDictionary { get; }

        public NotificationChangesService(IAuditService auditService, IUserRepository userRepository)
        {
            _auditService = auditService;
            UsernameDictionary = userRepository.GetUsernameDictionary();
        }

        public async Task<IEnumerable<NotificationHistoryListItemModel>> GetChangesList(int notificationId)
        {
            var auditLogs = (await _auditService.GetWriteAuditsForNotification(notificationId))
                .GroupByConsecutive((prev, next) => (next.AuditDateTime - prev.AuditDateTime).TotalSeconds <= 2)
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
                UserId = log.AuditUser
            });
        }

        private static IEnumerable<AuditLog> SkipDraftEdits(IReadOnlyCollection<AuditLog> auditLogs)
        {
            var createAuditLog = auditLogs.Take(1);
            var auditLogsFromSubmissionOnwards = auditLogs
                .SkipWhile(log => log.AuditDetails != NotificationAuditType.Notified.ToString());
            
            return createAuditLog.Concat(auditLogsFromSubmissionOnwards);
        }

        private static IEnumerable<AuditLog> GetCanonicalLogs(List<AuditLog> group)
        {
            // The initial creation has a record for each sub-model - we only want to mark creation with a single log
            if (group.Any(log => log.AuditDetails == NotificationAuditType.Added.ToString()))
            {
                yield return group.First();
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

            // Notification submission, as well as changes to notification date
            // also create a treatment start change - no need it though, as it only duplicates the info and is not
            // directly edited by the user
            if (group.Any(log => log.EntityType == nameof(Notification)))
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

        private static string MapAction(AuditLog log)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (Enum.Parse<NotificationAuditType>(log.AuditDetails))
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
                        default:
                            return log.EntityType;
                    }
            }
        }

        private static string MapSubject(AuditLog log)
        {
            if (log.AuditDetails == NotificationAuditType.Added.ToString())
                return "Draft";
            // TODO NTBS-1470 proper mapping that matches the pages' names
            return Regex.Replace(log.EntityType, "(\\B[A-Z])", " $1");
        }

        private string MapUsername(AuditLog log)
        {
            return UsernameDictionary.GetValueOrDefault(log.AuditUser, log.AuditUser);
        }
    }
}
