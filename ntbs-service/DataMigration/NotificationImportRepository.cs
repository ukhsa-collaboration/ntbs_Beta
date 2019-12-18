using System.Collections.Generic;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataMigration
{
    public interface INotificationImportRepository
    {
        Task<List<Notification>> AddLinkedNotificationsAsync(List<Notification> notifications);
    }

    public class NotificationImportRepository : INotificationImportRepository
    {
        private readonly NtbsContext _context;

        public NotificationImportRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> AddLinkedNotificationsAsync(List<Notification> notifications)
        {
            var group = new NotificationGroup();
            _context.NotificationGroup.Add(group);

            notifications.ForEach(n => n.Group = group);
            _context.Notification.AddRange(notifications);

            _context.AddAuditCustomField(CustomFields.AuditDetails, NotificationAuditType.Imported);
            await _context.SaveChangesAsync();
            return notifications;
        }    
    }
}