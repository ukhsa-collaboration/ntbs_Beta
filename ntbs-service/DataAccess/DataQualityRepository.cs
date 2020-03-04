using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        private int MIN_NUMBER_DAYS_NOTIFIED_FOR_ALERT = 45;
        
        public DataQualityRepository(NtbsContext context)
        {
            _context = context;
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityDraftAlerts()
        {
            return await GetBaseNotificationQueryableForAlerts()
                .Where(DataQualityDraftAlert.NotificationQualifiesExpression)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityBirthCountryAlerts()
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(DataQualityBirthCountryAlert.NotificationQualifiesExpression)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityClinicalDatesAlerts()
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(DataQualityClinicalDatesAlert.NotificationQualifiesExpression)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityClusterAlerts()
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(DataQualityClusterAlert.NotificationQualifiesExpression)
                .ToListAsync();
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts()
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            var notificationsInDateRange = await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                .Where(DataQualityTreatmentOutcome12.NotificationInQualifyingDateRangeExpression)
                .ToListAsync();
            return notificationsInDateRange
                .Where(DataQualityTreatmentOutcome12.NotificationInRangeQualifies)
                .ToList();
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts()
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            var notificationsInDateRange = await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                .Where(DataQualityTreatmentOutcome24.NotificationInQualifyingDateRangeExpression)
                .ToListAsync();
            return notificationsInDateRange
                .Where(DataQualityTreatmentOutcome24.NotificationInRangeQualifies)
                .ToList();
        }

        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts()
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            var notificationsInDateRange = await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                .Where(DataQualityTreatmentOutcome36.NotificationInQualifyingDateRangeExpression)
                .ToListAsync();
            return notificationsInDateRange
                .Where(DataQualityTreatmentOutcome36.NotificationInRangeQualifies)
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
