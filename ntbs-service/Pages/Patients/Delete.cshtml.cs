using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Data;
using ntbs_service.Models;

namespace ntbs_service.Pages_Patients
{
    public class DeleteModel : PageModel
    {
        private readonly IPatientRepository _repository;

        public DeleteModel(IPatientRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient = await _repository.FindPatientByIdAsync(id);

            if (Patient != null)
            {
                await _repository.DeletePatientAsync(Patient);
            }

            return RedirectToPage("./Index");
        }
    }
}
