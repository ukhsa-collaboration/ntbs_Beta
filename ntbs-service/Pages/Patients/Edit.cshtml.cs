using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.Pages_Patients
{
    public class EditModel : PageModel
    {
        private readonly ntbs_service.Models.NtbsContext _context;

        public EditModel(ntbs_service.Models.NtbsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patient Patient { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient = await _context.Patient
                .Include(p => p.Region)
                .Include(p => p.Sex).FirstOrDefaultAsync(m => m.PatientId == id);

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

            _context.Attach(Patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(Patient.PatientId))
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

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.PatientId == id);
        }
    }
}
