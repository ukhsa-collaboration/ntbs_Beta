using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ntbs_service.Models.Enums;
using ntbs_service.Models.SeedData;

namespace ntbs_service.Services
{
    public interface IAuditService
    {
        Task AuditNotificationReadAsync(int notificationId, NotificationAuditType auditDetails, string userName);
        Task AuditUnmatchSpecimen(int notificationId,
            string labReferenceNumber,
            string userName,
            NotificationAuditType auditType);
        Task AuditMatchSpecimen(int notificationId,
            string labReferenceNumber,
            string userName,
            NotificationAuditType auditType);
        Task AuditPrint(int notificationId,
            string userName);
        Task AuditSearch(IQueryCollection queryParameterString, string userName);
        Task<IList<AuditLog>> GetWriteAuditsForNotification(int notificationId);
    }

    public class AuditService : IAuditService
    {
        public const string AuditUserSystem = "SYSTEM";
        private readonly AuditDatabaseContext _auditContext;
        
        public static string SPECIMEN_ENTITY_TYPE = "Specimen";

        public AuditService(AuditDatabaseContext auditContext)
        {
            _auditContext = auditContext;
        }

        public async Task AuditNotificationReadAsync(int notificationId, NotificationAuditType auditDetails,
            string userName)
        {
            var notificationIdString = notificationId.ToString();
            await _auditContext.AuditOperationAsync(
                notificationIdString,
                RootEntities.Notification,
                auditDetails.ToString(),
                AuditEventType.READ_EVENT,
                userName,
                RootEntities.Notification,
                notificationIdString);
        }

        public async Task AuditUnmatchSpecimen(int notificationId,
            string labReferenceNumber,
            string userName,
            NotificationAuditType auditType)
        {
            await _auditContext.AuditOperationAsync(
                labReferenceNumber,
                SPECIMEN_ENTITY_TYPE,
                auditType.ToString(),
                AuditEventType.UNMATCH_EVENT,
                userName,
                RootEntities.Notification,
                notificationId.ToString());
        }

        public async Task AuditMatchSpecimen(int notificationId,
            string labReferenceNumber,
            string userName,
            NotificationAuditType auditType)
        {
            await _auditContext.AuditOperationAsync(
                labReferenceNumber,
                SPECIMEN_ENTITY_TYPE,
                auditType.ToString(),
                AuditEventType.MATCH_EVENT,
                userName,
                RootEntities.Notification,
                notificationId.ToString());
        }

        public async Task AuditPrint(int notificationId, string userName)
        {
            await _auditContext.AuditOperationAsync(
                notificationId.ToString(),
                RootEntities.Notification,
                null,
                AuditEventType.PRINT_EVENT,
                userName,
                RootEntities.Notification,
                notificationId.ToString());
        }

        public async Task<IList<AuditLog>> GetWriteAuditsForNotification(int notificationId)
        {
            return await _auditContext.AuditLogs
                .Where(log => log.EventType != AuditEventType.READ_EVENT && log.EventType != AuditEventType.PRINT_EVENT)
                .Where(log => log.RootEntity == RootEntities.Notification)
                .Where(log => log.RootId == notificationId.ToString())
                .ToListAsync();
        }

        public async Task AuditSearch(IQueryCollection queryParameters, string userName)
        {
            var parametersWithValuesDictionary = queryParameters
                .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First());
            var dataForAuditLog = JsonConvert.SerializeObject(parametersWithValuesDictionary);
            await _auditContext.AuditOperationAsync(
                null,
                RootEntities.Notification,
                null,
                AuditEventType.SEARCH_EVENT,
                userName,
                data: dataForAuditLog);
        }
    }
}
