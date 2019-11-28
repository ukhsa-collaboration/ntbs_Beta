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
        IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        IQueryable<Notification> GetRecentNotificationsIQueryable();
        IQueryable<Notification> GetDraftNotificationsIQueryable();
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? notificationId);
        Task<Notification> GetNotificationWithAllInfoAsync(int? notificationId);
        Task UpdateNotificationAsync(Notification notification);
        Task AddNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(Notification notification);
        Task<Notification> GetNotificationAsync(int? notificationId);
        Task<IList<Notification>> GetNotificationsByIdsAsync(IList<int> ids);
        bool NotificationExists(int notificationId);
        Task<IList<int>> GetNotificationIdsByNhsNumber(string nhsNumber);
        Task<NotificationGroup> GetNotificationGroupAsync(int notificationId);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly NtbsContext _context;

        public NotificationRepository(NtbsContext context)
        {
            this._context = context;
        }

        public IQueryable<Notification> GetRecentNotificationsIQueryable()
        {
            return GetBaseNotificationIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .OrderByDescending(n => n.SubmissionDate);
        }

        public  IQueryable<Notification> GetDraftNotificationsIQueryable()
        {
            return GetBaseNotificationIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Draft)
                .OrderByDescending(n => n.SubmissionDate);
        }
        public async Task UpdateNotificationAsync(Notification notification)
        {
            _context.Attach(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Notification notification)
        {
            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<Notification> GetNotificationAsync(int? notificationId)
        {
            return await GetBaseNotificationIQueryable()
                .FirstOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public bool NotificationExists(int notificationId)
        {
            return _context.Notification
                .Any(e => e.NotificationId == notificationId);
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
            return await GetBaseNotificationIQueryable()
                .Include(n => n.NotificationSites)
                .FirstOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int? notificationId)
        {
            return await GetBaseNotificationIQueryable()
                .Include(n => n.PatientDetails).ThenInclude(p => p.Ethnicity)
                .Include(n => n.PatientDetails).ThenInclude(p => p.Occupation)
                .Include(n => n.Episode).ThenInclude(p => p.Hospital)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorDrugs)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorHomelessness)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorImprisonment)
                .Include(n => n.NotificationSites).ThenInclude(x => x.Site)
                .Include(n => n.TravelDetails.Country1)
                .Include(n => n.TravelDetails.Country2)
                .Include(n => n.TravelDetails.Country3)
                .Include(n => n.VisitorDetails.Country1)
                .Include(n => n.VisitorDetails.Country2)
                .Include(n => n.VisitorDetails.Country3)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses)
        {
            return GetBaseNotificationIQueryable().Where(n => statuses.Contains(n.NotificationStatus));
        }

        public async Task<IList<Notification>> GetNotificationsByIdsAsync(IList<int> ids)
        {
            return await GetBaseNotificationIQueryable()
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

        private IQueryable<Notification> GetBaseNotificationIQueryable()
        {
            return _context.Notification
                .Where(n => n.NotificationStatus != NotificationStatus.Deleted)
                .Include(n => n.PatientDetails)
                    .ThenInclude(p => p.Sex)
                .Include(n => n.PatientDetails)
                    .ThenInclude(p => p.Country)
                .Include(n => n.PatientDetails)
                    .ThenInclude(p => p.PostcodeLookup)
                        .ThenInclude(pc => pc.LocalAuthority)
                            .ThenInclude(la => la.LocalAuthorityToPHEC)
                                .ThenInclude(pl => pl.PHEC)
                .Include(n => n.Episode)
                    .ThenInclude(p => p.TBService)
                        .ThenInclude(p => p.PHEC)
                .Include(n => n.Episode)
                    .ThenInclude(p => p.CaseManager)
                        .ThenInclude(p => p.CaseManagerTbServices);
        }
    }
}
