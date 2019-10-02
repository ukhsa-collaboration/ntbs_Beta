using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EFAuditer
{
    public class AuditDatabaseContext : DbContext
    {
        public AuditDatabaseContext() {}
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options) : base(options) {}

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public async Task AuditOperationAsync(int primaryKeyId, string entity, string details, string eventType)
        {
            var log = CreateAuditLog(primaryKeyId, entity, details, eventType);
            AuditLogs.Add(log);
            await SaveChangesAsync();
        }

        private AuditLog CreateAuditLog(int primaryKeyId, string entity, string details, string eventType)
        {
            return new AuditLog() {
                OriginalId = primaryKeyId,
                EntityType = entity,
                EventType = eventType,
                AuditDateTime = DateTime.Now,
                AuditUser = Environment.UserName,
                AuditDetails = details
            };
        }
    }
}
