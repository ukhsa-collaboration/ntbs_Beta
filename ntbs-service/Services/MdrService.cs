using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IMdrService
    {
        Task<bool> CreateOrDismissMdrAlert(Notification notification, bool? isCurrentlyMdr = null);
    }
    
    public class MdrService : IMdrService
    {
        private readonly IAlertService _alertService;

        public MdrService(IAlertService alertService)
        {
            _alertService = alertService;
        }

        public async Task<bool> CreateOrDismissMdrAlert(Notification notification, bool? isCurrentlyMdr = null)
        {
            if ((notification.ClinicalDetails?.IsMDRTreatment != true && isCurrentlyMdr == true) 
                || (notification.DrugResistanceProfile.IsMdr && !notification.MDRDetails.MDRDetailsEntered))
            {
                var mdrAlert = new MdrAlert() {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(mdrAlert);
            }
            else if ((notification.ClinicalDetails?.IsMDRTreatment == true && isCurrentlyMdr == false)
                || !notification.DrugResistanceProfile.IsMdr)
            {
                if (notification.MDRDetails.MDRDetailsEntered)
                {
                    return false;
                }
                
                await _alertService.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMDR);
            }

            return true;
        }
    }
}
