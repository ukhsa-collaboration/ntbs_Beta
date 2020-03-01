using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

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
        private readonly ITreatmentOutcomeService _treatmentOutcomeService;
        private int MIN_NUMBER_DAYS_NOTIFIED_FOR_ALERT = 45;
        private int MIN_NUMBER_DAYS_DRAFT_FOR_ALERT = 90;
        
        public DataQualityRepository(NtbsContext context, ITreatmentOutcomeService treatmentOutcomeService)
        {
            _context = context;
            _treatmentOutcomeService = treatmentOutcomeService;
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
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            return (await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                    .Where(n => (n.ClinicalDetails.TreatmentStartDate ?? n.NotificationDate) <
                                DateTime.Today.AddYears(-1))
                    .ToListAsync())
                .Where(n => _treatmentOutcomeService.IsTreatmentOutcomeMissingAtXYears(n, 1))
                .ToList();
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts()
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            return (await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                    .Where(n => (n.ClinicalDetails.TreatmentStartDate ?? n.NotificationDate) <
                                DateTime.Today.AddYears(-2))
                    .ToListAsync())
                .Where(n => _treatmentOutcomeService.IsTreatmentOutcomeMissingAtXYears(n, 2))
                .ToList();
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts()
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            return (await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                    .Where(n => (n.ClinicalDetails.TreatmentStartDate ?? n.NotificationDate) < DateTime.Today.AddYears(-3))
                    .ToListAsync())
                .Where(n => _treatmentOutcomeService.IsTreatmentOutcomeMissingAtXYears(n, 3))
                .ToList();
        }

        private IQueryable<Notification> GetBaseNotificationQueryableForAlerts()
        {
            return _context.Notification
                .Include(n => n.HospitalDetails);
        }
        
        private IQueryable<Notification> GetBaseNotificationQueryableWithTreatmentEventsForAlerts()
        {
            return GetBaseNotificationQueryableForAlerts()
                .Include(n => n.TreatmentEvents);
        }

        private IQueryable<Notification> GetNotificationQueryableForNotifiedDataQualityAlerts()
        {
            return GetBaseNotificationQueryableForAlerts()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .Where(n => n.NotificationDate < DateTime.Now.AddDays(-MIN_NUMBER_DAYS_NOTIFIED_FOR_ALERT));
        }
        
        private IQueryable<Notification> GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
        {
            return GetBaseNotificationQueryableWithTreatmentEventsForAlerts()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified);
        }
    }
}
