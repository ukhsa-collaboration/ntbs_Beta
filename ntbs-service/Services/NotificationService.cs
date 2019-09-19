using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface INotificationService
    {
        Task<Notification> GetNotificationAsync(int? id);
        Task<Notification> GetNotificationWithSocialRisksAsync(int? id);
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? id);
        Task<Notification> GetNotificationWithAllInfoAsync(int? id);
        Task UpdatePatientAsync(Notification notification, PatientDetails patientDetails);
        Task UpdateTimelineAsync(Notification notification, ClinicalDetails timeline);
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

        public async Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing) {
            context.Attach(notification);
            notification.ContactTracing = contactTracing;

            await context.SaveChangesAsync();
        }

        public async Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory tBHistory)
        {
            context.Attach(notification);
            notification.PatientTBHistory = tBHistory;

            await context.SaveChangesAsync();
        }

        public async Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors socialRiskFactors)
        {
            UpdateSocialRiskFactorsFlags(socialRiskFactors);
            var entry = context.Attach(notification);
            context.Entry(notification).Reference(p => p.SocialRiskFactors).TargetEntry.CurrentValues.SetValues(socialRiskFactors);
            context.Entry(notification).Reference(p => p.SocialRiskFactors).TargetEntry.State = EntityState.Modified;

            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorDrugs).TargetEntry.CurrentValues.SetValues(socialRiskFactors.RiskFactorDrugs);
            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorDrugs).TargetEntry.State = EntityState.Modified;

            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorHomelessness).TargetEntry.CurrentValues.SetValues(socialRiskFactors.RiskFactorHomelessness);
            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorHomelessness).TargetEntry.State = EntityState.Modified;

            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorImprisonment).TargetEntry.CurrentValues.SetValues(socialRiskFactors.RiskFactorImprisonment);
            context.Entry(notification.SocialRiskFactors).Reference(p => p.RiskFactorImprisonment).TargetEntry.State = EntityState.Modified;

            await context.SaveChangesAsync();        
        }

        private void UpdateSocialRiskFactorsFlags(SocialRiskFactors socialRiskFactors) 
        {
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorDrugs);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorHomelessness);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorImprisonment);
        }

        private void UpdateRiskFactorFlags(RiskFactorBase riskFactor)
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
            context.NotificationSite.RemoveRange(currentSites);
            context.NotificationSite.AddRange(notificationSites);
            await context.SaveChangesAsync();
        }

        public async Task SubmitNotification(Notification notification)
        {
            context.Attach(notification);
            notification.NotificationStatus = NotificationStatus.Notified;
            notification.SubmissionDate = DateTime.UtcNow;

            await context.SaveChangesAsync();
        }
        
        public async Task<Notification> GetNotificationWithAllInfoAsync(int? id) {
            return await repository.GetNotificationWithAllInfoAsync(id);
        }
    }
}