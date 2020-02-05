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
        private readonly IMdrService _mdrService;
        private readonly IDrugResistanceProfileRepository _drugResistanceProfileRepository;

        public DrugResistanceProfileService(
            INotificationService notificationService, 
            INotificationRepository notificationRepository,
            IDrugResistanceProfileRepository drugResistanceProfileRepository,
            IMdrService mdrService)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _drugResistanceProfileRepository = drugResistanceProfileRepository;
            _mdrService = mdrService;
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
                    await _mdrService.CreateOrDismissMdrAlert(notification);
                }
                catch (Exception e)
                {
                    Log.Warning("Error occured when updating drug resistance profile", e);
                }
                
            }
        }
    }
}
