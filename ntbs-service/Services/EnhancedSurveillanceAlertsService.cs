using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IEnhancedSurveillanceAlertsService
    {
        Task CreateOrDismissMdrAlert(Notification notification);
        Task CreateOrDismissMbovisAlert(Notification notification);
    }
    
    public class EnhancedSurveillanceAlertsService : IEnhancedSurveillanceAlertsService
    {
        private readonly IAlertService _alertService;

        public EnhancedSurveillanceAlertsService(IAlertService alertService)
        {
            _alertService = alertService;
        }

        public async Task CreateOrDismissMdrAlert(Notification notification)
        {
            if (notification.IsMdr)
            {
                await CreateMdrAlert(notification);
            }
            else
            {
                await _alertService.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMDR);
            }
        }
        
        public async Task CreateOrDismissMbovisAlert(Notification notification)
        {
            if (notification.IsMbovis)
            {
                await CreateMbovisAlert(notification);
            }
            else
            {
                await _alertService.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMDR);
            }
        }
        
        private async Task CreateMdrAlert(Notification notification)
        {
            var mdrAlert = new MdrAlert {NotificationId = notification.NotificationId};
            await _alertService.AddUniqueAlertAsync(mdrAlert);
        }

        private async Task CreateMbovisAlert(Notification notification)
        {
            var mbovisAlert = new MbovisAlert { NotificationId = notification.NotificationId};
            await _alertService.AddUniqueAlertAsync(mbovisAlert);
        }
    }
}
