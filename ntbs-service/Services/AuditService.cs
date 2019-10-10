using System.Threading.Tasks;
using EFAuditer;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.Services
{
    public interface IAuditService
    {
        Task OnGetAuditAsync(int notificationId, string model, string viewType);
    }

    public class AuditService : IAuditService
    {
        private AuditDatabaseContext auditContext;

        private const string READ_EVENT = "Read";

        public AuditService(AuditDatabaseContext auditContext)
        {
            this.auditContext = auditContext;
        }
        
        public async Task OnGetAuditAsync(int notificationId, string model, string viewType)
        {
            await auditContext.AuditOperationAsync(notificationId, model, viewType, READ_EVENT);
        }
    }
}