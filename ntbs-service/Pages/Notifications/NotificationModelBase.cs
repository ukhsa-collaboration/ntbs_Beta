using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ntbs_service.Pages_Notifications
{
    // Needed by all Notification pages
    public abstract class NotificationModelBase : PageModel
    {
        protected INotificationService service;

        public NotificationModelBase(INotificationService service) 
        {
            this.service = service;
        }

        protected NotificationGroup Group;

        public int NumberOfLinkedNotifications { get; set; }

        public Notification Notification { get; set; }

        public NotificationBannerModel NotificationBannerModel { get; set; }

        [BindProperty]
        public int NotificationId { get; set; }

        protected async Task GetLinkedNotifications()
        {
            Group = await GetNotificationGroupAsync();
            NumberOfLinkedNotifications = Group != null ? (Group.Notifications.Count -1) : 0;
        }

        private async Task<NotificationGroup> GetNotificationGroupAsync()
        {
            var groupId = Notification.GroupId;
            if (groupId == null)
            {
                return null;
            }

            return await service.GetNotificationGroupAsync(groupId.Value);
        }
    }
}
