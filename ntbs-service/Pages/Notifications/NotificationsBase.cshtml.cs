using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class NotificationModelBase : ValidationModel
    {
        protected INotificationService service;

        public NotificationModelBase(INotificationService service) {
            this.service = service;
        }

        [BindProperty]
        public int? NotificationId { get; set; }

        public async Task<IActionResult> OnPostSubmitAsync(int? NotificationId)
        {
            var notification = await service.GetNotificationAsync(NotificationId);
            if (notification == null)
            {
                return NotFound();
            }

            await service.SubmitNotification(notification);

            return RedirectToPage("../Index");
        } 
    }
}
