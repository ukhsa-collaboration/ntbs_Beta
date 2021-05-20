using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.DataMigration
{
    public interface INotificationImportRepository
    {
        Task<List<Notification>> AddLinkedNotificationsAsync(List<Notification> notifications);
        void AddSystemUserToAudits();

        Task<LegacyImportMigrationRun> CreateLegacyImportMigrationRun(IList<string> legacyIds,
            string fileName = null, string description = null);
        Task<LegacyImportMigrationRun> CreateLegacyImportMigrationRun(DateTime rangeStartDate,
            DateTime rangeEndDate, string description = null);

        Task AddLegacyImportNotificationLogMessage(LegacyImportNotificationLogMessage message);
        Task AddLegacyImportNotificationLogMessageRange(IEnumerable<LegacyImportNotificationLogMessage> messages);
        Task AddLegacyImportNotificationOutcomeRange(IEnumerable<LegacyImportNotificationOutcome> outcomes);
    }

    public class NotificationImportRepository : INotificationImportRepository
    {
        private readonly NtbsContext _context;
        private readonly IConfiguration _configuration;

        public NotificationImportRepository(NtbsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void AddSystemUserToAudits()
        {
            _context.AddAuditCustomField(CustomFields.AppUser, AuditService.AuditUserSystem);
        }

        public async Task<List<Notification>> AddLinkedNotificationsAsync(List<Notification> notifications)
        {
            if (notifications.Count > 1)
            {
                var group = new NotificationGroup();
                await _context.NotificationGroup.AddAsync(group);
                notifications.ForEach(n => n.Group = group);
            }

            await _context.Notification.AddRangeAsync(notifications);
            _context.AddAuditCustomField(CustomFields.AuditDetails, NotificationAuditType.Imported);
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<LegacyImportMigrationRun> CreateLegacyImportMigrationRun(IList<string> legacyIds,
            string fileName = null, string description = null)
        {
            var legacyIdsString = legacyIds == null ? null : string.Join(", ", legacyIds);

            var run = CreateBasicLegacyImportMigrationRun(description);
            run.LegacyIdList = legacyIdsString;
            run.FileName = fileName;

            await _context.LegacyImportMigrationRun.AddAsync(run);
            await _context.SaveChangesAsync();
            return run;
        }

        public async Task<LegacyImportMigrationRun> CreateLegacyImportMigrationRun(DateTime rangeStartDate,
            DateTime rangeEndDate, string description = null)
        {
            var run = CreateBasicLegacyImportMigrationRun(description);
            run.RangeStartDate = rangeStartDate;
            run.RangeEndDate = rangeEndDate;

            await _context.LegacyImportMigrationRun.AddAsync(run);
            await _context.SaveChangesAsync();
            return run;
        }

        public async Task AddLegacyImportNotificationLogMessage(LegacyImportNotificationLogMessage message)
        {
            await _context.LegacyImportNotificationLogMessage.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task AddLegacyImportNotificationLogMessageRange(
            IEnumerable<LegacyImportNotificationLogMessage> messages)
        {
            await _context.LegacyImportNotificationLogMessage.AddRangeAsync(messages);
            await _context.SaveChangesAsync();
        }

        public async Task AddLegacyImportNotificationOutcomeRange(
            IEnumerable<LegacyImportNotificationOutcome> outcomes)
        {
            await _context.LegacyImportNotificationOutcome.AddRangeAsync(outcomes);
            await _context.SaveChangesAsync();
        }

        private LegacyImportMigrationRun CreateBasicLegacyImportMigrationRun(string description) =>
            new LegacyImportMigrationRun
            {
                AppRelease = _configuration.GetValue<string>(Constants.Release),
                StartTime = DateTime.Now,
                Description = description
            };
    }
}
