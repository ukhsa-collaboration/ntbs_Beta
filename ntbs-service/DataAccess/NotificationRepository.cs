using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface INotificationRepository
    {
        IQueryable<Notification> GetQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        IQueryable<Notification> GetRecentNotificationsIQueryable();
        IQueryable<Notification> GetDraftNotificationsIQueryable();

        Task<Notification> GetNotificationWithNotificationSitesAsync(int notificationId);
        Task<Notification> GetNotificationWithCaseManagerTbServices(int notificationId);
        Task<Notification> GetNotificationWithTestsAsync(int notificationId);
        Task<Notification> GetNotificationWithSocialContextAddressesAsync(int notificationId);
        Task<Notification> GetNotificationWithSocialContextVenuesAsync(int notificationId);
        Task<Notification> GetNotificationWithTreatmentEventsAsync(int notificationId);
        Task<Notification> GetNotificationWithAllInfoAsync(int notificationId);
        Task<Notification> GetNotificationAsync(int notificationId);
        Task<Notification> GetNotifiedNotificationAsync(int notificationId);
        Task<Notification> GetNotificationForAlertCreation(int notificationId);
        Task<IEnumerable<NotificationBannerModel>> GetNotificationBannerModelsByIdsAsync(IList<int> ids);
        Task<IList<int>> GetNotificationIdsByNhsNumber(string nhsNumber);
        Task<NotificationGroup> GetNotificationGroupAsync(int notificationId);
        Task<bool> NotificationWithLegacyIdExistsAsync(string id);
        Task<bool> IsNotificationLegacyAsync(int id);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly NtbsContext _context;

        public NotificationRepository(NtbsContext context)
        {
            _context = context;
        }

        public IQueryable<Notification> GetRecentNotificationsIQueryable()
        {
            return GetNotificationsWithBasicInformationIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
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
                .FirstOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotifiedNotificationAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .SingleOrDefaultAsync(
                    n => n.NotificationId == notificationId
                         && n.NotificationStatus == NotificationStatus.Notified);
        }

        public async Task<Notification> GetNotificationForAlertCreation(int notificationId)
        {
            return await GetBaseNotificationsIQueryable()
                .Include(n => n.Episode)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<bool> NotificationWithLegacyIdExistsAsync(string id)
        {
            return await _context.Notification
                .AnyAsync(e => e.LTBRID == id || e.ETSID == id);
        }

        public async Task<bool> IsNotificationLegacyAsync(int id)
        {
            return await _context.Notification.Where(n => n.NotificationId == id)
                .Select(n => n.LTBRID != null || n.ETSID != null)
                .SingleAsync();
        }

        public async Task<IList<int>> GetNotificationIdsByNhsNumber(string nhsNumber)
        {
            return await _context.Notification
                .Where(n => (n.NotificationStatus == NotificationStatus.Notified ||
                             n.NotificationStatus == NotificationStatus.Denotified)
                            && n.PatientDetails.NhsNumber == nhsNumber)
                .Select(n => n.NotificationId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationWithNotificationSitesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.NotificationSites)
                .FirstOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithCaseManagerTbServices(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.Episode.CaseManager.CaseManagerTbServices)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithTestsAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.TestData.ManualTestResults)
                    .ThenInclude(t => t.ManualTestType.ManualTestTypeSampleTypes)
                        .ThenInclude(t => t.SampleType)
                .Include(n => n.TestData.ManualTestResults)
                    .ThenInclude(t => t.SampleType)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithSocialContextAddressesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.SocialContextAddresses)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithSocialContextVenuesAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.SocialContextVenues)
                    .ThenInclude(s => s.VenueType)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithTreatmentEventsAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.TreatmentEvents)
                    .ThenInclude(n => n.TreatmentOutcome)
                .SingleOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.PatientDetails).ThenInclude(p => p.Ethnicity)
                .Include(n => n.PatientDetails).ThenInclude(p => p.Occupation)
                .Include(n => n.Episode).ThenInclude(p => p.Hospital)
                .Include(n => n.Episode.CaseManager.CaseManagerTbServices)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorDrugs)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorHomelessness)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorImprisonment)
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
                .Include(n => n.Alerts)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
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
                    .ThenInclude(n => n.Episode)
                        .ThenInclude(e => e.TBService)
                .SingleOrDefaultAsync();
        }

        // Adds enough information to display notification banner, which makes it a good
        // base for most notification queries. Can be expanded upon for further pages as needed.
        private IQueryable<Notification> GetBannerReadyNotificationsIQueryable()
        {
            return GetNotificationsWithBasicInformationIQueryable()
                .Include(n => n.PatientDetails.Country)
                .Include(n => n.PatientDetails.Sex);
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
                .Include(n => n.Episode.TBService.PHEC)
                .Include(n => n.Episode.CaseManager);
        }

        private IQueryable<Notification> GetBaseNotificationsIQueryable()
        {
            return _context.Notification
                .Where(n => n.NotificationStatus != NotificationStatus.Deleted);
        }
    }
}
