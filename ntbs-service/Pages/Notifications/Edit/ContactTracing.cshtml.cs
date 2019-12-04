using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;
using ntbs_service.Helpers;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ContactTracingModel : NotificationEditModelBase
    {
        public ContactTracingModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService,
            notificationRepository)
        {
        }

        [BindProperty]
        public ContactTracing ContactTracing { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            ContactTracing = Notification.ContactTracing;
            await SetNotificationProperties<ContactTracing>(isBeingSubmitted, ContactTracing);

            return Page();
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./SocialRiskFactors", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            ContactTracing.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(ContactTracing, ContactTracing.GetType().Name))
            {
                await Service.UpdateContactTracingAsync(Notification, ContactTracing);
            }
        }

        public ContentResult OnGetValidateContactTracing(ContactTracing model, string key)
        {
            return ValidationService.ValidateFullModel(model);
        }
    }
}
