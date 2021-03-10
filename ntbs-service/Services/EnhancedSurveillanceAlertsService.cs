using System.Threading.Tasks;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Projections;

namespace ntbs_service.Services
{
    public interface IEnhancedSurveillanceAlertsService
    {
        Task CreateOrDismissMdrAlert(INotificationForDrugResistanceImport notification);
        Task CreateOrDismissMBovisAlert(INotificationForDrugResistanceImport notification);
    }

    public class EnhancedSurveillanceAlertsService : IEnhancedSurveillanceAlertsService
    {
        private readonly IAlertService _alertService;

        public EnhancedSurveillanceAlertsService(IAlertService alertService)
        {
            _alertService = alertService;
        }

        public async Task CreateOrDismissMdrAlert(INotificationForDrugResistanceImport notification)
        {
            if (notification.IsMdr && !notification.MDRDetails.MDRDetailsEntered)
            {
                await CreateMdrAlert(notification);
            }
            else
            {
                await _alertService.DismissMatchingAlertAsync<MdrAlert>(notification.NotificationId);
            }
        }

        public async Task CreateOrDismissMBovisAlert(INotificationForDrugResistanceImport notification)
        {
            if (notification.IsMBovis)
            {
                await CreateMBovisAlert(notification);
            }
            else
            {
                await _alertService.DismissMatchingAlertAsync<MBovisAlert>(notification.NotificationId);
            }
        }

        private async Task CreateMdrAlert(INotificationForDrugResistanceImport notification)
        {
            var mdrAlert = new MdrAlert { NotificationId = notification.NotificationId };
            await _alertService.AddUniqueAlertAsync(mdrAlert);
        }

        private async Task CreateMBovisAlert(INotificationForDrugResistanceImport notification)
        {
            var mBovisAlert = new MBovisAlert { NotificationId = notification.NotificationId };
            await _alertService.AddUniqueAlertAsync(mBovisAlert);
        }
    }
}
