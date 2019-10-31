using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface INotificationRepository
    {
        IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        Task<IEnumerable<Notification>> GetRecentNotificationsAsync();
        Task<IEnumerable<Notification>> GetDraftNotificationsAsync();
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? NotificationId);
        Task<Notification> GetNotificationWithAllInfoAsync(int? NotificationId);
        Task UpdateNotificationAsync(Notification Notification);
        Task AddNotificationAsync(Notification Notification);
        Task DeleteNotificationAsync(Notification Notification);
        Task<Notification> GetNotificationAsync(int? NotificationId);
        Task<IList<Notification>> GetNotificationsByIdsAsync(IList<int> ids);
        bool NotificationExists(int NotificationId);
    }
    
    public class NotificationRepository : INotificationRepository 
    {
        private readonly NtbsContext context;
        public NotificationRepository(NtbsContext context) 
        {
            this.context = context;
        }

        public async Task<IEnumerable<Notification>> GetRecentNotificationsAsync()
        {
            return await GetBaseNotificationIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .OrderByDescending(n => n.SubmissionDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetDraftNotificationsAsync()
        {
            return await GetBaseNotificationIQueryable()
                .Where(n => n.NotificationStatus == NotificationStatus.Draft)
                .OrderByDescending(n => n.SubmissionDate).ToListAsync();
        }

        public async Task UpdateNotificationAsync(Notification Notification) 
        {
            context.Attach(Notification).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task AddNotificationAsync(Notification Notification) 
        {
            context.Notification.Add(Notification);
            await context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Notification Notification)
        {
            context.Notification.Remove(Notification);
            await context.SaveChangesAsync();
        }

        public async Task<Notification> GetNotificationAsync(int? NotificationId)
        {
            return await GetBaseNotificationIQueryable()
                .FirstOrDefaultAsync(m => m.NotificationId == NotificationId);
        }

        public bool NotificationExists(int NotificationId)
        {
            return context.Notification.Any(e => e.NotificationId == NotificationId);
        }

        public async Task<Notification> GetNotificationWithNotificationSitesAsync(int? NotificationId) {
            return await GetBaseNotificationIQueryable()
                .Include(n => n.NotificationSites)
                .FirstOrDefaultAsync(m => m.NotificationId == NotificationId);
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int? NotificationId)
        {
            return await GetBaseNotificationIQueryable()
                .Include(n => n.PatientDetails).ThenInclude(p => p.Ethnicity)
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
                .FirstOrDefaultAsync(n => n.NotificationId == NotificationId);
        }

        public IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses) {
            return GetBaseNotificationIQueryable().Where(n => statuses.Contains(n.NotificationStatus));
        }

        public async Task<IList<Notification>> GetNotificationsByIdsAsync(IList<int> ids)
        {
            return await GetBaseNotificationIQueryable()
                        .Where(n => ids.Contains(n.NotificationId))
                        .ToListAsync();
        }

        private IQueryable<Notification> GetBaseNotificationIQueryable()
        {
            return context.Notification
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
                        .ThenInclude(p => p.PHEC);
        }
    }
}
