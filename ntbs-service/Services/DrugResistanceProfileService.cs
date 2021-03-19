using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Projections;

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

        private async Task UpdateDrugResistanceProfiles(Dictionary<int, DrugResistanceProfile> updatedDrugResistanceProfiles)
        {
            var drugResistanceProfilesToUpdate =
                (await GetDrugResistanceProfilesWhichDifferInNtbs(updatedDrugResistanceProfiles)).ToList();
            await UpdateDrugResistanceProfiles(drugResistanceProfilesToUpdate);
            var notifications = drugResistanceProfilesToUpdate.Select(t => t.Notification).ToList();
            await CreateOrDismissMdrAlerts(notifications);
            await CreateOrDismissMBovisAlerts(notifications);
        }

        private async Task<IEnumerable<DrugResistanceProfileUpdate>> GetDrugResistanceProfilesWhichDifferInNtbs(
            Dictionary<int, DrugResistanceProfile> drugResistanceProfileUpdates)
        {
            var updatedNotificationIds = drugResistanceProfileUpdates.Keys;
            var notificationsToUpdate =
                (await _notificationRepository.GetNotificationsForDrugResistanceImportAsync(updatedNotificationIds)
                ).ToList();

            var missingNotificationIds = updatedNotificationIds
                .Where(id => !notificationsToUpdate.Select(n => n.NotificationId).Contains(id))
                .ToList();
            if (missingNotificationIds.Any())
            {
                throw new DataException(
                    $"Reporting database sourced NotificationIds {string.Join(", ", missingNotificationIds)} were not found in NTBS database.");
            }

            return notificationsToUpdate
                .Select(n => new DrugResistanceProfileUpdate
                {
                    Notification = n,
                    UpdatedProfile = drugResistanceProfileUpdates[n.NotificationId]
                })
                .Where(update => update.SpeciesHasChanged || update.ProfileStringHasChanged);
        }

        private async Task UpdateDrugResistanceProfiles(List<DrugResistanceProfileUpdate> updates)
        {
            var profileUpdates = updates.Select(u => (u.Notification.DrugResistanceProfile, u.UpdatedProfile));
            await _notificationService.UpdateDrugResistanceProfileAsync(profileUpdates);
        }

        private async Task CreateOrDismissMdrAlerts(List<NotificationForDrugResistanceImport> notifications)
        {
            foreach (var notification in notifications)
            {
                await _enhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(notification);
            }
        }

        private async Task CreateOrDismissMBovisAlerts(List<NotificationForDrugResistanceImport> notifications)
        {
            foreach (var notification in notifications)
            {
                await _enhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(notification);
            }
        }

        private class DrugResistanceProfileUpdate
        {
            public NotificationForDrugResistanceImport Notification { get; set; }
            public DrugResistanceProfile UpdatedProfile { get; set; }

            public bool SpeciesHasChanged => Notification.DrugResistanceProfile.Species != UpdatedProfile.Species;
            public bool ProfileStringHasChanged =>
                Notification.DrugResistanceProfile.DrugResistanceProfileString != UpdatedProfile.DrugResistanceProfileString;
        }
    }

    internal class MockDrugResistanceProfilesService : IDrugResistanceProfilesService
    {
        public Task<int> UpdateDrugResistanceProfiles(int maxNumberOfUpdates)
        {
            return Task.FromResult(0);
        }
    }

}
