using System.Collections.Generic;
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
        protected INotificationService Service;
        protected IAuthorizationService AuthorizationService;
        protected IAlertRepository AlertRepository;
        protected INotificationRepository NotificationRepository;

        protected NotificationModelBase(
            INotificationService service,
            IAuthorizationService authorizationService,
            IAlertRepository alertRepository,
            INotificationRepository notificationRepository = null)
        {
            this.Service = service;
            this.AuthorizationService = authorizationService;
            this.AlertRepository = alertRepository;
            this.NotificationRepository = notificationRepository;
        }

        protected NotificationGroup Group;
        public int NumberOfLinkedNotifications { get; set; }

        public Notification Notification { get; set; }
        public NotificationBannerModel NotificationBannerModel { get; set; }
        public IList<Alert> Alerts { get; set; }

        [BindProperty]
        public bool HasEditPermission { get; set; }

        [BindProperty]
        public int NotificationId { get; set; }

        protected async Task AuthorizeAndSetBannerAsync()
        {
            HasEditPermission = await AuthorizationService.CanEdit(User, Notification);
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
            return await NotificationRepository.GetNotificationGroupAsync(NotificationId);
        }

        protected async Task GetAlertsAsync()
        {
            Alerts = await AlertRepository.GetAlertsForNotificationAsync(NotificationId);
        }
    }
}
