using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages.Exceptions;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class SocialRiskFactorsModel : NotificationEditModelBase
    {
        [BindProperty]
        public SocialRiskFactors SocialRiskFactors { get; set; }

        public SocialRiskFactorsModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService) {}

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

            SocialRiskFactors = Notification.SocialRiskFactors;
            await SetNotificationProperties(isBeingSubmitted, SocialRiskFactors);

            return Page();
        }

        protected override async Task<bool> ValidateAndSave() 
        {
            SocialRiskFactors.SetFullValidation(Notification.NotificationStatus);   
            if (!TryValidateModel(SocialRiskFactors))
            {
                return false;
            }

            await service.UpdateSocialRiskFactorsAsync(Notification, SocialRiskFactors);
            return true;
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Travel", new { id = notificationId, isBeingSubmitted });
        }
    }
}