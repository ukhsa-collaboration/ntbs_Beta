using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Audit.Core;
using Audit.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFAuditer
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
                var user = svcProvider.GetService<IHttpContextAccessor>().HttpContext?.User;

                if (user != null)
                {
                    // Fallback if user doesn't have an email associated with them - as is the case with our test users
                    var userName = !string.IsNullOrEmpty(user.FindFirstValue(ClaimTypes.Upn))
                        ? user.FindFirstValue(ClaimTypes.Upn)
                        : user.Identity.Name;

                    if (userName != null)
                    {
                        scope.SetCustomField(CustomFields.AppUser, userName);
                    }
                }
            });

            SetAuditJsonSettings();

            Action<AuditEvent, EventEntry, AuditLog> auditAction = AuditAction;

            Audit.Core.Configuration.Setup()
                .UseEntityFramework(ef => ef
                    .UseDbContext<AuditDatabaseContext>(contextOptions)
                    .AuditTypeMapper(t => typeof(AuditLog))
                    .AuditEntityAction(auditAction)
                    .IgnoreMatchedProperties()
                );
        }

        public static void AuditAction(AuditEvent ev, EventEntry entry, AuditLog audit)
        {
            audit.OriginalId = entry.PrimaryKey.First().Value.ToString();
            audit.EntityType = entry.EntityType.Name;
            audit.EventType = entry.Action;
            audit.AuditDetails = GetCustomKey(ev, CustomFields.AuditDetails);
            audit.AuditDateTime = DateTime.Now;
            audit.AuditUser = GetCustomKey(ev, CustomFields.OverrideUser)
                ?? GetCustomKey(ev, CustomFields.AppUser)
                ?? ev.Environment.UserName;

            switch (audit.EventType)
            {
                case "Insert":
                    audit.AuditData =
                        JsonSerializer.Serialize(entry.ColumnValues, Audit.Core.Configuration.JsonSettings);
                    break;
                case "Update":
                    if (Boolean.Parse(GetCustomKey(ev, CustomFields.IncludeTypeInUpdate) ?? "false") && entry.ColumnValues.ContainsKey("Type"))
                    {
                        entry.Changes.Insert(0, new EventEntryChange{ColumnName = "Type", OriginalValue = entry.ColumnValues["Type"]});
                    }
                    audit.AuditData =
                        JsonSerializer.Serialize(entry.Changes, Audit.Core.Configuration.JsonSettings);
                    break;
            }

            switch (entry.Entity)
            {
                case IHasRootEntityForAuditing entityWithParent:
                    audit.RootEntity = entityWithParent.RootEntityType;
                    audit.RootId = entityWithParent.RootId;
                    break;
                case IOwnedEntityForAuditing ownedEntity:
                    audit.RootEntity = ownedEntity.RootEntityType;
                    audit.RootId = audit.OriginalId;
                    break;
            }
        }

        public static void SetAuditJsonSettings()
        {
            Audit.Core.Configuration.JsonSettings.Converters.Add(new JsonStringEnumConverter());
        }

        private static string GetCustomKey(AuditEvent ev, string key)
        {
            if (ev.CustomFields.ContainsKey(key))
            {
                return ev.CustomFields[key]?.ToString();
            }

            return null;
        }
    }

}
