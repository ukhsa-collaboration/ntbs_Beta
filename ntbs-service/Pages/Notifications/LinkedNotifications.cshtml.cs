using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class LinkedNotificationsModel : NotificationModelBase
    {
        public List<Notification> LinkedNotifications { get; set; }

        public LinkedNotificationsModel(INotificationService service) : base(service) {}

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            await GetLinkedNotifications();
            NotificationId = Notification.NotificationId;

            if (Group == null)
            {
                return NotFound();
            }

            LinkedNotifications = Group.Notifications.Where(n => n.NotificationId != NotificationId).ToList();

            return Page();
        }
    }
}