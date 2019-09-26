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
        Task<SearchResults> GetSearchResultsAsync(int offset, int pageSize);
        Task<IList<Notification>> GetRecentNotificationsAsync(List<string> TBServices);
        Task<IList<Notification>> GetDraftNotificationsAsync(List<string> TBServices);
        Task<IList<Notification>> GetNotificationsWithPatientsAsync();
        Task<Notification> GetNotificationWithSocialRiskFactorsAsync(int? NotificationId);
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? NotificationId);
        Task<Notification> GetNotificationWithAllInfoAsync(int? NotificationId);

        Task UpdateNotificationAsync(Notification Notification);
        Task AddNotificationAsync(Notification Notification);
        Task DeleteNotificationAsync(Notification Notification);
        Task<Notification> GetNotificationAsync(int? NotificationId);
        Task<Notification> FindNotificationByIdAsync(int? NotificationId);
        bool NotificationExists(int NotificationId);
    }
    
    public class NotificationRepository : INotificationRepository 
    {
        private readonly NtbsContext context;
        public NotificationRepository(NtbsContext context) 
        {
            this.context = context;
        }

        public async Task<SearchResults> GetSearchResultsAsync(int offset, int pageSize) {
            IList<Notification> Notifications = await GetBaseNotification()
                .OrderByDescending(n => n.CreationDate)
                .OrderByDescending(n => n.SubmissionDate)
                .OrderBy(n => n.NotificationStatus)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
            int Count = await GetBaseNotification().CountAsync();
            return new SearchResults {notifications = Notifications, numberOfResults = Count};
        }

        public async Task<IList<Notification>> GetRecentNotificationsAsync(List<string> TBServices)
        {
            return await context.Notification
                .Include(n => n.Episode).ThenInclude(p => p.TBService)
                .Where(n => TBServices.Contains(n.Episode.TBService.Name))
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .OrderByDescending(n => n.SubmissionDate)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IList<Notification>> GetDraftNotificationsAsync(List<string> TBServices)
        {
            return await context.Notification
                .Include(n => n.Episode).ThenInclude(p => p.TBService)
                .Where(n => TBServices.Contains(n.Episode.TBService.Name))
                .Where(n => n.NotificationStatus == NotificationStatus.Draft)
                .OrderByDescending(n => n.CreationDate)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsWithPatientsAsync()
        {
            // This is to ensure all the relationships on patients are eagerly fetched
            // We are attempting to load these relationships lazily, but this does not currently seem to work -
            // Country, Sex and Ethnicity are always null. Might be related to PatientDetails being Owned?
            return await context.Notification
                .Include(n => n.PatientDetails).ThenInclude(p => p.Sex)
                .Include(n => n.PatientDetails).ThenInclude(p => p.Country)
                .Include(n => n.PatientDetails).ThenInclude(p => p.Ethnicity)
                .OrderByDescending(notification => notification.NotificationId)
                .ToListAsync();
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
            return await GetBaseNotification()
                .FirstOrDefaultAsync(m => m.NotificationId == NotificationId);
        }

        public async Task<Notification> FindNotificationByIdAsync(int? NotificationId)
        {
            return await context.Notification.FirstOrDefaultAsync(m => m.NotificationId == NotificationId);
        }

        public bool NotificationExists(int NotificationId)
        {
            return context.Notification.Any(e => e.NotificationId == NotificationId);
        }

        public async Task<Notification> GetNotificationWithNotificationSitesAsync(int? NotificationId) {
            return await GetBaseNotification()
                .Include(n => n.NotificationSites)
                .FirstOrDefaultAsync(m => m.NotificationId == NotificationId);
        }

        public async Task<Notification> GetNotificationWithSocialRiskFactorsAsync(int? NotificationId)
        {
            return await GetBaseNotification()
                    .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorDrugs)
                    .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorHomelessness)
                    .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorImprisonment)
                    .FirstOrDefaultAsync(n => n.NotificationId == NotificationId);
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int? NotificationId)
        {
            return await GetBaseNotification()
                .Include(n => n.PatientDetails).ThenInclude(p => p.Ethnicity)
                .Include(n => n.Episode).ThenInclude(p => p.Hospital)
                .Include(n => n.Episode).ThenInclude(p => p.TBService)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorDrugs)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorHomelessness)
                .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorImprisonment)
                .Include(n => n.NotificationSites).ThenInclude(x => x.Site)
                .Include(n => n.ClinicalDetails)
                .Include(n => n.ContactTracing)
                .Include(n => n.PatientTBHistory)
                .FirstOrDefaultAsync(n => n.NotificationId == NotificationId);
        }

        public IIncludableQueryable<Notification, TBService> GetBaseNotification() {
            return context.Notification
                .Include(n => n.PatientDetails).ThenInclude(p => p.Sex)
                .Include(n => n.PatientDetails).ThenInclude(p => p.Country)
                .Include(n => n.Episode).ThenInclude(p => p.TBService);
        }
    }
}