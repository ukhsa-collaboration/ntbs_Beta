using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Enums;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public abstract class NotificationModelBase : ValidationModel
    {
        protected INotificationService service;

        public NotificationModelBase(INotificationService service) {
            this.service = service;
        }

        [BindProperty]
        public int? NotificationId { get; set; }
        
        // This can be thrown away once proper banner work completes
        public NotificationStatus NotificationStatus { get; set; }

        public async Task<IActionResult> OnPostSubmitAsync(int? notificationId)
        {
            bool isValid = await ValidateAndSave(notificationId);
            if (!isValid) {
                return await OnGetAsync(notificationId);
            }
            
            var notification = await service.GetNotificationAsync(notificationId);
            if (notification == null)
            {
                return NotFound();
            }

            await service.SubmitNotification(notification);
            
            return RedirectToPage("../Overview", new {id = notificationId});
        }

        
        public async Task<IActionResult> OnPostSaveAsync(int? notificationId)
        {
            bool isValid = await ValidateAndSave(notificationId);

            if (!isValid) {
                return await OnGetAsync(notificationId);
            }

            return RedirectToNextPage(notificationId);
        }

        protected abstract Task<bool> ValidateAndSave(int? notificationId);

        public abstract Task<IActionResult> OnGetAsync(int? notificationId);

        protected abstract IActionResult RedirectToNextPage(int? notificationId);
    }
}
