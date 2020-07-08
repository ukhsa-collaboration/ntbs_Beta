using System;
using Hangfire.Server;
using Hangfire.Console;
using Serilog;

namespace ntbs_service.DataMigration
{
    public interface IImportLogger
    {
        void LogInformation(PerformContext context, string requestId, string message);
        void LogWarning(PerformContext context, string requestId, string message);
        void LogSuccess(PerformContext context, string requestId, string message);
        void LogImportFailure(PerformContext context, string requestId, string message, Exception exception = null);
        void LogError(PerformContext context, string requestId, string message, Exception exception = null);
    }

    public class ImportLogger : IImportLogger
    {
        public void LogInformation(PerformContext context, string requestId, string message)
        {
            Log.Information($"NOTIFICATION IMPORT - {requestId} - {message}");
            context.WriteLine($"NOTIFICATION IMPORT - {requestId} - {message}");

        }
        public void LogSuccess(PerformContext context, string requestId, string message)
        {
            Log.Information($"NOTIFICATION IMPORT - {requestId} - {message}");

            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine($"NOTIFICATION IMPORT - {requestId} - {message}");
            context.ResetTextColor();
        }

        public void LogImportFailure(PerformContext context, string requestId, string message, Exception exception = null)
        {
            // Import failure is not an error-level event - since failures
            // can be result of typical issues too (e.g. validation)
            
            // The reason for failure itself can report issues at higher log levels if needed.
            Log.Information(exception, $"NOTIFICATION IMPORT - {requestId} - {message}");

            context.SetTextColor(ConsoleTextColor.Red);
            context.WriteLine($"NOTIFICATION IMPORT - {requestId} - {message}");
            if (exception != null)
            {
                context.WriteLine(exception.Message);
            }
            context.ResetTextColor();
        }

        public void LogWarning(PerformContext context, string requestId, string message)
        {
            Log.Warning($"NOTIFICATION IMPORT - {requestId} - {message}");
            
            context.SetTextColor(ConsoleTextColor.Yellow);
            context.WriteLine($"NOTIFICATION IMPORT - {requestId} - {message}");
            context.ResetTextColor();
        }

        public void LogError(PerformContext context, string requestId, string message, Exception exception = null)
        {
            Log.Error($"NOTIFICATION IMPORT - {requestId} - {message}");
            
            context.SetTextColor(ConsoleTextColor.DarkRed);
            context.WriteLine($"NOTIFICATION IMPORT - {requestId} - {message}");
            context.ResetTextColor();
        }
    }
}
