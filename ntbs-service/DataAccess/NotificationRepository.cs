using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface INotificationRepository
    {
        Task SaveChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited,
            string auditUserOverride = null);

        Task AddNotificationAsync(Notification notification);
        IQueryable<Notification> GetBaseNotificationsIQueryable();
        IQueryable<Notification> GetQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        IQueryable<Notification> GetRecentNotificationsIQueryable();
        IQueryable<Notification> GetDraftNotificationsIQueryable();
        Task<Notification> GetNotificationWithNotificationSitesAsync(int notificationId);
        Task<Notification> GetNotificationWithCaseManagerTbServicesAsync(int notificationId);
        Task<Notification> GetNotificationWithTestsAsync(int notificationId);
        Task<Notification> GetNotificationWithSocialContextAddressesAsync(int notificationId);
        Task<Notification> GetNotificationWithSocialContextVenuesAsync(int notificationId);
        Task<Notification> GetNotificationWithTreatmentEventsAsync(int notificationId);
        Task<Notification> GetNotificationWithMBovisExposureToKnownCasesAsync(int notificationId);
        Task<Notification> GetNotificationWithMBovisUnpasteurisedMilkConsumptionAsync(int notificationId);
        Task<Notification> GetNotificationWithMBovisOccupationExposureAsync(int notificationId);
        Task<Notification> GetNotificationWithMBovisAnimalExposuresAsync(int notificationId);
        Task<Notification> GetNotificationWithAllInfoAsync(int notificationId);
        Task<Notification> GetNotificationAsync(int notificationId);
        Task<Notification> GetNotifiedNotificationAsync(int notificationId);
        Task<Notification> GetNotificationForAlertCreationAsync(int notificationId);
        Task<IEnumerable<NotificationBannerModel>> GetNotificationBannerModelsByIdsAsync(IList<int> ids);
        Task<IEnumerable<Notification>> GetInactiveNotificationsToCloseAsync();
        Task<IList<int>> GetNotificationIdsByNhsNumberAsync(string nhsNumber);
        Task<NotificationGroup> GetNotificationGroupAsync(int notificationId);
        Task<bool> NotificationWithLegacyIdExistsAsync(string id);
        Task<int> GetNotificationIdByLegacyIdAsync(string legacyId);
        Task<bool> IsNotificationLegacyAsync(int id);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly NtbsContext _context;

        public NotificationRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited,
            string auditUserOverride = null)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            if (auditUserOverride != null)
                _context.AddAuditCustomField(CustomFields.AppUser, auditUserOverride);

            await _context.SaveChangesAsync();
        }
        
        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notification.AddAsync(notification);
            await SaveChangesAsync(NotificationAuditType.Added);
        }

        public IQueryable<Notification> GetRecentNotificationsIQueryable()
        {
            return GetNotificationsWithBasicInformationIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified 
                            || n.NotificationStatus == NotificationStatus.Closed)
                .OrderByDescending(n => n.SubmissionDate);
        }

        public IQueryable<Notification> GetDraftNotificationsIQueryable()
        {
            return GetNotificationsWithBasicInformationIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Draft)
                .OrderByDescending(n => n.SubmissionDate);
        }

        public async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .SingleOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotifiedNotificationAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .SingleOrDefaultAsync(
                    n => n.NotificationId == notificationId
                         && (n.NotificationStatus == NotificationStatus.Notified 
                             || n.NotificationStatus == NotificationStatus.Closed));
        }

        public async Task<Notification> GetNotificationForAlertCreationAsync(int notificationId)
        {
            return await GetBaseNotificationsIQueryable()
                .Include(n => n.HospitalDetails)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<bool> NotificationWithLegacyIdExistsAsync(string id)
        {
            return await _context.Notification
                .AnyAsync(n => n.LTBRID == id || n.ETSID == id);
        }

        public async Task<int> GetNotificationIdByLegacyIdAsync(string legacyId)
        {
            return await _context.Notification
                .Where(n => n.LTBRID == legacyId || n.ETSID == legacyId)
                .Select(n => n.NotificationId)
                .FirstOrDefaultAsync();

        }

        public async Task<bool> IsNotificationLegacyAsync(int id)
        {
            return await _context.Notification
                .Where(n => n.NotificationId == id)
                .AnyAsync(n => n.LTBRID != null || n.ETSID != null);
        }

        public async Task<IList<int>> GetNotificationIdsByNhsNumberAsync(string nhsNumber)
        {
            return await _context.Notification
                .Where(n => (n.NotificationStatus == NotificationStatus.Notified 
                             || n.NotificationStatus == NotificationStatus.Closed
                             || n.NotificationStatus == NotificationStatus.Denotified)
                            && n.PatientDetails.NhsNumber == nhsNumber)
                .Select(n => n.NotificationId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationWithNotificationSitesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.NotificationSites)
                .SingleOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithCaseManagerTbServicesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.HospitalDetails.CaseManager.CaseManagerTbServices)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithTestsAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.TestData.ManualTestResults)
                    .ThenInclude(t => t.ManualTestType.ManualTestTypeSampleTypes)
                        .ThenInclude(t => t.SampleType)
                .Include(n => n.TestData.ManualTestResults)
                    .ThenInclude(t => t.SampleType)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithSocialContextAddressesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.SocialContextAddresses)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithSocialContextVenuesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.SocialContextVenues)
                    .ThenInclude(s => s.VenueType)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithTreatmentEventsAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithMBovisExposureToKnownCasesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.MBovisDetails.MBovisExposureToKnownCases)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithMBovisUnpasteurisedMilkConsumptionAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.MBovisDetails.MBovisUnpasteurisedMilkConsumptions)
                    .ThenInclude(m => m.Country)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);        
        }

        public async Task<Notification> GetNotificationWithMBovisOccupationExposureAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.MBovisDetails.MBovisOccupationExposures)
                    .ThenInclude(m => m.Country)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);           
        }

        public async Task<Notification> GetNotificationWithMBovisAnimalExposuresAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()                
                .Include(n => n.MBovisDetails.MBovisAnimalExposures)
                    .ThenInclude(m => m.Country)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);           
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.PatientDetails).ThenInclude(p => p.Ethnicity)
                .Include(n => n.PatientDetails).ThenInclude(p => p.Occupation)
                .Include(n => n.HospitalDetails).ThenInclude(p => p.Hospital)
                .Include(n => n.HospitalDetails.CaseManager.CaseManagerTbServices)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorDrugs)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorHomelessness)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorImprisonment)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorSmoking)
                .Include(n => n.NotificationSites).ThenInclude(x => x.Site)
                .Include(n => n.TestData.ManualTestResults)
                    .ThenInclude(r => r.ManualTestType.ManualTestTypeSampleTypes)
                        .ThenInclude(t => t.SampleType)
                .Include(n => n.TestData.ManualTestResults).ThenInclude(r => r.SampleType)
                .Include(n => n.TravelDetails.Country1)
                .Include(n => n.TravelDetails.Country2)
                .Include(n => n.TravelDetails.Country3)
                .Include(n => n.VisitorDetails.Country1)
                .Include(n => n.VisitorDetails.Country2)
                .Include(n => n.VisitorDetails.Country3)
                .Include(n => n.MDRDetails.Country)
                .Include(n => n.SocialContextAddresses)
                .Include(n => n.SocialContextVenues).ThenInclude(s => s.VenueType)
                .Include(n => n.PreviousTbHistory).ThenInclude(ph => ph.PreviousTreatmentCountry)
                .Include(n => n.Alerts)
                .Include(n => n.TreatmentEvents)
                    .ThenInclude(t => t.TbService)
                .Include(n => n.TreatmentEvents)
                    .ThenInclude(t => t.CaseManager)
                .Include(n => n.MBovisDetails.MBovisExposureToKnownCases)
                .Include(n => n.MBovisDetails.MBovisUnpasteurisedMilkConsumptions)
                    .ThenInclude(m => m.Country)
                .Include(n => n.MBovisDetails.MBovisOccupationExposures)
                    .ThenInclude(m => m.Country)
                .Include(n => n.MBovisDetails.MBovisAnimalExposures)
                    .ThenInclude(m => m.Country)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public IQueryable<Notification> GetQueryableNotificationByStatus(IList<NotificationStatus> statuses)
        {
            return GetBannerReadyNotificationsIQueryable().Where(n => statuses.Contains(n.NotificationStatus));
        }

        public async Task<IEnumerable<NotificationBannerModel>> GetNotificationBannerModelsByIdsAsync(IList<int> ids)
        {
            return (await GetBannerReadyNotificationsIQueryable()
                    .Where(n => ids.Contains(n.NotificationId))
                    .ToListAsync())
                .Select(n => new NotificationBannerModel(n, showLink: true));
        }

        public async Task<IEnumerable<Notification>> GetInactiveNotificationsToCloseAsync()
        {
            return (await _context.Notification
                .Include(n => n.TreatmentEvents)
                    .ThenInclude(t => t.TreatmentOutcome)
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .Where(n => n.TreatmentEvents.Any(t => t.TreatmentOutcome != null))
                .ToListAsync())
                .Where(n =>
                {
                    var lastTreatmentEvent = n.TreatmentEvents.OrderByDescending(t => t.EventDate)
                        .ThenBy(t => t.TreatmentEventTypeIsOutcome).FirstOrDefault();
                    if (lastTreatmentEvent != null)
                    {
                        return lastTreatmentEvent.TreatmentEventTypeIsOutcome
                               && lastTreatmentEvent.TreatmentOutcome.TreatmentOutcomeSubType !=
                               TreatmentOutcomeSubType.StillOnTreatment
                               && lastTreatmentEvent.EventDate < DateTime.Today.AddYears(-1);
                    }
                    return false;
                });
        }

        public async Task<NotificationGroup> GetNotificationGroupAsync(int notificationId)
        {
            return await _context.Notification
                .Where(n => n.NotificationId == notificationId)
                .Select(n => n.Group)
                .Include(g => g.Notifications)
                    .ThenInclude(n => n.PatientDetails)
                        .ThenInclude(p => p.PostcodeLookup)
                            .ThenInclude(l => l.LocalAuthority)
                                .ThenInclude(la => la.LocalAuthorityToPHEC)
                .Include(g => g.Notifications)
                    .ThenInclude(n => n.TreatmentEvents)
                        .ThenInclude(t => t.TreatmentOutcome)
                .Include(g => g.Notifications)
                    .ThenInclude(n => n.HospitalDetails)
                        .ThenInclude(e => e.TBService)
                .Include(g => g.Notifications)
                    .ThenInclude(n => n.HospitalDetails)
                        .ThenInclude(e => e.CaseManager)
                .SingleOrDefaultAsync();
        }

        // Adds enough information to display notification banner, which makes it a good
        // base for most notification queries. Can be expanded upon for further pages as needed.
        private IQueryable<Notification> GetBannerReadyNotificationsIQueryable()
        {
            return GetNotificationsWithBasicInformationIQueryable()
                .Include(n => n.PatientDetails.Country)
                .Include(n => n.PatientDetails.Sex)
                .Include(n => n.SocialContextAddresses)
                .Include(n => n.SocialContextVenues)
                .Include(n => n.TreatmentEvents)
                    .ThenInclude(t => t.TreatmentOutcome);
        }

        // Gets Notification model with basic information for use in notifications homepage lists.
        // Can be expanded upon for further pages as needed.
        private IQueryable<Notification> GetNotificationsWithBasicInformationIQueryable()
        {
            return GetBaseNotificationsIQueryable()
                .Include(n => n.PatientDetails)
                    .ThenInclude(p => p.PostcodeLookup)
                        .ThenInclude(pc => pc.LocalAuthority)
                            .ThenInclude(la => la.LocalAuthorityToPHEC)
                                .ThenInclude(pl => pl.PHEC)
                .Include(n => n.HospitalDetails.TBService.PHEC)
                .Include(n => n.HospitalDetails.CaseManager);
        }

        public IQueryable<Notification> GetBaseNotificationsIQueryable()
        {
            return _context.Notification
                .Where(n => n.NotificationStatus != NotificationStatus.Deleted);
        }
    }
}
