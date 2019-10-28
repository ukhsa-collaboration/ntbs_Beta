using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages.Exceptions;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ComorbiditiesModel : NotificationEditModelBase
    {
        [BindProperty]
        public ComorbidityDetails ComorbidityDetails { get; set; }
        public ComorbiditiesModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService) {}

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            try
            {
                await SetNotificationAndAuthorize(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (NotAuthorizedException)
            {
                return RedirectToOverview(id);
            }

            ComorbidityDetails = Notification.ComorbidityDetails;

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

        protected override async Task<bool> ValidateAndSave()
        {
            ComorbidityDetails.SetFullValidation(Notification.NotificationStatus);
            if (!TryValidateModel(this))
            {
                return false;
            }

            await service.UpdateComorbidityAsync(Notification, ComorbidityDetails);
            return true;
        }
    }
}