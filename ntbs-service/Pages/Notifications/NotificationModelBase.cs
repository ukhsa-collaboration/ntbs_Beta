using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using ntbs_service.Pages.Exceptions;

namespace ntbs_service.Pages_Notifications
{
    // Needed by all Notification pages
    public abstract class NotificationModelBase : PageModel
    {
        protected INotificationService service;
        protected IAuthorizationService authorizationService;

        public NotificationModelBase(INotificationService service, IAuthorizationService authorizationService)
        {
            this.service = service;
            this.authorizationService = authorizationService;
        }

        protected NotificationGroup Group;
        public int NumberOfLinkedNotifications { get; set; }

        public Notification Notification { get; set; }
        public NotificationBannerModel NotificationBannerModel { get; set; }

        [BindProperty]
        public bool HasEditPermission { get; set; }

        [BindProperty]
        public int NotificationId { get; set; }

        protected async Task AuthorizeAndSetBannerAsync()
        {
            HasEditPermission = await authorizationService.CanEdit(User, Notification);
            NotificationBannerModel = new NotificationBannerModel(Notification, HasEditPermission);
        }

        protected async Task TryGetLinkedNotifications()
        {
            await GetLinkedNotifications();
            if (Group == null)
            {
                throw new NoLinkedNotificationsException();
            }
        }

        protected async Task GetLinkedNotifications()
        {
            Group = await GetNotificationGroupAsync();
            NumberOfLinkedNotifications = Group?.Notifications.Count - 1 ?? 0;
        }

        protected IActionResult ForbiddenResult()
        {
            return StatusCode((int)HttpStatusCode.Forbidden);
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
