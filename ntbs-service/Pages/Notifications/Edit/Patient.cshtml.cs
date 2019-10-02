using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages;
using ntbs_service.Services;
using System;

namespace ntbs_service.Pages_Notifications
{
    public class PatientModel : NotificationModelBase
    {
        private readonly NtbsContext context;
        private readonly IAuditService auditService;

        public SelectList Ethnicities { get; set; }
        public SelectList Countries { get; set; }
        public List<Sex> Sexes { get; set; }

        [BindProperty]
        public PatientDetails Patient { get; set; }

        [BindProperty]
        public FormattedDate FormattedDob { get; set; }

        public PatientModel(INotificationService service, NtbsContext context, IAuditService auditService) : base(service)
        {
            this.context = context;
            this.auditService = auditService;
        }

        public override async Task<IActionResult> OnGetAsync(int? id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            Patient = Notification.PatientDetails;
            if (Patient == null)
            {
                Patient = new PatientDetails();
            }

            SetNotificationProperties<PatientDetails>(isBeingSubmitted, Patient);
            if (Patient.ShouldValidateFull)
            {
                TryValidateModel(Patient, "Patient");
            }

            FormattedDob = Patient.Dob.ConvertToFormattedDate();
            Ethnicities = new SelectList(context.GetAllEthnicitiesAsync().Result, nameof(Ethnicity.EthnicityId), nameof(Ethnicity.Label));
            Countries = new SelectList(context.GetAllCountriesAsync().Result, nameof(Country.CountryId), nameof(Country.Name));
            Sexes = context.GetAllSexesAsync().Result.ToList();

            await auditService.OnGetAuditAsync(Notification.NotificationId, Patient);
            return Page();
        }

        protected override async Task<bool> ValidateAndSave() {
            UpdatePatientFlags();
            SetAndValidateDateOnModel(Patient, nameof(Patient.Dob), FormattedDob);
            
            if (!ModelState.IsValid)
            {
                return false;
            }

            await service.UpdatePatientAsync(Notification, Patient);
            return true;
        }

        private void UpdatePatientFlags() {
            if (Patient.NhsNumberNotKnown) {
                Patient.NhsNumber = null;
                ModelState.Remove("Patient.NhsNumber");
            }

            if (Patient.NoFixedAbode) {
                Patient.Postcode = null;
                ModelState.Remove("Patient.Postcode");
            }
        }

        protected override IActionResult RedirectToNextPage(int? notificationId) {
            return RedirectToPage("./Episode", new {id = notificationId});
        }

        public ContentResult OnGetValidatePatientProperty(string key, string value)
        {
            return ValidateProperty(new PatientDetails(), key, value);
        }

        public ContentResult OnGetValidatePatientDate(string key, string day, string month, string year)
        {
            return ValidateDate(new PatientDetails(), key, day, month, year);
        }
    }
}