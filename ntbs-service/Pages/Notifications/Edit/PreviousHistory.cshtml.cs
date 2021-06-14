using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class PreviousHistoryModel : NotificationEditModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public PreviousHistoryModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository,
            IReferenceDataRepository referenceDataRepository,
            IUserHelper userHelper) : base(service, authorizationService, notificationRepository, alertRepository, userHelper)
        {
            _referenceDataRepository = referenceDataRepository;
            CurrentPage = NotificationSubPaths.EditPreviousHistory;
        }

        [BindProperty]
        public PreviousTbHistory PreviousTbHistory { get; set; }

        public SelectList Countries { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            var countries = await _referenceDataRepository.GetAllCountriesAsync();
            Countries = new SelectList(countries, nameof(Country.CountryId), nameof(Country.Name));

            PreviousTbHistory = Notification.PreviousTbHistory;
            await SetNotificationProperties(isBeingSubmitted, PreviousTbHistory);

            if (PreviousTbHistory.ShouldValidateFull)
            {
                TryValidateModel(PreviousTbHistory, PreviousTbHistory.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./TreatmentEvents", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            UpdateFlags();
            PreviousTbHistory.SetValidationContext(Notification);
            PreviousTbHistory.DobYear = Notification.PatientDetails.Dob?.Year;

            TryValidateModel(PreviousTbHistory, nameof(PreviousTbHistory));

            if (ModelState.IsValid)
            {
                await Service.UpdatePreviousTbHistoryAsync(Notification, PreviousTbHistory);
            }
        }

        private void UpdateFlags()
        {
            if (PreviousTbHistory.PreviouslyHadTb != Status.Yes)
            {
                PreviousTbHistory.PreviousTbDiagnosisYear = null;
                ModelState.Remove($"{nameof(PreviousTbHistory)}.{nameof(PreviousTbHistory.PreviousTbDiagnosisYear)}");
                PreviousTbHistory.PreviouslyTreated = null;
                ModelState.Remove($"{nameof(PreviousTbHistory)}.{nameof(PreviousTbHistory.PreviouslyTreated)}");
            }

            if (PreviousTbHistory.PreviouslyTreated != Status.Yes)
            {
                PreviousTbHistory.PreviousTreatmentCountryId = null;
                ModelState.Remove(
                    $"{nameof(PreviousTbHistory)}.{nameof(PreviousTbHistory.PreviousTreatmentCountryId)}");
            }
        }

        public ContentResult OnPostValidateProperty([FromBody]InputValidationModel validationData)
        {
            return ValidationService.GetPropertyValidationResult<PreviousTbHistory>(validationData.Key, validationData.Value, validationData.ShouldValidateFull);
        }
    }
}
