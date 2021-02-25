using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class NotificationClusterUpdateJob
    {
        private readonly INotificationClusterRepository _notificationClusterRepository;
        private readonly INotificationService _notificationService;

        public NotificationClusterUpdateJob(
            INotificationClusterRepository notificationClusterRepository,
            INotificationService notificationService)
        {
            _notificationClusterRepository = notificationClusterRepository;
            _notificationService = notificationService;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting notification cluster update job");
            
            var clusterValues = await _notificationClusterRepository.GetNotificationClusterValues();
            await _notificationService.UpdateNotificationClustersAsync(clusterValues);
            
            Log.Information($"Finishing notification cluster update job");
        }
    }
}
