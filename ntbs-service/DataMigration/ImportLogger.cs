using Serilog;

namespace ntbs_service.DataMigration
{
    public interface IImportLogger
    {
        void LogInformation(string requestId, string message);
        
        void LogWarning(string requestId, string message);
    }

    public class ImportLogger : IImportLogger
    {
        
        public void LogInformation(string requestId, string message)
        {
            Log.Information($"NOTIFICATION IMPORT - {requestId} - {message}");
        }

        public void LogWarning(string requestId, string message)
        {
            Log.Warning($"NOTIFICATION IMPORT - {requestId} - {message}");
        }
    }
}