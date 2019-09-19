using System.Threading.Tasks;
using EFAuditer;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.Services
{
    public interface IAuditService
    {
        Task OnGetAuditAsync(int notificationId, object model);
    }

    public class AuditService : IAuditService
    {
        private AuditDatabaseContext auditContext;

        private bool shouldAudit;

        public AuditService(AuditDatabaseContext auditContext, IConfiguration configuration)
        {
            this.auditContext = auditContext;
            shouldAudit = configuration.GetValue<bool>(Constants.AUDIT_ENABLED_CONFIG_VALUE);
        }
        
        public async Task OnGetAuditAsync(int notificationId, object model)
        {
            if (shouldAudit) {
                await auditContext.AuditReadOperationAsync("NotificationId", notificationId, model);
            }
        }
    }
}