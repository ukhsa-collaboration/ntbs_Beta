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
    public class IndexModel : PageModel
    {
        private readonly ntbs_service.Models.NtbsContext _context;

        public IndexModel(ntbs_service.Models.NtbsContext context)
        {
            _context = context;
        }

        public IList<Patient> Patient { get;set; }

        public async Task OnGetAsync()
        {
            Patient = await _context.Patient
                .Include(p => p.Region)
                .Include(p => p.Sex).ToListAsync();
        }
    }
}
