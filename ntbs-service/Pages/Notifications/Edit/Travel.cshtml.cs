using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;
using System.Threading.Tasks;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class TravelModel : NotificationEditModelBase
    {

        [BindProperty]
        public TravelDetails TravelDetails { get; set; }

        [BindProperty]
        public VisitorDetails VisitorDetails { get; set; }

        public SelectList HighTbIncidenceCountries { get; set; }

        public TravelModel(INotificationService service, NtbsContext context) : base(service)
        {
            HighTbIncidenceCountries = new SelectList(
                context.GetAllHighTbIncidenceCountriesAsync().Result,
                nameof(Country.CountryId),
                nameof(Country.Name));
        }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted = false)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }
            TravelDetails = Notification.TravelDetails;
            VisitorDetails = Notification.VisitorDetails;
            NotificationBannerModel = new NotificationBannerModel(Notification);

            await SetNotificationProperties(isBeingSubmitted, TravelDetails);
            await SetNotificationProperties(isBeingSubmitted, VisitorDetails);

            NotificationBannerModel = new NotificationBannerModel(Notification);

            if (TravelDetails.ShouldValidateFull)
                TryValidateModel(TravelDetails, TravelDetails.GetType().Name);
            if (VisitorDetails.ShouldValidateFull)
                TryValidateModel(VisitorDetails, VisitorDetails.GetType().Name);

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Comorbidities", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task<bool> ValidateAndSave()
        {
            TravelDetails.SetFullValidation(Notification.NotificationStatus);
            VisitorDetails.SetFullValidation(Notification.NotificationStatus);

            CleanModel();

            if (!TryValidateModel(this))
            {
                return false;
            }

            await service.UpdateTravelAndVisitorAsync(Notification, TravelDetails, VisitorDetails);
            return true;
        }

        private void CleanModel()
        {
            if (TravelDetails.HasTravel != true)
            {
                service.ClearTravelOrVisitorFields(TravelDetails);
            }

            if (VisitorDetails.HasVisitor != true)
            {
                service.ClearTravelOrVisitorFields(VisitorDetails);
            }
        }

        public IActionResult OnGetValidateTravel(TravelDetails travelDetails)
        {
            service.ClearTravelOrVisitorFields(travelDetails);
            return validationService.ValidateFullModel(travelDetails);
        }

        public IActionResult OnGetValidateVisitor(VisitorDetails visitorDetails)
        {
            service.ClearTravelOrVisitorFields(visitorDetails);
            return validationService.ValidateFullModel(visitorDetails);
        }
    }
}