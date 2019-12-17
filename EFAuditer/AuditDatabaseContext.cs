using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFAuditer
{
    public class AuditDatabaseContext : DbContext
    {
        public AuditDatabaseContext() {}
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options) : base(options) {}

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public async Task AuditOperationAsync(int primaryKeyId, string entity, string details, string eventType, string userName)
        {
            var log = CreateAuditLog(primaryKeyId, entity, details, eventType, userName);
            AuditLogs.Add(log);
            await SaveChangesAsync();
        }

        private AuditLog CreateAuditLog(int primaryKeyId, string entity, string details, string eventType, string userName)
        {
            return new AuditLog() {
                OriginalId = primaryKeyId.ToString(),
                EntityType = entity,
                EventType = eventType,
                AuditDateTime = DateTime.Now,
                AuditUser = userName,
                AuditDetails = details
            };
        }
    }
}
