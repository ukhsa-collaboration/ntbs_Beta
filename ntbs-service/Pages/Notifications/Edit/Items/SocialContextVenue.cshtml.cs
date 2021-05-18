using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit.Items
{
    public class SocialContextVenueModel : SocialContextBaseModel<SocialContextVenue>
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        public SelectList VenueTypes { get; set; }

        [BindProperty]
        public SocialContextVenue Venue { get; set; }

        public SocialContextVenueModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IUserHelper userHelper,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IItemRepository<SocialContextVenue> socialContextVenueRepository,
            IAlertService alertService,
            IAlertRepository alertRepository)
            : base(service, authorizationService, userHelper, notificationRepository, socialContextVenueRepository, alertService, alertRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            if (RowId != null)
            {
                Venue = GetSocialContextBaseById(Notification, RowId.Value);
                if (Venue == null)
                {
                    return NotFound();
                }
                FormatDatesForGet(Venue);
            }

            await SetDropdownsAsync();

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            await ValidateAndSave(Venue, "Venue");
        }

        private async Task SetDropdownsAsync()
        {
            var venueTypes = await _referenceDataRepository.GetAllVenueTypesAsync();
            VenueTypes = new SelectList(
                items: venueTypes,
                dataValueField: nameof(VenueType.VenueTypeId),
                dataTextField: nameof(VenueType.Name),
                selectedValue: null,
                dataGroupField: nameof(VenueType.Category));
        }

        protected override IActionResult RedirectForNotified()
        {
            return RedirectToPage("/Notifications/Edit/SocialContextVenues", new { NotificationId });
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("/Notifications/Edit/SocialContextVenues", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithSocialContextVenuesAsync(notificationId);
        }

        protected override SocialContextVenue GetSocialContextBaseById(Notification notification, int id)
        {
            return notification.SocialContextVenues
                    .SingleOrDefault(s => s.SocialContextVenueId == id);
        }
    }
}
