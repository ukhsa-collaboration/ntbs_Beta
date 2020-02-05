using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public abstract class NotificationModelBase : PageModel, INotificationLayoutWithBanner
    {
        protected INotificationService Service;
        protected IAuthorizationService AuthorizationService;
        protected INotificationRepository NotificationRepository;

        protected NotificationModelBase(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository = null)
        {
            Service = service;
            AuthorizationService = authorizationService;
            NotificationRepository = notificationRepository;
        }
        public int NumberOfLinkedNotifications { get; set; }

        public Notification Notification { get; set; }
        public NotificationBannerModel NotificationBannerModel { get; set; }
        public IList<Alert> Alerts { get; set; }

        public PermissionLevel PermissionLevel { get; set; }

        [BindProperty(SupportsGet = true)]
        public int NotificationId { get; set; }

        protected virtual async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationAsync(notificationId);
        }

        protected async Task AuthorizeAndSetBannerAsync()
        {
            PermissionLevel = await AuthorizationService.GetPermissionLevelForNotificationAsync(User, Notification);
            NotificationBannerModel = new NotificationBannerModel(Notification, PermissionLevel != PermissionLevel.Edit);
        }

        protected async Task<bool> TryGetLinkedNotifications()
        {
            await GetLinkedNotifications();
            return Notification.Group != null;
        }

        protected async Task GetLinkedNotifications()
        {
            if (Notification.Group == null)
            {
                Notification.Group = await GetNotificationGroupAsync();
                NumberOfLinkedNotifications = Notification.Group?.Notifications.Count - 1 ?? 0;
            }
        }

        protected IActionResult ForbiddenResult()
        {
            return StatusCode((int)HttpStatusCode.Forbidden);
        }

        private async Task<NotificationGroup> GetNotificationGroupAsync()
        {
            return await NotificationRepository.GetNotificationGroupAsync(NotificationId);
        }
    }
}
