using System;
using System.Collections.Generic;
using Audit.Core;
using Audit.EntityFramework;
using EFAuditer;
using Newtonsoft.Json;
using Xunit;

namespace EFAuditer_tests.UnitTests.Services
{
    public class Entity { }

    public class OwnedEntityForAuditing : IOwnedEntityForAuditing
    {
        public string RootEntityType => TestHelper.OwnedEntityTypeString;
    }

    public class HasRootEntityForAuditing : IHasRootEntityForAuditing
    {
        public string RootEntityType => TestHelper.HasRootEntityTypeString;
        public string RootId => TestHelper.HasRootEntityRootEntityId;
    }

    public static class TestHelper
    {
        public static string ToJson(this IList<EventEntryChange> changes) =>
            JsonConvert.SerializeObject(changes, Audit.Core.Configuration.JsonSettings);

        public static string OwnedEntityTypeString => "OwnedEntityTypeString";

        public static string HasRootEntityTypeString => "RootEntityTypeString";

        public static string HasRootEntityRootEntityId => "123456";
    }

    public class EFAuditServiceExtensionsTest
    {
        [Fact]
        public void AuditAction_SetsUpdateValuesCorrectly()
        {
            // Arrange
            var ev = new AuditEvent
            {
                Environment = new AuditEventEnvironment()
                {
                    UserName = "Env user"
                },
                CustomFields = new Dictionary<string, object> { }
            };
            var entry = new EventEntry
            {
                PrimaryKey = new Dictionary<string, object> { { "EntityId", "123" } },
                EntityType = typeof(Entity),
                Action = "Update",
                Table = "EntityTable",
                Changes = new List<EventEntryChange>
                {
                    new EventEntryChange
                    {
                        ColumnName = "Test1", NewValue = "Value1"
                    },
                    new EventEntryChange
                    {
                        ColumnName = "Test2", NewValue = "Value3", OriginalValue = "Value2"
                    }
                },
                ColumnValues = new Dictionary<string, object>
                {
                    { "Column1", "Value1" },
                    { "Column2", "Value2" }
                }
            };
            var audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal("123", audit.OriginalId);
            Assert.Equal("Entity", audit.EntityType);
            Assert.Equal("Update", audit.EventType);

            const string expectedFromNullChangesJson = @"{""ColumnName"":""Test1"",""NewValue"":""Value1""}";
            const string expectedUpdateChangesJson = @"{""ColumnName"":""Test2"",""OriginalValue"":""Value2"",""NewValue"":""Value3""}";
            Assert.Contains(expectedFromNullChangesJson, audit.AuditData);
            Assert.Contains(expectedUpdateChangesJson, audit.AuditData);
            const string notExpectedColumnValuesJson = @"{""Column1"":""Value1"",""Column2"":""Value2""}";
            Assert.DoesNotContain(notExpectedColumnValuesJson, audit.AuditData);

            // Close enough when not injecting time services into the class
            Assert.InRange(audit.AuditDateTime, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Equal("Env user", audit.AuditUser);
            Assert.Null(audit.AuditDetails);
        }

        [Fact]
        public void AuditAction_SetsInsertValuesCorrectly()
        {
            // Arrange
            var ev = new AuditEvent
            {
                Environment = new AuditEventEnvironment()
                {
                    UserName = "Env user"
                },
                CustomFields = new Dictionary<string, object> { }
            };
            var entry = new EventEntry
            {
                PrimaryKey = new Dictionary<string, object> { { "EntityId", "123" } },
                EntityType = typeof(Entity),
                Action = "Insert",
                Table = "EntityTable",
                Changes = new List<EventEntryChange>
                {
                    new EventEntryChange
                    {
                        ColumnName = "Test1", NewValue = "Value1"
                    },
                    new EventEntryChange
                    {
                        ColumnName = "Test2", NewValue = "Value3", OriginalValue = "Value2"
                    }
                },
                ColumnValues = new Dictionary<string, object>
                {
                    { "Column1", "Value1" },
                    { "Column2", "Value2" }
                }
            };
            var audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal("123", audit.OriginalId);
            Assert.Equal("Entity", audit.EntityType);
            Assert.Equal("Insert", audit.EventType);

            const string notExpectedFromNullChangesJson = @"{""ColumnName"":""Test1"",""NewValue"":""Value1""}";
            const string notExpectedUpdateChangesJson = @"{""ColumnName"":""Test2"",""OriginalValue"":""Value2"",""NewValue"":""Value3""}";
            Assert.DoesNotContain(notExpectedFromNullChangesJson, audit.AuditData);
            Assert.DoesNotContain(notExpectedUpdateChangesJson, audit.AuditData);
            const string expectedColumnValuesJson = @"{""Column1"":""Value1"",""Column2"":""Value2""}";
            Assert.Contains(expectedColumnValuesJson, audit.AuditData);

            // Close enough when not injecting time services into the class
            Assert.InRange(audit.AuditDateTime, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Equal("Env user", audit.AuditUser);
            Assert.Null(audit.AuditDetails);
        }

        [Fact]
        public void AuditAction_SetsAuditUserAndAuditDetailsCorrectlyWhenProvided()
        {
            // Arrange
            var ev = new AuditEvent()
            {
                Environment = new AuditEventEnvironment()
                {
                    UserName = "Env user"
                },
                CustomFields = new Dictionary<string, object>
                {
                    {CustomFields.AuditDetails, "Notified"},
                    {CustomFields.AppUser, "User 1"}
                }
            };
            var entry = new EventEntry
            {
                PrimaryKey = new Dictionary<string, object> { { "EntityId", "123" } },
                EntityType = typeof(Entity),
                Action = "Update",
                Table = "EntityTable"
            };
            var audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal("User 1", audit.AuditUser);
            Assert.Equal("Notified", audit.AuditDetails);
        }

        [Fact]
        public void AuditAction_UsesOverrideUserInsteadOfAppUserWhenProvided()
        {
            // Arrange
            var ev = new AuditEvent
            {
                Environment = new AuditEventEnvironment
                {
                    UserName = "Env user"
                },
                CustomFields = new Dictionary<string, object>
                {
                    {CustomFields.AuditDetails, "Notified"},
                    {CustomFields.AppUser, "User 1"},
                    {CustomFields.OverrideUser, "SYSTEM"}
                }
            };
            var entry = new EventEntry
            {
                PrimaryKey = new Dictionary<string, object> { { "EntityId", "123" } },
                EntityType = typeof(Entity),
                Action = "Update",
                Table = "EntityTable"
            };
            var audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal("SYSTEM", audit.AuditUser);
        }

        [Fact]
        public void AuditActionForIOwnedEntity_SetsUpdateValuesCorrectly()
        {
            // Arrange
            var entity = new OwnedEntityForAuditing();
            var ev = new AuditEvent
            {
                Environment = new AuditEventEnvironment()
                {
                    UserName = "Env user"
                },
                CustomFields = new Dictionary<string, object> { }
            };
            var entry = new EventEntry
            {
                PrimaryKey = new Dictionary<string, object> { { "EntityId", "123" } },
                EntityType = entity.GetType(),
                Action = "Update",
                Table = "EntityTable",
                Entity = entity
            };
            var audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal("123", audit.RootId);
            Assert.Equal(TestHelper.OwnedEntityTypeString, audit.RootEntity);
        }

        [Fact]
        public void AuditActionForIHasRootEntity_SetsUpdateValuesCorrectly()
        {
            // Arrange
            var entity = new HasRootEntityForAuditing();
            var ev = new AuditEvent
            {
                Environment = new AuditEventEnvironment()
                {
                    UserName = "Env user"
                },
                CustomFields = new Dictionary<string, object> { }
            };
            var entry = new EventEntry
            {
                PrimaryKey = new Dictionary<string, object> { { "EntityId", "123" } },
                EntityType = entity.GetType(),
                Action = "Update",
                Table = "EntityTable",
                Entity = entity
            };
            var audit = new AuditLog();

            // Act
            EFAuditServiceExtensions.AuditAction(ev, entry, audit);

            // Assert
            Assert.Equal(TestHelper.HasRootEntityRootEntityId, audit.RootId);
            Assert.Equal(TestHelper.HasRootEntityTypeString, audit.RootEntity);
        }
    }
}
