using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Data;
using ntbs_service.Models;

namespace ntbs_service.Pages_Patients
{
    public class EditModel : PageModel
    {
        private readonly NtbsContext _context;
        private readonly IPatientRepository _repository;

        public EditModel(NtbsContext context, PatientRepository repository)
        {
            _context = context;
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
            ViewData["RegionId"] = new SelectList(_context.Region, "RegionId", "Label");
            ViewData["SexId"] = new SelectList(_context.Sex, "SexId", "Label");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _repository.UpdatePatientAsync(Patient);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.PatientExists(Patient.PatientId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
