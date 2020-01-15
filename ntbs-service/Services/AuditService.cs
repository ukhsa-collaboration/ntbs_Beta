using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAuditService
    {
        Task AuditNotificationReadAsync(int notificationId, NotificationAuditType auditDetails, string userName);
        Task AuditUnmatchSpecimen(int notificationId, string labReferenceNumber, string userName);
    }

    public class AuditService : IAuditService
    {
        private readonly AuditDatabaseContext _auditContext;

        private const string READ_EVENT = "Read";
        private const string UNMATCH_EVENT = "Unmatch";

        public AuditService(AuditDatabaseContext auditContext)
        {
            _auditContext = auditContext;
        }

        public async Task AuditNotificationReadAsync(int notificationId, NotificationAuditType auditDetails, string userName)
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

        public async Task AuditUnmatchSpecimen(int notificationId, string labReferenceNumber, string userName)
        {
            await _auditContext.AuditOperationAsync(
                labReferenceNumber,
                typeof(Specimen).Name,
                null,
                UNMATCH_EVENT,
                userName,
                RootEntities.Notification,
                notificationId.ToString()
            );
        }
    }
}
