using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit.Items
{
    public class SocialContextAddressModel : SocialContextBaseModel<SocialContextAddress>
    {
        [BindProperty] public SocialContextAddress Address { get; set; }

        public SocialContextAddressModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IUserHelper userHelper,
            INotificationRepository notificationRepository,
            IItemRepository<SocialContextAddress> socialContextAddressRepository,
            IAlertService alertService,
            IAlertRepository alertRepository)
            : base(service, authorizationService, userHelper, notificationRepository, socialContextAddressRepository, alertService, alertRepository)
        {
        }

#pragma warning disable 1998
        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
#pragma warning restore 1998
        {
            if (RowId != null)
            {
                Address = GetSocialContextBaseById(Notification, RowId.Value);
                if (Address == null)
                {
                    return NotFound();
                }
                FormatDatesForGet(Address);
            }

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            await ValidateAndSave(Address, "Address");
        }

        protected override IActionResult RedirectForNotified()
        {
            return RedirectToPage("/Notifications/Edit/SocialContextAddresses", new { NotificationId });
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("/Notifications/Edit/SocialContextAddresses", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithSocialContextAddressesAsync(notificationId);
        }

        protected override SocialContextAddress GetSocialContextBaseById(Notification notification, int id)
        {
            return notification.SocialContextAddresses
                    .SingleOrDefault(s => s.SocialContextAddressId == id);
        }
    }
}
