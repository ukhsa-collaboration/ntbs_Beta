using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IDataQualityRepository
    {
        // Draft Alerts
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityDraftAlerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityDraftAlerts(int count, int offset);
        Task<int> GetNotificationsEligibleForDataQualityDraftAlertsCount();
        
        // Birth Country Alerts
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityBirthCountryAlerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityBirthCountryAlerts(int count, int offset);
        Task<int> GetNotificationsEligibleForDataQualityBirthCountryAlertsCount();
        
        // Clinical Dates Alerts
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityClinicalDatesAlerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityClinicalDatesAlerts(int count, int offset);
        Task<int> GetNotificationsEligibleForDataQualityClinicalDatesAlertsCount();
        
        // Cluster Alerts
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityClusterAlerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityClusterAlerts(
            int count,
            int offset);
        Task<int> GetNotificationsEligibleForDataQualityClusterAlertsCount();
        
        // DOT/VOT Alerts
        Task<IList<Notification>> GetNotificationsEligibleForDotVotAlerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDotVotAlerts(int count, int offset);
        Task<int> GetNotificationsEligibleForDotVotAlertsCount();
        
        // Treatment Outcome 12
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome12Alerts(int count, int offset);
        Task<int> GetNotificationsEligibleForDqTreatmentOutcome12AlertsCount();
        
        // Treatment Outcome 24
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome24Alerts(int count, int offset);
        Task<int> GetNotificationsEligibleForDqTreatmentOutcome24AlertsCount();
        // Treatment Outcome 36
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts();
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome36Alerts(int count, int offset);
        Task<int> GetNotificationsEligibleForDqTreatmentOutcome36AlertsCount();
        
        // Potential Duplicates
        Task<IList<DataQualityRepository.NotificationAndDuplicateIds>> GetNotificationIdsEligibleForPotentialDuplicateAlerts();
    }

    public class DataQualityRepository : IDataQualityRepository
    {
        private readonly NtbsContext _context;
        private const int MIN_NUMBER_DAYS_NOTIFIED_FOR_ALERT = 45;

        public DataQualityRepository(NtbsContext context)
        {
            _context = context;
        }

        #region Draft Alerts
        public Task<IList<Notification>> GetNotificationsEligibleForDataQualityDraftAlerts() =>
            GetNotificationsEligibleForAlerts(DataQualityDraftAlert.NotificationQualifiesExpression);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityDraftAlerts(int count, int offset) =>
            GetMultipleNotificationsEligibleForAlertsWithOffset(
                DataQualityDraftAlert.NotificationQualifiesExpression,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDataQualityDraftAlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityDraftAlert.NotificationQualifiesExpression);
        
        #endregion
        
        #region Birth Country Alerts
        public Task<IList<Notification>> GetNotificationsEligibleForDataQualityBirthCountryAlerts() =>
            GetNotificationsEligibleForAlerts(DataQualityBirthCountryAlert.NotificationQualifiesExpression);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityBirthCountryAlerts(int count, int offset) =>
            GetMultipleNotificationsEligibleForAlertsWithOffset(
                DataQualityBirthCountryAlert.NotificationQualifiesExpression,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDataQualityBirthCountryAlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityBirthCountryAlert.NotificationQualifiesExpression);
        
        #endregion
        
        #region Clinical Dates Alerts
        public Task<IList<Notification>> GetNotificationsEligibleForDataQualityClinicalDatesAlerts() =>
            GetNotificationsEligibleForAlerts(DataQualityClinicalDatesAlert.NotificationQualifiesExpression);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityClinicalDatesAlerts(int count, int offset) =>
            GetMultipleNotificationsEligibleForAlertsWithOffset(
                DataQualityClinicalDatesAlert.NotificationQualifiesExpression,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDataQualityClinicalDatesAlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityClinicalDatesAlert.NotificationQualifiesExpression);

        #endregion
        
        #region Cluster Alerts
        public Task<IList<Notification>> GetNotificationsEligibleForDataQualityClusterAlerts() => 
            GetNotificationsEligibleForAlerts(DataQualityClusterAlert.NotificationQualifiesExpression);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityClusterAlerts(int count, int offset) =>
            GetMultipleNotificationsEligibleForAlertsWithOffset(
                DataQualityClusterAlert.NotificationQualifiesExpression,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDataQualityClusterAlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityClusterAlert.NotificationQualifiesExpression);

        #endregion
        
        #region Dot/Vot Alerts

        public Task<IList<Notification>> GetNotificationsEligibleForDotVotAlerts() =>
            GetNotificationsEligibleForAlerts(DataQualityDotVotAlert.NotificationQualifiesExpression);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDotVotAlerts(int count, int offset) =>
            GetMultipleNotificationsEligibleForAlertsWithOffset(
                DataQualityDotVotAlert.NotificationQualifiesExpression,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDotVotAlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityDotVotAlert.NotificationQualifiesExpression);

        #endregion
        
        #region Treatment Outcomes 12 Months
        public Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts() =>
            GetNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
                DataQualityTreatmentOutcome12.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome12.NotificationInRangeQualifies);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome12Alerts(int count, 
            int offset) => GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
                DataQualityTreatmentOutcome12.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome12.NotificationInRangeQualifies, 
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDqTreatmentOutcome12AlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityTreatmentOutcome12.NotificationInQualifyingDateRangeExpression);
        #endregion
        
        #region Treatment Outcomes 24 Months
        public Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts() =>
            GetNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
                DataQualityTreatmentOutcome24.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome24.NotificationInRangeQualifies);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome24Alerts(int count,
            int offset) => GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
                DataQualityTreatmentOutcome24.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome24.NotificationInRangeQualifies, 
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDqTreatmentOutcome24AlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityTreatmentOutcome24.NotificationInQualifyingDateRangeExpression);
        
        #endregion
        
        #region Treatment Outcomes 36 Months
        public Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts() =>
            GetNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
                DataQualityTreatmentOutcome36.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome36.NotificationInRangeQualifies);

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome36Alerts(int count, 
            int offset) => GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
                DataQualityTreatmentOutcome36.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome36.NotificationInRangeQualifies, 
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDqTreatmentOutcome36AlertsCount() =>
            GetNotificationsEligibleForAlertsCount(DataQualityTreatmentOutcome36.NotificationInQualifyingDateRangeExpression);
        #endregion
        
        private async Task<IList<Notification>> GetNotificationsEligibleForAlerts(Expression<Func<Notification, bool>> expression)
        {
            return await GetBaseNotificationQueryableForAlerts()
                .Where(expression)
                .ToListAsync();
        }
        
        private async Task<IList<Notification>> GetMultipleNotificationsEligibleForAlertsWithOffset(
            Expression<Func<Notification, bool>> expression, int count, int offset)
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(expression)
                .OrderBy(n => n.NotificationId)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
        }
        
        private async Task<int> GetNotificationsEligibleForAlertsCount(Expression<Func<Notification, bool>> expression)
        {
            return await _context.Notification
                .Where(expression)
                .CountAsync();
        }
        
        private async Task<IList<Notification>> GetNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
            Expression<Func<Notification, bool>> notificationInQualifyingDateRangeExpression,
            Func<Notification, bool> notificationInRangeQualifies
        )
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            var notificationsInDateRange = await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                .Where(notificationInQualifyingDateRangeExpression)
                .ToListAsync();
            return notificationsInDateRange
                .Where(notificationInRangeQualifies)
                .ToList();    
        }
        
        private async Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts(
            Expression<Func<Notification, bool>> notificationInQualifyingDateRangeExpression,
            Func<Notification, bool> notificationInRangeQualifies,
            int count,
            int offset
        )
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            var notificationsInDateRange = await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts()
                .Where(notificationInQualifyingDateRangeExpression)
                .OrderBy(n => n.NotificationId)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
            return notificationsInDateRange
                .Where(notificationInRangeQualifies)
                .ToList();    
        }

        public async Task<IList<NotificationAndDuplicateIds>> GetNotificationIdsEligibleForPotentialDuplicateAlerts()
        {
            return await _context.Notification
                .SelectMany(_ => _context.Notification, 
                    (notification, duplicate) => new {notification, duplicate})
                .Where(t =>
                    // Check that one of the following cases are true:
                    // The notification's names match directly (and it is not matching on itself)
                    ((t.notification.PatientDetails.GivenName == t.duplicate.PatientDetails.GivenName &&
                     t.notification.PatientDetails.FamilyName == t.duplicate.PatientDetails.FamilyName)
                    ||
                    // The notification's given and family names match but are reversed
                    (t.notification.PatientDetails.GivenName == t.duplicate.PatientDetails.FamilyName &&
                     t.notification.PatientDetails.FamilyName == t.duplicate.PatientDetails.GivenName)
                    ||
                    // The notification's date of births match
                    (t.notification.PatientDetails.Dob == t.duplicate.PatientDetails.Dob))
                    &&
                    // Then check that the notification's nhs numbers match, the notification is notified and the notifications
                    // have been notified for at least the minimum amount of time specified
                    t.notification.PatientDetails.NhsNumber == t.duplicate.PatientDetails.NhsNumber &&
                    t.notification.PatientDetails.NhsNumber != null &&
                    t.notification.NotificationId != t.duplicate.NotificationId &&
                    t.notification.NotificationDate < DateTime.Now.AddDays(-DataQualityPotentialDuplicateAlert.MinNumberDaysNotifiedForAlert) &&
                    t.duplicate.NotificationDate < DateTime.Now.AddDays(-DataQualityPotentialDuplicateAlert.MinNumberDaysNotifiedForAlert) &&
                    t.notification.NotificationStatus == NotificationStatus.Notified &&
                    t.duplicate.NotificationStatus == NotificationStatus.Notified &&
                    (t.notification.GroupId != t.duplicate.GroupId ||
                    t.notification.GroupId == null))
                .Select(t => new NotificationAndDuplicateIds
                {
                    NotificationId = t.notification.NotificationId,
                    DuplicateId = t.duplicate.NotificationId
                }).ToListAsync();
        }

        public class NotificationAndDuplicateIds
        {
            public int NotificationId { get; set; }
            public int DuplicateId { get; set; }
        }

        private IQueryable<Notification> GetBaseNotificationQueryableForAlerts()
        {
            return _context.Notification
                .Include(n => n.HospitalDetails)
                .Where(n => n.NotificationStatus != NotificationStatus.Closed
                            && n.NotificationStatus != NotificationStatus.Deleted
                            && n.NotificationStatus != NotificationStatus.Denotified);
        }
        
        private IQueryable<Notification> GetBaseNotificationQueryableWithTreatmentEventsForAlerts()
        {
            return GetBaseNotificationQueryableForAlerts()
                .Include(n => n.TreatmentEvents)
                    .ThenInclude(t => t.TreatmentOutcome);
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
