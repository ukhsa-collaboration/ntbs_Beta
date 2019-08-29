using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Patients
{
    public class EditModel : ValidationModel
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

        [BindProperty]
        public Patient Patient { get; set; }
        [BindProperty]
        public FormattedDate FormattedDob { get; set; }

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

            FormattedDob = Patient.Dob.ConvertToFormattedDate();
            Ethnicities = new SelectList(_context.GetAllEthnicitiesAsync().Result, nameof(Ethnicity.EthnicityId), nameof(Ethnicity.Label));
            Countries = new SelectList(_context.GetAllCountriesAsync().Result, nameof(Country.CountryId), nameof(Country.Name));
            Sexes = _context.GetAllSexesAsync().Result.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            SetAndValidateDate(Patient, nameof(Patient.Dob), FormattedDob);

            if (!ModelState.IsValid)
            {
                return await OnGetAsync(id);
            }

            await service.UpdatePatientAsync(Patient);
            
            return RedirectToPage("./Index");
        }

        public ContentResult OnPostValidatePatientProperty(string key, string value)
        {
            return OnPostValidateProperty(Patient, key, value);
        }

        public ContentResult OnPostValidatePatientDate(string key, string day, string month, string year)
        {
            return OnPostValidateDate(Patient, key, day, month, year);
        }
    }
}