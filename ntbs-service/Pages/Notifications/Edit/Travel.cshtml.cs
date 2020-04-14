using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
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
            IAlertRepository alertRepository,
            IReferenceDataRepository referenceDataRepository) : base(service, authorizationService, notificationRepository, alertRepository)
        {
            HighTbIncidenceCountries = new SelectList(
                referenceDataRepository.GetAllHighTbIncidenceCountriesAsync().Result,
                nameof(Country.CountryId),
                nameof(Country.Name));
            
            CurrentPage = NotificationSubPaths.EditTravel;
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

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./Comorbidities", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            TravelDetails.SetValidationContext(Notification);
            VisitorDetails.SetValidationContext(Notification);

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
                ModelState.ClearValidationState($"{nameof(TravelDetails)}.{nameof(TravelDetails.TotalDurationOfTravel)}");
                ModelState.ClearValidationState($"{nameof(TravelDetails)}.{nameof(TravelDetails.Country1Id)}");
                ModelState.ClearValidationState($"{nameof(TravelDetails)}.{nameof(TravelDetails.Country2Id)}");
                ModelState.ClearValidationState($"{nameof(TravelDetails)}.{nameof(TravelDetails.Country3Id)}");
                ModelState.ClearValidationState($"{nameof(TravelDetails)}.{nameof(TravelDetails.StayLengthInMonths1)}");
                ModelState.ClearValidationState($"{nameof(TravelDetails)}.{nameof(TravelDetails.StayLengthInMonths2)}");
                ModelState.ClearValidationState($"{nameof(TravelDetails)}.{nameof(TravelDetails.StayLengthInMonths3)}");
                Service.ClearTravelOrVisitorFields(TravelDetails);
            }

            if (VisitorDetails.HasVisitor != true)
            {
                ModelState.ClearValidationState($"{nameof(VisitorDetails)}.{nameof(VisitorDetails.TotalDurationOfVisit)}");
                ModelState.ClearValidationState($"{nameof(VisitorDetails)}.{nameof(VisitorDetails.Country1Id)}");
                ModelState.ClearValidationState($"{nameof(VisitorDetails)}.{nameof(VisitorDetails.Country2Id)}");
                ModelState.ClearValidationState($"{nameof(VisitorDetails)}.{nameof(VisitorDetails.Country3Id)}");
                ModelState.ClearValidationState($"{nameof(VisitorDetails)}.{nameof(VisitorDetails.StayLengthInMonths1)}");
                ModelState.ClearValidationState($"{nameof(VisitorDetails)}.{nameof(VisitorDetails.StayLengthInMonths2)}");
                ModelState.ClearValidationState($"{nameof(VisitorDetails)}.{nameof(VisitorDetails.StayLengthInMonths3)}");
                Service.ClearTravelOrVisitorFields(VisitorDetails);
            }
        }

        public IActionResult OnGetValidateTravel(TravelDetails travelDetails)
        {
            Service.ClearTravelOrVisitorFields(travelDetails);
            return ValidationService.GetFullModelValidationResult(travelDetails);
        }

        public IActionResult OnGetValidateVisitor(VisitorDetails visitorDetails)
        {
            Service.ClearTravelOrVisitorFields(visitorDetails);
            return ValidationService.GetFullModelValidationResult(visitorDetails);
        }
    }
}
