using System;
using System.Linq;
using Audit.Core;
using EFAuditer;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EFAuditServiceExtensions
    {
        public static void AddEFAuditer(this IServiceCollection services, string connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<AuditDatabaseContext>()
                .UseSqlServer(connectionString)
                .Options;

            Audit.Core.Configuration.Setup()
                .UseEntityFramework(ef => ef
                    .UseDbContext<AuditDatabaseContext>(contextOptions)
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
    }
}