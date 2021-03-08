using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataMigration;
using Serilog;

namespace ntbs_service.Jobs
{
    public class MarkImportedNotificationsAsImportedJob
    {
        private readonly IMigratedNotificationsMarker _notificationsMarker;

        public MarkImportedNotificationsAsImportedJob(IMigratedNotificationsMarker notificationsMarker)
        {
            _notificationsMarker = notificationsMarker;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting mark imported notifications as imported job");

            await _notificationsMarker.BulkMarkNotificationsAsImportedAsync();

            Log.Information($"Finishing mark imported notifications as imported job");
        }
    }
}
