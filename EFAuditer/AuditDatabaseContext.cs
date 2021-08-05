using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFAuditer
{
    public class AuditDatabaseContext : DbContext
    {
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options) : base(options) { }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public async Task AuditOperationAsync(
            string key,
            string entity,
            string details,
            string eventType,
            string userName,
            string rootEntity = null,
            string rootId = null,
            string data = null)
        {
            var log = CreateAuditLog(
                key,
                entity,
                details,
                eventType,
                userName,
                rootEntity,
                rootId,
                data);
            AuditLogs.Add(log);
            await SaveChangesAsync();
        }

        private static AuditLog CreateAuditLog(
            string key,
            string entity,
            string details,
            string eventType,
            string userName,
            string rootEntity,
            string rootId,
            string data)
        {
            return new AuditLog
            {
                OriginalId = key,
                EntityType = entity,
                EventType = eventType,
                AuditDateTime = DateTime.Now,
                AuditUser = userName,
                AuditDetails = details,
                RootEntity = rootEntity,
                RootId = rootId,
                AuditData = data
            };
        }
    }
}
