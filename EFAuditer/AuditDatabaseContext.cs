using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EFAuditer
{
    public class AuditDatabaseContext : DbContext
    {
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options) : base(options) {}

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public async Task AuditReadOperationAsync(string primaryKeyName, int primaryKeyId, object model)
        {
            var log = CreateReadLog(primaryKeyName, primaryKeyId, model);
            AuditLogs.Add(log);
            await SaveChangesAsync();
        }

        private AuditLog CreateReadLog(string primaryKeyName, int primaryKeyId, object model)
        {
            var modelName = model.GetType().Name;
            return new AuditLog() {
                AuditData = CreateReadAuditData(primaryKeyName, primaryKeyId, modelName, model),
                OriginalId = primaryKeyId,
                EntityType = modelName,
                EventType = "Read",
                AuditDateTime = DateTime.Now,
                AuditUser = Environment.UserName
            };
        }

        // This matches the format of the Entity Framework Data Provider audit entry in Audit.NET
        private string CreateReadAuditData(string primaryKeyName, int primaryKeyId, string modelName, object model)
        {
            var auditData = new {
                Table = modelName,
                Action = "Read",
                PrimaryKey = new Dictionary<string, int> {
                    { primaryKeyName, primaryKeyId }
                },
                ColumnValues = model
            };
            return JsonConvert.SerializeObject(auditData);
        }
    }
}
