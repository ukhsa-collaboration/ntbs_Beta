using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ComorbiditiesModel : NotificationEditModelBase
    {
        [BindProperty]
        public ComorbidityDetails ComorbidityDetails { get; set; }
        public ComorbiditiesModel(INotificationService service) : base(service) { }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            ComorbidityDetails = Notification.ComorbidityDetails;
            NotificationBannerModel = new NotificationBannerModel(Notification);

            await SetNotificationProperties(isBeingSubmitted, ComorbidityDetails);
            if (ComorbidityDetails.ShouldValidateFull)
            {
                TryValidateModel(ComorbidityDetails, ComorbidityDetails.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Immunosuppression", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            ComorbidityDetails.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(ComorbidityDetails, ComorbidityDetails.GetType().Name))
            {
                await service.UpdateComorbidityAsync(Notification, ComorbidityDetails);
            }
        }
    }
}