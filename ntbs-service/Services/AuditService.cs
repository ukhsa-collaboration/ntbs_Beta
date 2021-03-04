using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;

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
        Task<IList<AuditLog>> GetWriteAuditsForNotification(int notificationId);
    }

    public class AuditService : IAuditService
    {
        public const string AuditUserSystem = "SYSTEM";
        private readonly AuditDatabaseContext _auditContext;

        private const string READ_EVENT = "Read";
        private const string UNMATCH_EVENT = "Unmatch";
        private const string MATCH_EVENT = "Match";
        private const string SPECIMEN_ENTITY_TYPE = "Specimen";

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
                READ_EVENT,
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
                UNMATCH_EVENT,
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
                MATCH_EVENT,
                userName,
                RootEntities.Notification,
                notificationId.ToString());
        }

        public async Task<IList<AuditLog>> GetWriteAuditsForNotification(int notificationId)
        {
            return await _auditContext.AuditLogs
                .Where(log => log.EventType != READ_EVENT)
                .Where(log => log.RootEntity == RootEntities.Notification)
                .Where(log => log.RootId == notificationId.ToString())
                .ToListAsync();
        }
    }
}
