using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;

namespace MyApp.Namespace
{
    [Authorize("AdminOnly")]
    public class UsersSyncModel : PageModel
    {
        private readonly IAdImportService _adImportService;

        public UsersSyncModel(IAdImportService adImportService)
        {
            _adImportService = adImportService;
        }

        public void OnGet()
        {
        }

        public async Task OnPost()
        {
            await _adImportService.RunCaseManagerImportAsync();
            ViewData.Add("Result", "Success");
        }
    }
}
