using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAuditService
    {
        Task AuditNotificationReadAsync(int notificationId, NotificationAuditType auditDetails, string userName);
        Task AuditUnmatchSpecimen(int notificationId, string labReferenceNumber, string userName);
        Task AuditMatchSpecimen(int notificationId, string labReferenceNumber, string userName);
    }

    public class AuditService : IAuditService
    {
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

        public async Task AuditUnmatchSpecimen(int notificationId, string labReferenceNumber, string userName)
        {
            await _auditContext.AuditOperationAsync(
                labReferenceNumber,
                SPECIMEN_ENTITY_TYPE,
                null,
                UNMATCH_EVENT,
                userName,
                RootEntities.Notification,
                notificationId.ToString());
        }

        public async Task AuditMatchSpecimen(int notificationId, string labReferenceNumber, string userName)
        {
            await _auditContext.AuditOperationAsync(
                labReferenceNumber,
                SPECIMEN_ENTITY_TYPE,
                null,
                MATCH_EVENT,
                userName,
                RootEntities.Notification,
                notificationId.ToString());
        }
    }
}
