using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using Serilog;

namespace ntbs_service.DataMigration
{
    public interface IImportLogger
    {
        void LogInformation(PerformContext context, int runId, string message);

        Task LogNotificationWarning(PerformContext context, int runId, string legacyId, string message);
        Task LogGroupWarning(PerformContext context, int runId, IList<Notification> notificationGroup,
            string message);

        void LogSuccess(PerformContext context, int runId, string message);
        Task LogGroupSuccess(PerformContext context, int runId, IList<Notification> notificationGroup);
        Task LogImportGroupFailure(PerformContext context, int runId, IList<Notification> notificationGroup,
            string message, Exception exception = null);

        Task LogNotificationError(PerformContext context, int runId, string legacyId, string message);
        Task LogGroupError(PerformContext context, int runId, IList<Notification> notificationGroup,
            string message);
    }

    public class ImportLogger : IImportLogger
    {
        private readonly INotificationImportRepository _notificationImportRepository;

        public ImportLogger(INotificationImportRepository notificationImportRepository)
        {
            _notificationImportRepository = notificationImportRepository;
        }

        public void LogInformation(PerformContext context, int runId, string message)
        {
            Log.Information($"NOTIFICATION IMPORT - {runId} - {message}");
            context.WriteLine($"NOTIFICATION IMPORT - {runId} - {message}");

        }
        public void LogSuccess(PerformContext context, int runId, string message)
        {
            Log.Information($"NOTIFICATION IMPORT - {runId} - {message}");

            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine($"NOTIFICATION IMPORT - {runId} - {message}");
            context.ResetTextColor();
        }

        public async Task LogGroupSuccess(PerformContext context, int runId, IList<Notification> notificationGroup)
        {
            await SaveNotificationGroupOutcome(runId, notificationGroup, message: null, successfullyMigrated: true);

            var importMappings =
                string.Join(", ", notificationGroup.Select(n => $"({n.NotificationId}, {n.LegacyId})"));
            var message = $"Imported notifications, in form (NtbsId, LegacyId) - {importMappings}";
            LogSuccess(context, runId, message);

            LogInformation(context, runId,
                $"Finished importing notification group containing legacy ID {notificationGroup.First().LegacyId}");
        }

        public async Task LogImportGroupFailure(PerformContext context, int runId, IList<Notification>
            notificationGroup, string message, Exception exception = null)
        {
            var legacyIds = string.Join(", ", notificationGroup.Select(n => n.LegacyId));
            message = $"Notification group ({legacyIds}) failed to import {message}";

            await SaveNotificationGroupOutcome(runId, notificationGroup, message, successfullyMigrated: false);

            // Import failure is not an error-level event - since failures
            // can be result of typical issues too (e.g. validation)
            // The reason for failure itself can report issues at higher log levels if needed.
            Log.Information(exception, $"NOTIFICATION IMPORT - {runId} - {message}");

            context.SetTextColor(ConsoleTextColor.Red);
            context.WriteLine($"NOTIFICATION IMPORT - {runId} - {message}");
            if (exception != null)
            {
                context.WriteLine(exception.Message);
            }
            context.ResetTextColor();
        }

        private void LogWarning(PerformContext context, int runId, string message)
        {
            Log.Warning($"NOTIFICATION IMPORT - {runId} - {message}");

            context.SetTextColor(ConsoleTextColor.Yellow);
            context.WriteLine($"NOTIFICATION IMPORT - {runId} - {message}");
            context.ResetTextColor();
        }

        public async Task LogNotificationWarning(PerformContext context, int runId, string legacyId, string message)
        {
            await SaveNotificationMessage(runId, legacyId, message, LogMessageLevel.Warning);

            LogWarning(context, runId, $"Legacy notification {legacyId} {message}");
        }

        public async Task LogGroupWarning(PerformContext context, int runId, IList<Notification> notificationGroup,
            string message)
        {
            await SaveNotificationGroupMessage(runId, notificationGroup, message, LogMessageLevel.Error);

            LogWarning(context, runId, message);
        }

        private void LogError(PerformContext context, int runId, string message, Exception exception = null)
        {
            Log.Error($"NOTIFICATION IMPORT - {runId} - {message}");

            context.SetTextColor(ConsoleTextColor.DarkRed);
            context.WriteLine($"NOTIFICATION IMPORT - {runId} - {message}");
            context.ResetTextColor();
        }

        public async Task LogNotificationError(PerformContext context, int runId, string legacyId, string message)
        {
            await SaveNotificationMessage(runId, legacyId, message, LogMessageLevel.Error);

            LogError(context, runId, $"Legacy notification {legacyId} {message}");
        }

        public async Task LogGroupError(PerformContext context, int runId, IList<Notification> notificationGroup,
            string message)
        {
            await SaveNotificationGroupMessage(runId, notificationGroup, message, LogMessageLevel.Error);

            LogError(context, runId, message);
        }

        private async Task SaveNotificationMessage(int runId, string legacyId, string message,
            LogMessageLevel messageLevel)
        {
            var dbMessage = new LegacyImportNotificationLogMessage
            {
                LegacyImportMigrationRunId = runId,
                OldNotificationId = legacyId,
                Message = message,
                LogMessageLevel = messageLevel
            };
            await _notificationImportRepository.AddLegacyImportNotificationLogMessage(dbMessage);
        }

        private async Task SaveNotificationGroupMessage(int runId, IEnumerable<Notification> notificationGroup,
            string message, LogMessageLevel messageLevel)
        {
            var messages = notificationGroup
                .Select(n => new LegacyImportNotificationLogMessage
                {
                    LegacyImportMigrationRunId = runId,
                    OldNotificationId = n.LegacyId,
                    Message = message,
                    LogMessageLevel = messageLevel
                });
            await _notificationImportRepository.AddLegacyImportNotificationLogMessageRange(messages);
        }

        private async Task SaveNotificationGroupOutcome(int runId, IEnumerable<Notification> notificationGroup,
            string message, bool successfullyMigrated)
        {
            var outcomes = notificationGroup
                .Select(n => new LegacyImportNotificationOutcome
                {
                    LegacyImportMigrationRunId = runId,
                    OldNotificationId = n.LegacyId,
                    NtbsId = n.NotificationId,
                    SuccessfullyMigrated = successfullyMigrated,
                    Notes = message
                });
            await _notificationImportRepository.AddLegacyImportNotificationOutcomeRange(outcomes);
        }
    }
}
