using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class SocialRiskFactorsModel : NotificationEditModelBase
    {
        [BindProperty]
        public SocialRiskFactors SocialRiskFactors { get; set; }

        public SocialRiskFactorsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository)
        {
        }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            SocialRiskFactors = Notification.SocialRiskFactors;
            await SetNotificationProperties(isBeingSubmitted, SocialRiskFactors);

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            SocialRiskFactors.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(SocialRiskFactors))
            {
                await Service.UpdateSocialRiskFactorsAsync(Notification, SocialRiskFactors);
            }
        }

        protected override IActionResult RedirectAfterSaveForDraft(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Travel", new { NotificationId, isBeingSubmitted });
        }
    }
}
