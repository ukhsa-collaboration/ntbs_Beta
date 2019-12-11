using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAuditService
    {
        Task OnGetAuditAsync(int notificationId, string model, AuditType auditDetails, string userName);
    }

    public class AuditService : IAuditService
    {
        private readonly AuditDatabaseContext auditContext;

        private const string READ_EVENT = "Read";

        public AuditService(AuditDatabaseContext auditContext)
        {
            this.auditContext = auditContext;
        }
        
        public async Task OnGetAuditAsync(int notificationId, string model, AuditType auditDetails, string userName)
        {
            await auditContext.AuditOperationAsync(notificationId, model, auditDetails.ToString(), READ_EVENT, userName);
        }
    }
}