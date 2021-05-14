using Serilog;

namespace ntbs_service.Services
{
    // This class is a thin wrapper around Serilog which we can use when testing the logging behaviour
    public interface ILogService
    {
        void LogWarning(string message);
    }
    
    public class LogService : ILogService
    {
        public void LogWarning(string message)
        {
            Log.Warning(message);
        }
    }
}
