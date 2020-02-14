using System;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using Serilog;

namespace ntbs_service.Services
{
    public interface IDrugResistanceProfilesService
    {
        Task UpdateDrugResistanceProfiles();
    }

    public class DrugResistanceProfileService : IDrugResistanceProfilesService
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEnhancedSurveillanceAlertsService EnhancedSurveillanceAlertsService;
        private readonly IDrugResistanceProfileRepository _drugResistanceProfileRepository;

        public DrugResistanceProfileService(
            INotificationService notificationService, 
            INotificationRepository notificationRepository,
            IDrugResistanceProfileRepository drugResistanceProfileRepository,
            IEnhancedSurveillanceAlertsService enhancedSurveillanceAlertsService)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _drugResistanceProfileRepository = drugResistanceProfileRepository;
            EnhancedSurveillanceAlertsService = enhancedSurveillanceAlertsService;
        }

        public async Task UpdateDrugResistanceProfiles()
        {
            var drugResistanceProfiles = await _drugResistanceProfileRepository.GetDrugResistanceProfilesAsync();
            
            foreach (var (notificationId, drugResistanceProfile) in drugResistanceProfiles)
            {
                var notification = await _notificationRepository.GetNotificationAsync(notificationId);

                // No notifications found with given notification id
                if (notification == null)
                {
                    continue;
                }
                
                // There are no changes in Drug resistance profile details
                if (notification.DrugResistanceProfile.Species == drugResistanceProfile.Species &&
                    notification.DrugResistanceProfile.DrugResistanceProfileString == drugResistanceProfile.DrugResistanceProfileString)
                {
                    continue;
                }

                try
                {
                    await _notificationService.UpdateDrugResistanceProfile(notification, drugResistanceProfile);
                    await EnhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(notification);
                    await EnhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(notification);
                }
                catch (Exception e)
                {
                    Log.Warning("Error occured when updating drug resistance profile", e);
                }
                
            }
        }
    }
}
