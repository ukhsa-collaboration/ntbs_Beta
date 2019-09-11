using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface INotificationService
    {
        Task<Notification> GetNotificationAsync(int? id);
        Task UpdatePatientAsync(Notification notification, PatientDetails patientDetails);
        Task UpdateTimelineAsync(Notification notification, ClinicalDetails timeline);
        Task UpdateSitesAsync(Notification notification, IEnumerable<NotificationSite> notificationSites);
        Task UpdateEpisodeAsync(Notification notification, Episode episode);
        Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory history);
    }

    public class NotificationService : INotificationService
    {
        private INotificationRepository repository;
        private NtbsContext context;
        public NotificationService(INotificationRepository repository, NtbsContext context) {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Notification> GetNotificationAsync(int? id) {
            return await repository.GetNotificationAsync(id);
        }

        public async Task UpdatePatientAsync(Notification notification, PatientDetails patient)
        {
            await UpdatePatientFlags(patient);
            context.Attach(notification);
            notification.PatientDetails = patient;

            await context.SaveChangesAsync();
        }

        private async Task UpdatePatientFlags(PatientDetails patient)
        {
            await UpdateUkBorn(patient);
        }

        private async Task UpdateUkBorn(PatientDetails patient)
        {
            var country = await context.GetCountryByIdAsync(patient.CountryId);
            if (country == null) {
                patient.UkBorn = null;
                return;
            }

            switch (country.IsoCode)
            {
                case Countries.UkCode:
                    patient.UkBorn = true;
                    break;
                case Countries.UnknownCode:
                    patient.UkBorn = null;
                    break;
                default:
                    patient.UkBorn = false;
                    break;
            }
        }

        public async Task UpdateTimelineAsync(Notification notification, ClinicalDetails timeline)
        {
            context.Attach(notification);
            notification.ClinicalDetails = timeline;

            await context.SaveChangesAsync();
        }
        
        public async Task UpdateEpisodeAsync(Notification notification, Episode episode)
        {
            context.Attach(notification);
            notification.Episode = episode;

            await context.SaveChangesAsync();
        }

        public async Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory tBHistory)
        {
            context.Attach(notification);
            notification.PatientTBHistory = tBHistory;

            await context.SaveChangesAsync();
        }

        public async Task UpdateSitesAsync(Notification notification, IEnumerable<NotificationSite> notificationSites) {
            var currentSites = context.NotificationSite.Where(ns => ns.NotificationId == notification.NotificationId);
            context.NotificationSite.RemoveRange(currentSites);
            context.NotificationSite.AddRange(notificationSites);
            await context.SaveChangesAsync();
        }
    }
}