using System.Data;
using System.Threading.Tasks;
using ntbs_service.DataAccess;

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
        private readonly IEnhancedSurveillanceAlertsService _enhancedSurveillanceAlertsService;
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
            _enhancedSurveillanceAlertsService = enhancedSurveillanceAlertsService;
        }

        public async Task UpdateDrugResistanceProfiles()
        {
            var drugResistanceProfiles = await _drugResistanceProfileRepository.GetDrugResistanceProfilesAsync();
            
            foreach (var (notificationId, drugResistanceProfile) in drugResistanceProfiles)
            {
                var notification = await _notificationRepository.GetNotificationAsync(notificationId);
                if (notification == null)
                {
                    throw new DataException(
                        $"Reporting database sourced NotificationId {notificationId} was not found in NTBS database.");
                }
                
                if (notification.DrugResistanceProfile.Species == drugResistanceProfile.Species &&
                    notification.DrugResistanceProfile.DrugResistanceProfileString == drugResistanceProfile.DrugResistanceProfileString)
                {
                    continue;
                }
                
                await _notificationService.UpdateDrugResistanceProfileAsync(notification, drugResistanceProfile);
                await _enhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(notification);
                await _enhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(notification);
            }
        }
    }
    
    class MockDrugResistanceProfilesService : IDrugResistanceProfilesService
    {
        public Task UpdateDrugResistanceProfiles()
        {
            return Task.CompletedTask;
        }
    }

}
