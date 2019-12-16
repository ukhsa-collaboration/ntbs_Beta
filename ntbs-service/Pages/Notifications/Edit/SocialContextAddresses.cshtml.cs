using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class SocialContextAddressesModel : NotificationEditModelBase
    {
        public ICollection<SocialContextAddress> SocialContextAddresses { get; set; }

        public SocialContextAddressesModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository)
        {
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            SocialContextAddresses = Notification.SocialContextAddresses;
            await SetNotificationProperties(isBeingSubmitted);

            return Page();
        }

        #pragma warning disable 1998
        protected override async Task ValidateAndSave()
        {
            // No validation or saving happening on list
        }
        #pragma warning restore 1998
        
        protected override IActionResult RedirectToCreate()
        {
            return RedirectToPage("./Items/NewSocialContextAddress", new { NotificationId });
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./SocialContextVenues", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithSocialContextAddressesAsync(notificationId);
        }
    }
}
