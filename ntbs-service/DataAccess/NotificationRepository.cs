using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.DataAccess
{
    public interface INotificationRepository
    {
        Task<IList<Notification>> GetNotificationsAsync();
        Task<IList<Notification>> GetNotificationsWithPatientsAsync();
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

        public async Task<IList<Notification>> GetNotificationsAsync() 
        {
            return await context.Notification.ToListAsync();
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
            return await context.Notification.FirstOrDefaultAsync(m => m.NotificationId == NotificationId);
        }

        public async Task<Notification> FindNotificationByIdAsync(int? NotificationId)
        {
            return await context.Notification.FirstOrDefaultAsync(m => m.NotificationId == NotificationId);
        }

        public bool NotificationExists(int NotificationId)
        {
            return context.Notification.Any(e => e.NotificationId == NotificationId);
        }
    }
}