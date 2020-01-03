using Audit.Core;
using EFAuditer;
using Audit.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EFAuditer_tests.UnitTests.Services
{
    public class Entity {}
    public class EFAuditServiceExtensionsTest
    {
        [Fact]
        public void  AuditAction_SetsUpdateValuesCorrectly()
        {
            // Arrange
            var ev = new AuditEvent()
            {
                Environment = new AuditEventEnvironment()
                {
                    UserName = "Env user"
                },
                CustomFields  = new Dictionary<string, object> {}
            };
            EventEntry entry = new EventEntry() {
                PrimaryKey = new Dictionary<string, object> { {"EntityId", "123"} },
                EntityType = typeof(Entity),
                Action = "Update",
                Table = "EntityTable",
            };
            AuditLog audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal("123", audit.OriginalId);
            Assert.Equal("Entity", audit.EntityType);
            Assert.Equal("Update", audit.EventType);
            Assert.Equal(entry.ToJson(), audit.AuditData);
            // Close enough when not injecting time services into the class
            Assert.InRange(audit.AuditDateTime, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Equal("Env user", audit.AuditUser);
            Assert.Equal(null, audit.AuditDetails);
        }

        [Fact]
        public void  AuditAction_SetsUpdateValuesWithCustomFieldsCorrectly()
        {
            // Arrange
            var ev = new AuditEvent()
            {
                Environment = new AuditEventEnvironment()
                {
                    UserName = "Env user"
                },
                CustomFields  = new Dictionary<string, object> 
                { 
                    {CustomFields.AuditDetails, "Notified"}, 
                    {CustomFields.AppUser, "User 1"} 
                }
            };
            EventEntry entry = new EventEntry() {
                PrimaryKey = new Dictionary<string, object> { {"EntityId", "123"} },
                EntityType = typeof(Entity),
                Action = "Update",
                Table = "EntityTable",
            };
            AuditLog audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal("123", audit.OriginalId);
            Assert.Equal("Entity", audit.EntityType);
            Assert.Equal("Update", audit.EventType);
            Assert.Equal(entry.ToJson(), audit.AuditData);
            // Close enough when not injecting time services into the class
            Assert.InRange(audit.AuditDateTime, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Equal("User 1", audit.AuditUser);
            Assert.Equal("Notified", audit.AuditDetails);
        }
    }
}
