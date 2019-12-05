using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface INotificationRepository
    {
        IQueryable<Notification> GetQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        IQueryable<Notification> GetRecentNotificationsIQueryable();
        IQueryable<Notification> GetDraftNotificationsIQueryable();
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? notificationId);
        Task<Notification> GetNotificationWithTestsAsync(int notificationId);
        Task<Notification> GetNotificationWithAllInfoAsync(int notificationId);
        Task AddNotificationAsync(Notification notification);
        Task<Notification> GetNotificationAsync(int notificationId);
        Task<IList<Notification>> GetNotificationsByIdsAsync(IList<int> ids);
        Task<IList<int>> GetNotificationIdsByNhsNumber(string nhsNumber);
        Task<NotificationGroup> GetNotificationGroupAsync(int notificationId);
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
            return GetBaseNotificationsIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .OrderByDescending(n => n.SubmissionDate);
        }

        public  IQueryable<Notification> GetDraftNotificationsIQueryable()
        {
            return GetBaseNotificationsIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Draft)
                .OrderByDescending(n => n.SubmissionDate);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .FirstOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public async Task<IList<int>> GetNotificationIdsByNhsNumber(string nhsNumber)
        {
            return await _context.Notification
                .Where(n => (n.NotificationStatus == NotificationStatus.Notified || n.NotificationStatus == NotificationStatus.Denotified)
                            && n.PatientDetails.NhsNumber == nhsNumber)
                .Select(n => n.NotificationId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationWithNotificationSitesAsync(int? notificationId)
        {
            return await GetBannerReadyNotificationsIQueryable()
                .Include(n => n.NotificationSites)
                .FirstOrDefaultAsync(m => m.NotificationId == notificationId);
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
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public IQueryable<Notification> GetQueryableNotificationByStatus(IList<NotificationStatus> statuses)
        {
            return GetBannerReadyNotificationsIQueryable().Where(n => statuses.Contains(n.NotificationStatus));
        }

        public async Task<IList<Notification>> GetNotificationsByIdsAsync(IList<int> ids)
        {
            return await GetBannerReadyNotificationsIQueryable()
                        .Where(n => ids.Contains(n.NotificationId))
                        .ToListAsync();
        }

        public async Task<NotificationGroup> GetNotificationGroupAsync(int notificationId)
        {
            return await _context.Notification
                .Where(n => n.NotificationId == notificationId)
                .Select(n => n.Group)
                .Include(g => g.Notifications)
                .SingleOrDefaultAsync();
        }

        // Adds enough information to display notification banner, which makes it a good
        // base for most notification queries. Can be expanded upon for further pages as needed.
        private IQueryable<Notification> GetBannerReadyNotificationsIQueryable()
        {
            return GetBaseNotificationsIQueryable()
                .Include(n => n.PatientDetails.Country)
                .Include(n => n.PatientDetails.Sex);
        } 

        // The base notification model for use in notifications homepage lists.
        // Can be expanded upon for further pages as needed.
        private IQueryable<Notification> GetBaseNotificationsIQueryable()
        {
            return _context.Notification
                .Where(n => n.NotificationStatus != NotificationStatus.Deleted)
                .Include(n => n.PatientDetails)
                    .ThenInclude(p => p.PostcodeLookup)
                        .ThenInclude(pc => pc.LocalAuthority)
                            .ThenInclude(la => la.LocalAuthorityToPHEC)
                                .ThenInclude(pl => pl.PHEC)
                .Include(n => n.Episode.TBService.PHEC)
                .Include(n => n.Episode.CaseManager);
        }
    }
}
