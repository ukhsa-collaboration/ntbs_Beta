using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Helpers;
using ntbs_service.Services;

namespace ntbs_service.Pages_Patients
{
    public class CreateModel : PageModel
    {
        private readonly IPatientService service;
        private readonly NtbsContext _context;
        private readonly IPatientRepository _repository;

        public SelectList Ethnicities { get; set;}
        public SelectList Countries { get; set; }
        public List<Sex> Sexes { get; set; }

        public CreateModel(IPatientService service, NtbsContext context, IPatientRepository repository)
        {
            this.service = service;
            _context = context;
            _repository = repository;
        }

        public IActionResult OnGet()
        {
            Ethnicities = new SelectList(_context.GetAllEthnicitiesAsync().Result, nameof(Ethnicity.EthnicityId), nameof(Ethnicity.Label));
            Countries = new SelectList(_context.GetAllCountriesAsync().Result, nameof(Country.CountryId), nameof(Country.Name));
            Sexes = _context.GetAllSexesAsync().Result.ToList();
            return Page();
        }

        [BindProperty]
        public Patient Patient { get; set; }
        [BindProperty]
        public FormattedDate FormattedDob { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            SetAndValidateBirthDate();

            if (!ModelState.IsValid)
            {
                return OnGet();
            }

            service.UpdateUkBorn(Patient);
            await _repository.AddPatientAsync(Patient);

            return RedirectToPage("./Index");
        }

        public ContentResult OnPostValidateProperty(string key, string value)
        {
            Patient.GetType().GetProperty(key).SetValue(Patient, value);
            if (TryValidateModel(Patient)) {
                return Content("");
            } else {
                var model = ModelState[key];
                return Content(ModelState[key].Errors[0].ErrorMessage);
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
                ModelState.AddModelError(patientKey, "Please enter a valid date");
                return;
            }
        }
    }
}