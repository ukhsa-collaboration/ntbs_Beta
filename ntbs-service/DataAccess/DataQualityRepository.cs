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
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqDraftAlertsAsync(int count, int offset);
        Task<int> GetNotificationsEligibleForDqDraftAlertsCountAsync();
        
        // Birth Country Alerts
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqBirthCountryAlertsAsync(int count, int offset);
        Task<int> GetNotificationsEligibleForDqBirthCountryAlertsCountAsync();
        
        // Clinical Dates Alerts
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqClinicalDatesAlertsAsync(int count, int offset);
        Task<int> GetNotificationsEligibleForDqClinicalDatesAlertsCountAsync();
        
        // Cluster Alerts
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqClusterAlertsAsync(
            int count,
            int offset);
        Task<int> GetNotificationsEligibleForDqClusterAlertsCountAsync();
        
        // DOT/VOT Alerts
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqDotVotAlertsAsync(int count, int offset);
        Task<int> GetNotificationsEligibleForDqDotVotAlertsCountAsync();
        
        // Treatment Outcome 12
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome12AlertsAsync(int count, int offset);
        Task<int> GetNotificationsEligibleForDqTreatmentOutcome12AlertsCountAsync();
        
        // Treatment Outcome 24
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome24AlertsAsync(int count, int offset);
        Task<int> GetNotificationsEligibleForDqTreatmentOutcome24AlertsCountAsync();
        // Treatment Outcome 36
        Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome36AlertsAsync(int count, int offset);
        Task<int> GetNotificationsEligibleForDqTreatmentOutcome36AlertsCountAsync();
        
        // Potential Duplicates
        Task<IList<DataQualityRepository.NotificationAndDuplicateIds>> GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync();
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

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqDraftAlertsAsync(
            int count, int offset) =>
                GetMultipleNotificationsEligibleForAlertsWithOffset<DataQualityDraftAlert>(
                    DataQualityDraftAlert.NotificationQualifiesExpression, count, offset);

        public Task<int> GetNotificationsEligibleForDqDraftAlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityDraftAlert.NotificationQualifiesExpression);
        
        #endregion
        
        #region Birth Country Alerts

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqBirthCountryAlertsAsync(
            int count, int offset) =>
                GetMultipleNotificationsEligibleForAlertsWithOffset<DataQualityBirthCountryAlert>(
                    DataQualityBirthCountryAlert.NotificationQualifiesExpression, count, offset);

        public Task<int> GetNotificationsEligibleForDqBirthCountryAlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityBirthCountryAlert.NotificationQualifiesExpression);
        
        #endregion
        
        #region Clinical Dates Alerts

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqClinicalDatesAlertsAsync(
            int count, int offset) =>
                GetMultipleNotificationsEligibleForAlertsWithOffset<DataQualityClinicalDatesAlert>(
                    DataQualityClinicalDatesAlert.NotificationQualifiesExpression, count, offset);

        public Task<int> GetNotificationsEligibleForDqClinicalDatesAlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityClinicalDatesAlert.NotificationQualifiesExpression);

        #endregion
        
        #region Cluster Alerts

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqClusterAlertsAsync(int count, int offset) =>
                GetMultipleNotificationsEligibleForAlertsWithOffset<DataQualityClusterAlert>(
                    DataQualityClusterAlert.NotificationQualifiesExpression, count, offset);

        public Task<int> GetNotificationsEligibleForDqClusterAlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityClusterAlert.NotificationQualifiesExpression);

        #endregion
        
        #region Dot/Vot Alerts

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqDotVotAlertsAsync(int count, int offset) =>
                GetMultipleNotificationsEligibleForAlertsWithOffset<DataQualityDotVotAlert>(
                    DataQualityDotVotAlert.NotificationQualifiesExpression, count, offset);

        public Task<int> GetNotificationsEligibleForDqDotVotAlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityDotVotAlert.NotificationQualifiesExpression);

        #endregion
        
        #region Treatment Outcomes 12 Months

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome12AlertsAsync(
            int count, int offset) => 
            GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts<DataQualityTreatmentOutcome12>(
                DataQualityTreatmentOutcome12.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome12.NotificationInRangeQualifies,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDqTreatmentOutcome12AlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityTreatmentOutcome12.NotificationInQualifyingDateRangeExpression);

        #endregion
        
        #region Treatment Outcomes 24 Months

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome24AlertsAsync(
            int count, int offset) => 
            GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts<DataQualityTreatmentOutcome24>(
                DataQualityTreatmentOutcome24.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome24.NotificationInRangeQualifies,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDqTreatmentOutcome24AlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityTreatmentOutcome24.NotificationInQualifyingDateRangeExpression);

        #endregion
        
        #region Treatment Outcomes 36 Months

        public Task<IList<Notification>> GetMultipleNotificationsEligibleForDqTreatmentOutcome36AlertsAsync(int count, int offset) => 
            GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts<DataQualityTreatmentOutcome36>(
                DataQualityTreatmentOutcome36.NotificationInQualifyingDateRangeExpression,
                DataQualityTreatmentOutcome36.NotificationInRangeQualifies,
                count, 
                offset);

        public Task<int> GetNotificationsEligibleForDqTreatmentOutcome36AlertsCountAsync() =>
            GetNotificationsEligibleForAlertsCount(DataQualityTreatmentOutcome36.NotificationInQualifyingDateRangeExpression);
        #endregion

        private async Task<IList<Notification>> GetMultipleNotificationsEligibleForAlertsWithOffset<T>(
            Expression<Func<Notification, bool>> expression, int count, int offset) where T : Alert
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts<T>()
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

        private async Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityTreatmentOutcomeAlerts<T>(
            Expression<Func<Notification, bool>> notificationInQualifyingDateRangeExpression,
            Func<Notification, bool> notificationInRangeQualifies, 
            int count,
            int offset) where T : Alert
        {
            // IsTreatmentOutcomeMissingAtXYears cannot be translated to SQL so will be calculated in memory so the
            // method has been split up into a DB query and an in memory where statement separated by the ToListAsync call
            var notificationsInDateRange = await GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts<T>()
                .Where(notificationInQualifyingDateRangeExpression)
                .OrderBy(n => n.NotificationId)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
            return notificationsInDateRange
                .Where(notificationInRangeQualifies)
                .ToList();    
        }

        public async Task<IList<NotificationAndDuplicateIds>> GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync()
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

        // Get notifications which doesn't have any alerts with certain alert type
        private IQueryable<Notification> GetBaseNotificationQueryableWithoutAlertWithType<T>() where T : Alert
        {
            return _context.Notification
                .Include(n => n.HospitalDetails)
                .Include(n => n.Alerts)
                .Where(n => n.NotificationStatus != NotificationStatus.Closed
                            && n.NotificationStatus != NotificationStatus.Deleted
                            && n.NotificationStatus != NotificationStatus.Denotified
                            && n.Alerts.All(a => a is T));
        }
        
        private IQueryable<Notification> GetBaseNotificationQueryableWithTreatmentEventsForAlerts<T>() where T : Alert
        {
            return GetBaseNotificationQueryableWithoutAlertWithType<T>()
                .Include(n => n.TreatmentEvents)
                    .ThenInclude(t => t.TreatmentOutcome);
        }

        private IQueryable<Notification> GetNotificationQueryableForNotifiedDataQualityAlerts<T>() where T : Alert
        {
            return GetBaseNotificationQueryableWithoutAlertWithType<T>()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .Where(n => n.NotificationDate < DateTime.Now.AddDays(-MIN_NUMBER_DAYS_NOTIFIED_FOR_ALERT));
        }
        
        private IQueryable<Notification> GetNotificationQueryableForNotifiedTreatmentOutcomeDataQualityAlerts<T>() where T : Alert
        {
            return GetBaseNotificationQueryableWithTreatmentEventsForAlerts<T>()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified);
        }
    }
}
