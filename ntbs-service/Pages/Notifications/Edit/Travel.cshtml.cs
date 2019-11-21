using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class TravelModel : NotificationEditModelBase
    {
        [BindProperty]
        public TravelDetails TravelDetails { get; set; }

        [BindProperty]
        public VisitorDetails VisitorDetails { get; set; }

        public SelectList HighTbIncidenceCountries { get; set; }

        public TravelModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository) : base(service, authorizationService, notificationRepository)
        {
            HighTbIncidenceCountries = new SelectList(
                referenceDataRepository.GetAllHighTbIncidenceCountriesAsync().Result,
                nameof(Country.CountryId),
                nameof(Country.Name));
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            TravelDetails = Notification.TravelDetails;
            VisitorDetails = Notification.VisitorDetails;
            await SetNotificationProperties(isBeingSubmitted, TravelDetails);
            await SetNotificationProperties(isBeingSubmitted, VisitorDetails);

            if (TravelDetails.ShouldValidateFull)
                TryValidateModel(TravelDetails, TravelDetails.GetType().Name);
            if (VisitorDetails.ShouldValidateFull)
                TryValidateModel(VisitorDetails, VisitorDetails.GetType().Name);

            return Page();
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./Comorbidities", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            TravelDetails.SetFullValidation(Notification.NotificationStatus);
            VisitorDetails.SetFullValidation(Notification.NotificationStatus);

            CleanModel();

            TryValidateModel(TravelDetails, TravelDetails.GetType().Name);
            TryValidateModel(VisitorDetails, VisitorDetails.GetType().Name);

            if (ModelState.IsValid)
            {
                await Service.UpdateTravelAndVisitorAsync(Notification, TravelDetails, VisitorDetails);
            }
        }

        private void CleanModel()
        {
            if (TravelDetails.HasTravel != true)
            {
                Service.ClearTravelOrVisitorFields(TravelDetails);
            }

            if (VisitorDetails.HasVisitor != true)
            {
                Service.ClearTravelOrVisitorFields(VisitorDetails);
            }
        }

        public IActionResult OnGetValidateTravel(TravelDetails travelDetails)
        {
            Service.ClearTravelOrVisitorFields(travelDetails);
            return ValidationService.ValidateFullModel(travelDetails);
        }

        public IActionResult OnGetValidateVisitor(VisitorDetails visitorDetails)
        {
            Service.ClearTravelOrVisitorFields(visitorDetails);
            return ValidationService.ValidateFullModel(visitorDetails);
        }
    }
}
