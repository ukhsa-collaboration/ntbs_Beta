using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Pages_Patients
{
    public class CreateModel : PageModel
    {
        private readonly IPatientRepository _repository;

        public CreateModel(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var Patient = new Patient();
            await _repository.AddPatientAsync(Patient);
            return RedirectToPage("./Edit", new {id = Patient.PatientId });
        }

    }
}