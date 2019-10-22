using System.Threading.Tasks;
using EFAuditer;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.Services
{
    public interface IAuditService
    {
        Task OnGetAuditAsync(int notificationId, string model, string viewType, string userName);
    }

    public class AuditService : IAuditService
    {
        private readonly AuditDatabaseContext auditContext;

        private const string READ_EVENT = "Read";

        public AuditService(AuditDatabaseContext auditContext)
        {
            this.auditContext = auditContext;
        }
        
        public async Task OnGetAuditAsync(int notificationId, string model, string viewType, string userName)
        {
            await auditContext.AuditOperationAsync(notificationId, model, viewType, READ_EVENT, userName);
        }
    }
}