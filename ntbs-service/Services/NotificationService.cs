using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface INotificationService
    {
        Task<IList<Notification>> GetRecentNotificationsAsync(List<string> TBServices);
        Task<IList<Notification>> GetDraftNotificationsAsync(List<string> TBServices);
        Task<Notification> GetNotificationAsync(int? id);
        Task<Notification> GetNotificationWithSocialRisksAsync(int? id);
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? id);
        Task<Notification> GetNotificationWithAllInfoAsync(int? id);
        Task UpdatePatientAsync(Notification notification, PatientDetails patientDetails);
        Task UpdateClinicalDetailsAsync(Notification notification, ClinicalDetails timeline);
        Task UpdateSitesAsync(Notification notification, IEnumerable<NotificationSite> notificationSites);
        Task UpdateEpisodeAsync(Notification notification, Episode episode);
        Task SubmitNotification(Notification notification);
        Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing);
        Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory history);
        Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors riskFactors);
    }

    public class NotificationService : INotificationService
    {
        private INotificationRepository repository;
        private NtbsContext context;
        public NotificationService(INotificationRepository repository, NtbsContext context) {
            this.repository = repository;
            this.context = context;
        }

        public async Task<IList<Notification>> GetRecentNotificationsAsync(List<string> TBServices) {
            return await repository.GetRecentNotificationsAsync(TBServices);
        }

        public async Task<IList<Notification>> GetDraftNotificationsAsync(List<string> TBServices) {
            return await repository.GetDraftNotificationsAsync(TBServices);
        }

        public async Task<Notification> GetNotificationAsync(int? id) {
            return await repository.GetNotificationAsync(id);
        }

        public async Task UpdatePatientAsync(Notification notification, PatientDetails patient)
        {
            await UpdatePatientFlags(patient);
            context.Entry(notification.PatientDetails).CurrentValues.SetValues(patient);

            await UpdateDatabase();
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

        public async Task UpdateClinicalDetailsAsync(Notification notification, ClinicalDetails clinicalDetails)
        {
            context.Entry(notification.ClinicalDetails).CurrentValues.SetValues(clinicalDetails);

            await UpdateDatabase();
        }

        public async Task UpdateEpisodeAsync(Notification notification, Episode episode)
        {
            context.Entry(notification.Episode).CurrentValues.SetValues(episode);

            await UpdateDatabase();
        }

        public async Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing) {
            context.Entry(notification.ContactTracing).CurrentValues.SetValues(contactTracing);

            await UpdateDatabase();
        }

        public async Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory tBHistory)
        {
            context.Entry(notification.PatientTBHistory).CurrentValues.SetValues(tBHistory);

            await UpdateDatabase();
        }

        public async Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors socialRiskFactors)
        {
            UpdateSocialRiskFactorsFlags(socialRiskFactors);
            context.Entry(notification).Reference(p => p.SocialRiskFactors).TargetEntry.CurrentValues.SetValues(socialRiskFactors);

            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorDrugs).TargetEntry.CurrentValues.SetValues(socialRiskFactors.RiskFactorDrugs);

            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorHomelessness).TargetEntry.CurrentValues.SetValues(socialRiskFactors.RiskFactorHomelessness);

            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorImprisonment).TargetEntry.CurrentValues.SetValues(socialRiskFactors.RiskFactorImprisonment);

            await UpdateDatabase();
        }

        private void UpdateSocialRiskFactorsFlags(SocialRiskFactors socialRiskFactors) 
        {
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorDrugs);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorHomelessness);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorImprisonment);
        }

        private void UpdateRiskFactorFlags(RiskFactorDetails riskFactor)
        {
            if (riskFactor.Status != Status.Yes)
            {
                riskFactor.IsCurrent = false;
                riskFactor.InPastFiveYears = false;
                riskFactor.MoreThanFiveYearsAgo = false;
            } 
        }

        public async Task<Notification> GetNotificationWithSocialRisksAsync(int? id)
        {
            return await repository.GetNotificationWithSocialRiskFactorsAsync(id);
        }

        public async Task<Notification> GetNotificationWithNotificationSitesAsync(int? id) 
        {
            return await repository.GetNotificationWithNotificationSitesAsync(id);
        }

        public async Task UpdateSitesAsync(Notification notification, IEnumerable<NotificationSite> notificationSites) 
        {
            var currentSites = context.NotificationSite.Where(ns => ns.NotificationId == notification.NotificationId);

            foreach (var newSite in notificationSites)
            {
                var existingSite = currentSites.FirstOrDefault(s => s.SiteId == newSite.SiteId);
                if (existingSite == null)
                {
                    context.NotificationSite.Add(newSite);
                } else if (existingSite.SiteDescription != newSite.SiteDescription)
                {
                    existingSite.SiteDescription = newSite.SiteDescription;
                }
            }

            var sitesToRemove = currentSites.Where(s => !notificationSites.Select(ns => ns.SiteId).Contains(s.SiteId));
            context.NotificationSite.RemoveRange(sitesToRemove);

            await UpdateDatabase();
        }

        public async Task SubmitNotification(Notification notification)
        {
            context.Attach(notification);
            notification.NotificationStatus = NotificationStatus.Notified;
            notification.SubmissionDate = DateTime.UtcNow;

            await UpdateDatabase(isSubmission: true);
        }
        
        public async Task<Notification> GetNotificationWithAllInfoAsync(int? id) {
            return await repository.GetNotificationWithAllInfoAsync(id);
        }

        private async Task UpdateDatabase(bool? isSubmission = null) {
            AuditType auditType;

            switch (isSubmission)
            {
                case true:
                    auditType = AuditType.Notified;
                    break;
                case false:
                    auditType = AuditType.Denotified;
                    break;
                default:
                    auditType = AuditType.Edit;
                    break;
            }
            context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await context.SaveChangesAsync();
        }
    }
}