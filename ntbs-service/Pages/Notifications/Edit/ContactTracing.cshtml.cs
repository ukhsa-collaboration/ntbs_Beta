using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ContactTracingModel : NotificationEditModelBase
    {
        public ContactTracingModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository) : base(service, authorizationService, notificationRepository, alertRepository)
        {
            CurrentPage = NotificationSubPaths.EditContactTracing;
        }

        [BindProperty]
        public ContactTracing ContactTracing { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            ContactTracing = Notification.ContactTracing;
            await SetNotificationProperties<ContactTracing>(isBeingSubmitted, ContactTracing);

            return Page();
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./SocialRiskFactors", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            ContactTracing.SetValidationContext(Notification);
            if (TryValidateModel(ContactTracing, ContactTracing.GetType().Name))
            {
                await Service.UpdateContactTracingAsync(Notification, ContactTracing);
            }
        }

        public ContentResult OnGetValidateContactTracing(ContactTracing model, string key)
        {
            return ValidationService.GetFullModelValidationResult(model);
        }
    }
}
