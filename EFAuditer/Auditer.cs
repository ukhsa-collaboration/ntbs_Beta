using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Core;
using Newtonsoft.Json;

namespace EFAuditer
{
    public static class Auditer
    {
        // TODO NTBS-218: Split this into a static class just containing the setup method to be called once, and a non-static auditer that can be injected as a singleton into the application that uses this
        // Add Db read methods to fetch all data by e.g. Id, type etc.
        public static void SetupAuditFramework()
        {
            Audit.Core.Configuration.Setup()
                .UseEntityFramework(ef => ef
                    .UseDbContext<AuditDatabaseContext>()
                    .AuditTypeMapper(t => typeof(AuditLog))
                    .AuditEntityAction<AuditLog>((ev, entry, audit) =>
                    {
                        audit.AuditData = entry.ToJson();
                        audit.OriginalId = int.Parse(entry.PrimaryKey.First().Value.ToString());
                        audit.EntityType = entry.EntityType.Name;
                        audit.EventType = entry.Action;
                        audit.AuditDateTime = DateTime.Now;
                        audit.AuditUser = ev.Environment.UserName;
                    })
                    .IgnoreMatchedProperties(true));
        }

        public static async Task AuditReadOperation(string primaryKeyName, int primaryKeyId, object model)
        {
            using (var dbContext = new AuditDatabaseContext())
            {
                var log = CreateReadLog(primaryKeyName, primaryKeyId, model);
                dbContext.AuditLogs.Add(log);
                await dbContext.SaveChangesAsync();
            }
        }

        private static AuditLog CreateReadLog(string primaryKeyName, int primaryKeyId, object model)
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
        private static string CreateReadAuditData(string primaryKeyName, int primaryKeyId, string modelName, object model)
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