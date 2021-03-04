using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface IDrugResistanceProfilesService
    {
        Task<int> UpdateDrugResistanceProfiles(int maxNumberOfUpdates);
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

        public async Task<int> UpdateDrugResistanceProfiles(int maxNumberOfUpdates)
        {
            var drugResistanceProfiles = await _drugResistanceProfileRepository.GetDrugResistanceProfilesAsync();

            var totalNumberOfProfilesInNeedOfUpdate = drugResistanceProfiles.Count;
            var profilesToUpdateOnThisRun = drugResistanceProfiles.AsEnumerable()
                .Take(maxNumberOfUpdates)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);

            await UpdateDrugResistanceProfiles(profilesToUpdateOnThisRun);

            return totalNumberOfProfilesInNeedOfUpdate > maxNumberOfUpdates
                ? totalNumberOfProfilesInNeedOfUpdate - maxNumberOfUpdates
                : 0;
        }

        private async Task UpdateDrugResistanceProfiles(Dictionary<int, DrugResistanceProfile> drugResistanceProfiles)
        {
            foreach (var (notificationId, drugResistanceProfile) in drugResistanceProfiles)
            {
                var notification = await _notificationRepository.GetNotificationForDrugResistanceImportAsync(notificationId);
                if (notification == null)
                {
                    throw new DataException(
                        $"Reporting database sourced NotificationId {notificationId} was not found in NTBS database.");
                }

                if (notification.DrugResistanceProfile.Species == drugResistanceProfile.Species &&
                    notification.DrugResistanceProfile.DrugResistanceProfileString ==
                    drugResistanceProfile.DrugResistanceProfileString)
                {
                    continue;
                }

                await _notificationService.UpdateDrugResistanceProfileAsync(notification.DrugResistanceProfile,
                    drugResistanceProfile);
                await _enhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(notification);
                await _enhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(notification);
            }
        }
    }

    class MockDrugResistanceProfilesService : IDrugResistanceProfilesService
    {
        public Task<int> UpdateDrugResistanceProfiles(int maxNumberOfUpdates)
        {
            return Task.FromResult(0);
        }
    }

}
