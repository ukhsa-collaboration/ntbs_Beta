using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ComorbiditiesModel : NotificationEditModelBase
    {
        [BindProperty]
        public ComorbidityDetails ComorbidityDetails { get; set; }
        public ComorbiditiesModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository) { }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            ComorbidityDetails = Notification.ComorbidityDetails;

            await SetNotificationProperties(isBeingSubmitted, ComorbidityDetails);
            if (ComorbidityDetails.ShouldValidateFull)
            {
                TryValidateModel(ComorbidityDetails, ComorbidityDetails.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectAfterSaveForDraft(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Immunosuppression", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            ComorbidityDetails.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(ComorbidityDetails, ComorbidityDetails.GetType().Name))
            {
                await Service.UpdateComorbidityAsync(Notification, ComorbidityDetails);
            }
        }
    }
}
