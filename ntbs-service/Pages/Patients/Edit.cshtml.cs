using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Patients
{
    public class EditModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly NtbsContext _context;

        public SelectList Ethnicities { get; set;}
        public SelectList Countries { get; set; }
        public List<Sex> Sexes { get; set; }


        public EditModel(INotificationService service, NtbsContext context)
        {
            this.service = service;
            _context = context;
        }

        [BindProperty]
        public PatientDetails Patient { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }
        [BindProperty]
        public FormattedDate FormattedDob { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var notification = await service.GetNotificationAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            NotificationId = notification.NotificationId;
            Patient = notification.PatientDetails;

            if (Patient == null) {
                Patient = new PatientDetails();
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

            var notification = await service.GetNotificationAsync(id);
            await service.UpdatePatientAsync(notification, Patient);
            
            return RedirectToPage("/ClinicalDetails/Edit", new {id = notification.NotificationId});
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