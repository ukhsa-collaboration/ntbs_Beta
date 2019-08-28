using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages_Patients
{
    public class EditModel : PageModel
    {
        private readonly IPatientService service;
        private readonly NtbsContext _context;

        public SelectList Ethnicities { get; set;}
        public SelectList Countries { get; set; }
        public List<Sex> Sexes { get; set; }

        public EditModel(IPatientService service, NtbsContext context)
        {
            this.service = service;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient = await service.GetPatientAsync(id);

            if (Patient == null)
            {
                return NotFound();
            }

            Ethnicities = new SelectList(_context.GetAllEthnicitiesAsync().Result, nameof(Ethnicity.EthnicityId), nameof(Ethnicity.Label));
            Countries = new SelectList(_context.GetAllCountriesAsync().Result, nameof(Country.CountryId), nameof(Country.Name));
            Sexes = _context.GetAllSexesAsync().Result.ToList();
            return Page();
        }

        [BindProperty]
        public Patient Patient { get; set; }
        [BindProperty]
        public FormattedDate FormattedDob { get; set; }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            SetAndValidateBirthDate();

            if (!ModelState.IsValid)
            {
                return await OnGetAsync(id);
            }

            await service.UpdatePatientAsync(Patient);

            return RedirectToPage("./Index");
        }

        public ContentResult OnPostValidateProperty(string key, string value)
        {
            Patient.GetType().GetProperty(key).SetValue(Patient, value);
            return GetValidationResult(key);
        }

        private ContentResult GetValidationResult(string key)
        {
            if (TryValidateModel(Patient))
            {
                return Content("");
            }
            else
            {
                var model = ModelState[key];
                return Content(ModelState[key].Errors[0].ErrorMessage);
            }
        }

        public ContentResult OnPostValidateDate(string key, string day, string month, string year)
        {
            DateTime? convertedDob;
            var formattedDate = new FormattedDate() { Day = day, Month = month, Year = year };
            if (formattedDate.TryConvertToDateTime(out convertedDob)) {
                Patient.GetType().GetProperty(key).SetValue(Patient, convertedDob);
                return GetValidationResult(key);
            }
            else
            {
                return Content(ValidationMessage.ValidDate);
            }
        }

        public bool IsValid(string key)
        {
            return ModelState[key] == null ? true : ModelState[key].Errors.Count == 0;
        }

        private void SetAndValidateBirthDate()
        {
            if (FormattedDob.IsEmpty()) {
                return;
            }

            var patientKey = "Patient.Dob";
            DateTime? convertedDob;

            if (FormattedDob.TryConvertToDateTime(out convertedDob)) {
                Patient.Dob = convertedDob;
                TryValidateModel(Patient, "Patient");
            }
            else
            {
                ModelState.AddModelError(patientKey, ValidationMessage.ValidDate);
                return;
            }
        }
    }
}