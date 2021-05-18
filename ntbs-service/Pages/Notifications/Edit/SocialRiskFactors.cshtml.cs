using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
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
            IUserHelper userHelper,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository) : base(service, authorizationService, userHelper, notificationRepository, alertRepository)
        {
            CurrentPage = NotificationSubPaths.EditSocialRiskFactors;
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            SocialRiskFactors = Notification.SocialRiskFactors;
            await SetNotificationProperties(isBeingSubmitted, SocialRiskFactors);

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            SocialRiskFactors.SetValidationContext(Notification);
            if (TryValidateModel(SocialRiskFactors))
            {
                await Service.UpdateSocialRiskFactorsAsync(Notification, SocialRiskFactors);
            }
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./Travel", new { NotificationId, isBeingSubmitted });
        }
    }
}
