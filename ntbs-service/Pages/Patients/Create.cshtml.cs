using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Models;

namespace ntbs_service.Pages_Patients
{
    public class CreateModel : PageModel
    {
        private readonly NtbsContext _context;

        public CreateModel(NtbsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["RegionId"] = new SelectList(_context.Region, "RegionId", "Label");
            ViewData["SexId"] = new SelectList(_context.Sex, "SexId", "Label");
            return Page();
        }

        [BindProperty]
        public Patient Patient { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Patient.Add(Patient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}