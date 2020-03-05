using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class CloseInactiveNotificationsJob
    {
        private readonly INotificationService _notificationService;

        public CloseInactiveNotificationsJob(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting close inactive notifications job");

            await _notificationService.CloseInactiveNotifications();
            
            Log.Information($"Finishing close inactive notifications job");
        }
    }
}
