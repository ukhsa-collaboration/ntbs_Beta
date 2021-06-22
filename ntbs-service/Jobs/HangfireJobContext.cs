using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Services;

namespace ntbs_service.Jobs
{
    public class HangfireJobContext
    {
        protected HangfireJobContext(NtbsContext auditDbContext)
        {
            auditDbContext.AddAuditCustomField(CustomFields.OverrideUser, AuditService.AuditUserSystem);
        }
    }
}
