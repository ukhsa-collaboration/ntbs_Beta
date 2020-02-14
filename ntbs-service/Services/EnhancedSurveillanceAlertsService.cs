using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IEnhancedSurveillanceAlertsService
    {
        Task CreateOrDismissMdrAlert(Notification notification);
        Task CreateOrDismissMBovisAlert(Notification notification);
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
        
        public async Task CreateOrDismissMBovisAlert(Notification notification)
        {
            if (notification.IsMBovis)
            {
                await CreateMBovisAlert(notification);
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

        private async Task CreateMBovisAlert(Notification notification)
        {
            var mBovisAlert = new MBovisAlert { NotificationId = notification.NotificationId};
            await _alertService.AddUniqueAlertAsync(mBovisAlert);
        }
    }
}
