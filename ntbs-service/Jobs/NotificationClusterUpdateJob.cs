using System.Threading.Tasks;
using Hangfire;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class NotificationClusterUpdateJob
    {
        private readonly INotificationClusterService _notificationClusterService;
        private readonly INotificationService _notificationService;

        public NotificationClusterUpdateJob(
            INotificationClusterService notificationClusterService,
            INotificationService notificationService)
        {
            _notificationClusterService = notificationClusterService;
            _notificationService = notificationService;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting notification cluster update job");
            
            var clusterValues = await _notificationClusterService.GetNotificationClusterValues();
            await _notificationService.UpdateNotificationClustersAsync(clusterValues);
            
            Log.Information($"Finishing notification cluster update job");
        }
    }
}
