using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IMdrService
    {
        Task CreateOrDismissMdrAlert(Notification notification);
    }
    
    public class MdrService : IMdrService
    {
        private readonly IAlertService _alertService;

        public MdrService(IAlertService alertService)
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
                await DismissMdrAlert(notification);
            }
        }
        
        private async Task CreateMdrAlert(Notification notification)
        {
            var mdrAlert = new MdrAlert() {NotificationId = notification.NotificationId};
            await _alertService.AddUniqueAlertAsync(mdrAlert);
        }

        private async Task DismissMdrAlert(Notification notification)
        {
            await _alertService.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMDR);
        }
    }
}
