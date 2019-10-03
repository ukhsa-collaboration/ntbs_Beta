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
        private bool shouldAudit;

        public AuditService(AuditDatabaseContext auditContext, IConfiguration configuration)
        {
            this.auditContext = auditContext;
            shouldAudit = configuration.GetValue<bool>(Constants.AUDIT_ENABLED_CONFIG_VALUE);
        }
        
        public async Task OnGetAuditAsync(int notificationId, string model, string viewType)
        {
            if (shouldAudit) {
                await auditContext.AuditOperationAsync(notificationId, model, viewType, READ_EVENT);
            }
        }
    }
}