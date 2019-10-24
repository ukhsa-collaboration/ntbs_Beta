using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class PatientModel : NotificationEditModelBase
    {

        public SelectList Ethnicities { get; set; }
        public SelectList Countries { get; set; }
        public List<Sex> Sexes { get; set; }

        [BindProperty]
        public PatientDetails Patient { get; set; }

        [BindProperty]
        [ValidFormattedDateCanConvertToDatetime(ErrorMessage = ValidationMessages.InvalidDate)]
        public FormattedDate FormattedDob { get; set; }
        private IPostcodeService PostcodeService { get; set; }

        public PatientModel(INotificationService service, IPostcodeService postcodeService, NtbsContext context) : base(service)
        {
            PostcodeService = postcodeService;
            Ethnicities = new SelectList(context.GetAllOrderedEthnicitiesAsync().Result, nameof(Ethnicity.EthnicityId), nameof(Ethnicity.Label));
            Countries = new SelectList(context.GetAllCountriesAsync().Result, nameof(Country.CountryId), nameof(Country.Name));
            Sexes = context.GetAllSexesAsync().Result.ToList();
        }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            Patient = Notification.PatientDetails;
            await SetNotificationProperties<PatientDetails>(isBeingSubmitted, Patient);

            FormattedDob = Patient.Dob.ConvertToFormattedDate();

            if (Patient.ShouldValidateFull)
            {
                TryValidateModel(Patient, "Patient");
            }

            return Page();
        }

        protected override async Task<bool> ValidateAndSave()
        {
            UpdatePatientFlags();
            ModelState.ClearValidationState("Patient.Postcode");

            Patient.SetFullValidation(Notification.NotificationStatus);
            await FindAndSetPostcodeAsync();
            validationService.TrySetAndValidateDateOnModel(Patient, nameof(Patient.Dob), FormattedDob);
            
            if (!TryValidateModel(Patient, "Patient"))
            {
                return false;
            }

            await service.UpdatePatientAsync(Notification, Patient);
            return true;
        }

        private async Task FindAndSetPostcodeAsync()
        {
            var foundPostcode = await PostcodeService.FindPostcode(Patient.Postcode);
            Patient.PostcodeToLookup = foundPostcode?.Postcode;
        }
        
        private void UpdatePatientFlags()
        {
            if (Patient.NhsNumberNotKnown)
            {
                Patient.NhsNumber = null;
                ModelState.Remove("Patient.NhsNumber");
            }

            if (Patient.NoFixedAbode)
            {
                Patient.Postcode = null;
                ModelState.Remove("Patient.Postcode");
            }
        }


        public async Task<ContentResult> OnGetValidatePostcode(string postcode, bool shouldValidateFull)
        {   
            var foundPostcode = await PostcodeService.FindPostcode(postcode);
            var propertyValueTuples = new List<Tuple<string, object>>
            {
                new Tuple<string, object>("PostcodeToLookup", foundPostcode?.Postcode),
                new Tuple<string, object>("Postcode", postcode)
            };

            return validationService.ValidateMultipleProperties<PatientDetails>(propertyValueTuples, shouldValidateFull);
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Episode", new { id = notificationId, isBeingSubmitted });
        }

        public ContentResult OnGetValidatePatientProperty(string key, string value, bool shouldValidateFull)
        {
            return validationService.ValidateModelProperty<PatientDetails>(key, value, shouldValidateFull);
        }

        public ContentResult OnGetValidatePatientDate(string key, string day, string month, string year)
        {
            return validationService.ValidateDate<PatientDetails>(key, day, month, year);
        }
    }
}
