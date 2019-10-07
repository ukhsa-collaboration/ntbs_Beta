using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    // TODO - To complete when spec is ready for this page
    public class ComorbiditiesModel : NotificationEditModelBase
    {
        public ComorbiditiesModel(INotificationService service) : base(service) { }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            Notification.SetFullValidation(Notification.NotificationStatus, isBeingSubmitted);
            NotificationBannerModel = new NotificationBannerModel(Notification);

            NotificationId = id;

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Immunosupression", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task<bool> ValidateAndSave() => await Task.FromResult(true);
    }
}