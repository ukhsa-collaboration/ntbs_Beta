using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public abstract class NotificationModelBase : PageModel
    {
        protected INotificationService service;
        protected IAuthorizationService authorizationService;
        protected INotificationRepository notificationRepository;

        protected NotificationModelBase(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository = null)
        {
            this.service = service;
            this.notificationRepository = notificationRepository;
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

        protected async Task<bool> TryGetLinkedNotifications()
        {
            await GetLinkedNotifications();
            return Group != null;
        }

        protected async Task GetLinkedNotifications()
        {
            if (Group == null)
            {
                Group = await GetNotificationGroupAsync();
                NumberOfLinkedNotifications = Group?.Notifications.Count - 1 ?? 0;
            }
        }

        protected IActionResult ForbiddenResult()
        {
            return StatusCode((int)HttpStatusCode.Forbidden);
        }

        private async Task<NotificationGroup> GetNotificationGroupAsync()
        {
            return await notificationRepository.GetNotificationGroupAsync(NotificationId);
        }
    }
}
