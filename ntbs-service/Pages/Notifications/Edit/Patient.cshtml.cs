using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class PatientModel : NotificationEditModelBase
    {
        private readonly IPostcodeService _postcodeService;

        public SelectList Ethnicities { get; set; }
        public SelectList Countries { get; set; }
        public List<string> RenderConditionalCountryFieldIds { get; set; }
        public List<Sex> Sexes { get; set; }
        public SelectList Occupations { get; set; }
        public List<string> RenderConditionalOccupationFieldIds { get; set; }
        public Dictionary<string, string> DuplicateNhsNumberNotifications { get; set; }

        [BindProperty]
        public PatientDetails Patient { get; set; }

        [BindProperty]
        [ValidFormattedDateCanConvertToDatetime(ErrorMessage = ValidationMessages.InvalidDate)]
        public FormattedDate FormattedDob { get; set; }

        public PatientModel(
            INotificationService service,
            IPostcodeService postcodeService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository) : base(service, authorizationService, notificationRepository)
        {
            _postcodeService = postcodeService;
            GenerateReferenceData(referenceDataRepository);
        }

        private void GenerateReferenceData(IReferenceDataRepository referenceDataRepository)
        {
            Ethnicities = new SelectList(
                referenceDataRepository.GetAllOrderedEthnicitiesAsync().Result,
                nameof(Ethnicity.EthnicityId),
                nameof(Ethnicity.Label));

            var countries = referenceDataRepository.GetAllCountriesAsync().Result;
            Countries = new SelectList(countries, nameof(Country.CountryId), nameof(Country.Name));
            RenderConditionalCountryFieldIds = countries
                .Where(c =>
                    c.IsoCode == Models.Countries.UkCode
                    || c.IsoCode == Models.Countries.UnknownCode)
                .Select(c => c.CountryId.ToString())
                .ToList();

            Sexes = referenceDataRepository.GetAllSexesAsync().Result.ToList();

            var occupations = referenceDataRepository.GetAllOccupationsAsync().Result;
            Occupations = new SelectList(
                items: occupations,
                dataValueField: nameof(Occupation.OccupationId),
                dataTextField: nameof(Occupation.Role),
                selectedValue: null,
                dataGroupField: nameof(Occupation.Sector));
            RenderConditionalOccupationFieldIds = occupations
                .Where(o => o.HasFreeTextField)
                .Select(o => o.OccupationId.ToString())
                .ToList();
        }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            Patient = Notification.PatientDetails;
            await SetNotificationProperties(isBeingSubmitted, Patient);

            FormattedDob = Patient.Dob.ConvertToFormattedDate();

            if (Patient.ShouldValidateFull)
            {
                TryValidateModel(Patient, "Patient");
            }

            DuplicateNhsNumberNotifications = await GenerateDuplicateNhsNumberNotificationUrlsAsync(Patient.NhsNumber);

            return Page();
        }

        private async Task<Dictionary<string, string>> GenerateDuplicateNhsNumberNotificationUrlsAsync(string nhsNumber)
        {
            // If NhsNumber is empty or does not pass validation - return null
            if (string.IsNullOrEmpty(nhsNumber) || !string.IsNullOrEmpty(
                ValidationService.ValidateModelProperty<PatientDetails>("NhsNumber", nhsNumber, false).Content))
            {
                return null;
            }

            var notificationIds =
                await NotificationRepository.GetNotificationIdsByNhsNumber(nhsNumber);
            // Filter this notification and linked notifications from collection.
            notificationIds = notificationIds
                .Where(n => n != NotificationId
                         && (!Group?.Notifications.Exists(linked => linked.NotificationId == n) ?? true))
                .ToList();
            return notificationIds.ToDictionary(
                id => id.ToString(),
                id => RouteHelper.GetNotificationPath(NotificationSubPaths.Overview, id));
        }

        protected override async Task ValidateAndSave()
        {
            await Service.UpdatePatientFlagsAsync(Patient);
            // Remove already invalidated states from modelState as rely
            // on changes made in UpdatePatientFlags
            ModelState.ClearValidationState("Patient.Postcode");
            ModelState.ClearValidationState("Patient.NHSNumber");
            ModelState.ClearValidationState("Patient.OccupationOther");
            if (Patient.UkBorn != false)
            {
                ModelState.ClearValidationState("Patient.YearOfUkEntry");
            }

            Patient.SetFullValidation(Notification.NotificationStatus);
            await FindAndSetPostcodeAsync();

            ValidationService.TrySetAndValidateDateOnModel(Patient, nameof(Patient.Dob), FormattedDob);

            if (TryValidateModel(Patient, "Patient"))
            {
                await Service.UpdatePatientAsync(Notification, Patient);
            }
        }

        private async Task FindAndSetPostcodeAsync()
        {
            var foundPostcode = await _postcodeService.FindPostcode(Patient.Postcode);
            Patient.PostcodeToLookup = foundPostcode?.Postcode;
        }

        public async Task<ContentResult> OnGetValidatePostcode(string postcode, bool shouldValidateFull)
        {
            var foundPostcode = await _postcodeService.FindPostcode(postcode);
            var propertyValueTuples = new List<Tuple<string, object>>
            {
                new Tuple<string, object>("PostcodeToLookup", foundPostcode?.Postcode),
                new Tuple<string, object>("Postcode", postcode)
            };

            return ValidationService.ValidateMultipleProperties<PatientDetails>(propertyValueTuples, shouldValidateFull);
        }

        protected override IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Episode", new { id = notificationId, isBeingSubmitted });
        }

        public ContentResult OnGetValidatePatientProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.ValidateModelProperty<PatientDetails>(key, value, shouldValidateFull);
        }

        public ContentResult OnGetValidatePatientDate(string key, string day, string month, string year)
        {
            return ValidationService.ValidateDate<PatientDetails>(key, day, month, year);
        }

        public async Task<JsonResult> OnGetNhsNumberDuplicates(int notificationId, string nhsNumber)
        {
            NotificationId = notificationId;
            await GetLinkedNotifications();
            return new JsonResult(await GenerateDuplicateNhsNumberNotificationUrlsAsync(nhsNumber));
        }
    }
}
