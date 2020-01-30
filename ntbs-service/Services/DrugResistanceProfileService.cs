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
        private readonly IReportingService _reportingService;
        private readonly IMdrService _mdrService;

        public DrugResistanceProfileService(
            INotificationService notificationService, 
            INotificationRepository notificationRepository,
            IReportingService reportingService,
            IMdrService mdrService)
        {
            _reportingService = reportingService;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _mdrService = mdrService;
        }

        public async Task UpdateDrugResistanceProfiles()
        {
            var drugResistanceProfiles = await _reportingService.GetDrugResistanceProfiles();
            
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

                if (notification.DrugResistanceProfile == null)
                {
                    notification.DrugResistanceProfile = drugResistanceProfile;
                }
                
                await _mdrService.CreateOrDismissMdrAlert(notification);
                await _notificationService.UpdateDrugResistanceProfile(notification, drugResistanceProfile);
            }
        }
    }
}
