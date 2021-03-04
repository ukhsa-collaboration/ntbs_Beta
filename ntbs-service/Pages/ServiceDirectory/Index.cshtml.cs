using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Pages.ServiceDirectory
{
    public class IndexModel : ServiceDirectorySearchBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public IndexModel(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public IList<PHEC> AllRegions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (SearchKeyword != null && ModelState.IsValid)
            {
                return RedirectToPage("/ServiceDirectory/SearchResults", new { SearchKeyword });
            }

            AllRegions = await _referenceDataRepository.GetAllPhecs();

            PrepareBreadcrumbs();

            return Page();
        }


        private void PrepareBreadcrumbs()
        {
            var breadcrumbs = new List<Breadcrumb>
            {
                new Breadcrumb {Label = "Service Directory", Url = "/ServiceDirectory"},
            };

            ViewData["Breadcrumbs"] = breadcrumbs;
        }
    }
}
