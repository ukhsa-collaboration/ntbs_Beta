using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class SocialContextVenueModel : NotificationEditModelBase
    {
        private readonly IPostcodeService _postcodeService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IItemRepository<SocialContextVenue> _socialContextVenueRepository;
        public SelectList VenueTypes { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? RowId { get; set; }

        [BindProperty]
        public SocialContextVenue Venue { get; set; }

        [BindProperty]
        public FormattedDate FormattedDateFrom { get; set; }
        [BindProperty]
        public FormattedDate FormattedDateTo { get; set; }

        public SocialContextVenueModel(
            INotificationService service,
            IPostcodeService postcodeService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IItemRepository<SocialContextVenue> socialContextVenueRepository) : base(service, authorizationService, notificationRepository)
        {
            _postcodeService = postcodeService;
            _referenceDataRepository = referenceDataRepository;
            _socialContextVenueRepository = socialContextVenueRepository;
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            if (RowId != null)
            {
                Venue = Notification.SocialContextVenues
                    .SingleOrDefault(s => s.SocialContextVenueId == RowId.Value);
                if (Venue == null)
                {
                    return NotFound();
                }
                FormattedDateFrom = Venue.DateFrom.ConvertToFormattedDate();
                FormattedDateTo = Venue.DateTo.ConvertToFormattedDate();
            }

            await SetDropdownsAsync();

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            Venue.NotificationId = NotificationId;
            Venue.Dob = Notification.PatientDetails.Dob;
            await FindAndSetPostcodeAsync();
            SetDates();
            Venue.SetFullValidation(Notification.NotificationStatus);

            if (TryValidateModel(Venue, "Venue"))
            {
                if (RowId == null)
                {
                    await _socialContextVenueRepository.AddAsync(Venue);
                }
                else
                {
                    Venue.SocialContextVenueId = RowId.Value;
                    await _socialContextVenueRepository.UpdateAsync(Notification, Venue);
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Notification = await GetNotificationAsync(NotificationId);
            if (!(await AuthorizationService.CanEdit(User, Notification)))
            {
                return ForbiddenResult();
            }

            var venue = Notification.SocialContextVenues
                    .SingleOrDefault(s => s.SocialContextVenueId == RowId.Value);
            if (venue == null)
            {
                return NotFound();
            }

            await _socialContextVenueRepository.DeleteAsync(venue);

            return RedirectToPage("/Notifications/Edit/SocialContextVenues", new { NotificationId });
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

        protected override IActionResult RedirectAfterSaveForNotified()
        {
            return RedirectToPage("/Notifications/Edit/SocialContextVenues", new { NotificationId });
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("/Notifications/Edit/SocialContextVenues", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithSocialContextVenuesAsync(notificationId);
        }


        private void SetDates()
        {
            // The required date will be marked as missing on the model, since we are setting it manually, rather than binding it
            ModelState.Remove("Venue.DateFrom");
            ValidationService.TrySetFormattedDate(Venue, "Venue", nameof(Venue.DateFrom), FormattedDateFrom);
            ModelState.Remove("Venue.DateTo");
            ValidationService.TrySetFormattedDate(Venue, "Venue", nameof(Venue.DateTo), FormattedDateTo);
        }

        private async Task FindAndSetPostcodeAsync()
        {
            ModelState.ClearValidationState("Venue.Postcode");
            await FindAndSetPostcodeAsync(_postcodeService, Venue);
        }

        public async Task<ContentResult> OnGetValidatePostcode(string postcode, bool shouldValidateFull)
        {
            return await OnGetValidatePostcode<SocialContextVenue>(_postcodeService, postcode, shouldValidateFull);
        }

        public ContentResult OnGetValidateVenueProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.ValidateModelProperty<SocialContextVenue>(key, value, shouldValidateFull);
        }

        public ContentResult OnGetValidateVenueDate(string key, string day, string month, string year)
        {
            return ValidationService.ValidateDate<SocialContextVenue>(key, day, month, year);
        }

        public ContentResult OnGetValidateVenueDates(IEnumerable<Dictionary<string, string>> keyValuePairs)
        {
            var propertyValueTuples = new List<Tuple<string, object>>();
            foreach (var keyValuePair in keyValuePairs)
            {
                var formattedDate = new FormattedDate() { Day = keyValuePair["day"], Month = keyValuePair["month"], Year = keyValuePair["year"] };
                if (formattedDate.TryConvertToDateTime(out DateTime? convertedDob))
                {
                    propertyValueTuples.Add(new Tuple<string, object>(keyValuePair["key"], convertedDob));
                }
                else
                {
                    // should not ever get here as we validate individual dates first before comparing
                    return null;
                }
            }
            return ValidationService.ValidateMultipleProperties<SocialContextVenue>(propertyValueTuples);
        }
    }
}
