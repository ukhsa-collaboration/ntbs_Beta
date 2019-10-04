using System;
using System.Linq;
using Audit.Core;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class EFAuditServiceExtensions
    {
        public static void AddEFAuditer(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var svcProvider = services.BuildServiceProvider();
            var contextOptions = new DbContextOptionsBuilder<AuditDatabaseContext>()
                .UseSqlServer(connectionString)
                .Options;

            Audit.Core.Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
            {
                // Get user from http context
                // Solution follows idea from https://github.com/thepirat000/Audit.NET/issues/136#issuecomment-402532587
                var userName = svcProvider.GetService<IHttpContextAccessor>().HttpContext?.User?.Identity?.Name;
                scope.SetCustomField(CustomFields.AppUser, userName);
            });

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
                        audit.AuditDetails = ev.CustomFields.ContainsKey(CustomFields.AuditDetails) ? ev.CustomFields[CustomFields.AuditDetails].ToString() : null;
                        audit.AuditDateTime = DateTime.Now;
                        audit.AuditUser = ev.CustomFields.ContainsKey(CustomFields.AppUser) ? ev.CustomFields[CustomFields.AppUser].ToString() : ev.Environment.UserName;
                    })
                    .IgnoreMatchedProperties(true));
        }
    }
}