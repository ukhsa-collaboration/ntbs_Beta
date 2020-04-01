using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
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
        Task<IList<Notification>> GetNotificationsEligibleForDotVotAlerts();
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

        public async Task<IList<Notification>> GetNotificationsEligibleForDotVotAlerts()
        {
            return await GetNotificationQueryableForNotifiedDataQualityAlerts()
                .Where(DataQualityDotVotAlert.NotificationQualifiesExpression)
                .ToListAsync();
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
