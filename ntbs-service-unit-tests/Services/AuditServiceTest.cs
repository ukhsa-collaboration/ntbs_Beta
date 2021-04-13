using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using ntbs_service.Models.SeedData;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AuditServiceTest : IDisposable
    {
        private readonly AuditDatabaseContext _context;
        private readonly AuditService _auditService;

        public AuditServiceTest()
        {
            _context = SetupTestContext();
            _auditService = new AuditService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task CorrectlyFiltersReadAndPrintAuditLogs()
        {
            // Arrange
            _context.AuditLogs.AddRange(new List<AuditLog>
            {
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "32", EventType = AuditEventType.READ_EVENT, AuditData = "Read a book"},
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "32", EventType = AuditEventType.PRINT_EVENT, AuditData = "Printed a book"},
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "32", EventType = AuditEventType.MATCH_EVENT, AuditData = "Used matches"},
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "32", EventType = AuditEventType.UNMATCH_EVENT, AuditData = "Put down matches"}
            });
            _context.SaveChanges();
            
            // Act
            var notificationLogs = await _auditService.GetWriteAuditsForNotification(32);

            // Assert
            Assert.Equal(2, notificationLogs.Count);
            Assert.Empty(notificationLogs.Where(log => log.EventType == AuditEventType.PRINT_EVENT));
            Assert.Empty(notificationLogs.Where(log => log.EventType == AuditEventType.READ_EVENT));
            Assert.Contains("Used matches", notificationLogs.Select(log => log.AuditData));
            Assert.Contains("Put down matches", notificationLogs.Select(log => log.AuditData));
        }

        [Fact]
        public async Task CorrectlyFiltersRootEntityTypeAndRootId()
        {
            // Arrange
            _context.AuditLogs.AddRange(new List<AuditLog>
            {
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "33", EventType = AuditEventType.MATCH_EVENT, AuditData = "Used matches"},
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "33", EventType = AuditEventType.UNMATCH_EVENT, AuditData = "Put down matches"},
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "32", EventType = AuditEventType.MATCH_EVENT, AuditData = "Used matches"}, 
                new AuditLog{RootEntity = RootEntities.Notification, RootId = "32", EventType = AuditEventType.UNMATCH_EVENT, AuditData = "Put down matches"},
                new AuditLog{RootEntity = "SomethingElse", RootId = "33", EventType = AuditEventType.MATCH_EVENT, AuditData = "Used matches"}
            });
            _context.SaveChanges();
            
            // Act
            var notificationLogs = await _auditService.GetWriteAuditsForNotification(33);

            // Assert
            Assert.Equal(2, notificationLogs.Count);
            Assert.Empty(notificationLogs.Where(log => log.RootEntity != RootEntities.Notification));
            Assert.Empty(notificationLogs.Where(log => log.RootId != "33"));
        }

        private AuditDatabaseContext SetupTestContext()
        {
            // Generating a unique database name makes sure the database is not shared between tests.
            string dbName = Guid.NewGuid().ToString();
            return new AuditDatabaseContext(new DbContextOptionsBuilder<AuditDatabaseContext>()
                .UseInMemoryDatabase(dbName)
                .Options
            );
        }
    }
}
