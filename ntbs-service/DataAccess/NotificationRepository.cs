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
        IQueryable<Notification> GetBaseNotificationIQueryable();
        IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        Task<IList<Notification>> GetRecentNotificationsAsync(IEnumerable<TBService> TBServices);
        Task<IList<Notification>> GetDraftNotificationsAsync(IEnumerable<TBService> TBServices);
        Task<IList<Notification>> GetNotificationsWithPatientsAsync();
        Task<Notification> GetNotificationWithSocialRiskFactorsAsync(int? NotificationId);
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? NotificationId);
        Task<Notification> GetNotificationWithImmunosuppresionDetailsAsync(int? NotificationId);
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

        public async Task<IList<Notification>> GetRecentNotificationsAsync(IEnumerable<TBService> TBServices)
        {
            var serviceNames = TBServices.Select(tbs => tbs.Name);
            return await context.Notification
                .Include(n => n.Episode).ThenInclude(p => p.TBService)
                .Where(n => serviceNames.Contains(n.Episode.TBService.Name))
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .OrderByDescending(n => n.SubmissionDate)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IList<Notification>> GetDraftNotificationsAsync(IEnumerable<TBService> TBServices)
        {
            var serviceNames = TBServices.Select(tbs => tbs.Name);
            return await context.Notification
                .Include(n => n.Episode).ThenInclude(p => p.TBService)
                .Where(n => serviceNames.Contains(n.Episode.TBService.Name))
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

        public async Task<Notification> GetNotificationWithSocialRiskFactorsAsync(int? NotificationId)
        {
            return await GetBaseNotificationIQueryable()
                    .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorDrugs)
                    .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorHomelessness)
                    .Include(n => n.SocialRiskFactors).ThenInclude(x => x.RiskFactorImprisonment)
                    .FirstOrDefaultAsync(n => n.NotificationId == NotificationId);
        }

        public async Task<Notification> GetNotificationWithImmunosuppresionDetailsAsync(int? NotificationId)
        {
            return await GetBaseNotificationIQueryable()
                .Include(n => n.ImmunosuppressionDetails)
                .FirstOrDefaultAsync(n => n.NotificationId == NotificationId);
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int? NotificationId)
        {
            return await GetBaseNotificationIQueryable()
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
                .Include(n => n.ImmunosuppressionDetails)
                .Include(n => n.TravelDetails.Country1)
                .Include(n => n.TravelDetails.Country2)
                .Include(n => n.TravelDetails.Country3)
                .Include(n => n.VisitorDetails.Country1)
                .Include(n => n.VisitorDetails.Country2)
                .Include(n => n.VisitorDetails.Country3)
                .FirstOrDefaultAsync(n => n.NotificationId == NotificationId);
        }

        public IQueryable<Notification> GetBaseNotificationIQueryable() {
            return context.Notification
                .Include(n => n.PatientDetails).ThenInclude(p => p.Sex)
                .Include(n => n.PatientDetails).ThenInclude(p => p.Country)
                .Include(n => n.Episode).ThenInclude(p => p.TBService);
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
    }
}
