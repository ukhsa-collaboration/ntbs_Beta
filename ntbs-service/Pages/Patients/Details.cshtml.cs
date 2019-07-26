using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.Pages_Patients
{
    public class DetailsModel : PageModel
    {
        private readonly ntbs_service.Models.NtbsContext _context;

        public DetailsModel(ntbs_service.Models.NtbsContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
