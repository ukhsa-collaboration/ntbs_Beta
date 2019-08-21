using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Pages_Patients
{
    public class DetailsModel : PageModel
    {
        private readonly IPatientRepository _repository;

        public DetailsModel(IPatientRepository repository)
        {
            _repository = repository;
        }

        public Patient Patient { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient = await _repository.GetPatientAsync(id);

            if (Patient == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
