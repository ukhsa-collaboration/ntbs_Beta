using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IDataQualityRepository
    {
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityDraftAlerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityBirthCountryAlerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityClinicalDatesAlerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityClusterAlerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts();
    }

    public class DataQualityRepository : IDataQualityRepository
    {
        private readonly NtbsContext _context;
        private int MIN_NUMBER_DAYS_NOTIFIED_FOR_ALERT = 45;
        private int MIN_NUMBER_DAYS_DRAFT_FOR_ALERT = 90;
        
        public DataQualityRepository(NtbsContext context)
        {
            _context = context;
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityDraftAlerts()
        {
            return await GetBaseNotificationQueryableForAlerts()
                .Where(n => n.NotificationStatus == NotificationStatus.Draft)
                .Where(n => n.CreationDate < DateTime.Now.AddDays(-MIN_NUMBER_DAYS_DRAFT_FOR_ALERT))
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityBirthCountryAlerts()
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(n => n.PatientDetails.CountryId == Countries.UnknownId)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityClinicalDatesAlerts()
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(n => n.ClinicalDetails.SymptomStartDate > n.ClinicalDetails.TreatmentStartDate
                            || n.ClinicalDetails.SymptomStartDate > n.ClinicalDetails.FirstPresentationDate
                            || n.ClinicalDetails.FirstPresentationDate > n.ClinicalDetails.TBServicePresentationDate
                            || n.ClinicalDetails.TBServicePresentationDate > n.ClinicalDetails.DiagnosisDate
                            || n.ClinicalDetails.DiagnosisDate > n.ClinicalDetails.TreatmentStartDate)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityClusterAlerts()
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(n => n.ClusterId != null 
                            && !n.SocialContextAddresses.Any()
                            && !n.SocialContextVenues.Any())
                .ToListAsync();
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts()
        {
            return await GetNotificationsEligibleForTreatmentOutcomeAlertByAgeInMonths(12);
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts()
        {
            return await GetNotificationsEligibleForTreatmentOutcomeAlertByAgeInMonths(24);
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts()
        {
            return await GetNotificationsEligibleForTreatmentOutcomeAlertByAgeInMonths(36);
        }

        private async Task<IList<Notification>> GetNotificationsEligibleForTreatmentOutcomeAlertByAgeInMonths(
            int notificationAgeRequirementInMonths)
        {
            var outcomeDateLowerLimit = notificationAgeRequirementInMonths - 12;
            var outcomeDateUpperLimit = notificationAgeRequirementInMonths;
            
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(n =>
                    n.NotificationDate <= DateTime.Today.AddMonths(-notificationAgeRequirementInMonths)
                    && !n.TreatmentEvents.Any(t =>
                        t.TreatmentEventType == TreatmentEventType.TreatmentOutcome
                        && t.EventDate >= n.NotificationDate.Value.AddMonths(outcomeDateLowerLimit)
                        && t.EventDate < n.NotificationDate.Value.AddMonths(outcomeDateUpperLimit)))
                .ToListAsync();
        }

        private IQueryable<Notification> GetBaseNotificationQueryableForAlerts()
        {
            return _context.Notification
                .Include(n => n.Episode);
        }

        private IQueryable<Notification> GetNotificationQueryableForNotifiedDataQualityAlerts()
        {
            return GetBaseNotificationQueryableForAlerts()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .Where(n => n.NotificationDate < DateTime.Now.AddDays(-MIN_NUMBER_DAYS_NOTIFIED_FOR_ALERT));
        }
    }
}
