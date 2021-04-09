using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Pages.ServiceDirectory
{
    public class RegionModel : PageModel
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public RegionModel(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string PhecCode { get; set; }

        public Dictionary<TBService, List<User>> TbServicesWithCaseManagers;
        public PHEC Phec;
        public IList<User> RegionalCaseManagers;

        public async Task<IActionResult> OnGetAsync()
        {
            TbServicesWithCaseManagers =
                (await _referenceDataRepository.GetTbServicesWithCaseManagersFromPhecCodeAsync(PhecCode))
                .ToDictionary(
                    service => service,
                    service => service.CaseManagerTbServices
                        .Select(c => c.CaseManager)
                        .OrderBy(c => c.DisplayName)
                        .ToList()
                    );

            Phec = await _referenceDataRepository.GetPhecByCode(PhecCode);

            RegionalCaseManagers = await _referenceDataRepository.GetRegionalTeamByPhecAdGroup(Phec.AdGroup);

            PrepareBreadcrumbs();

            return Page();
        }

        private void PrepareBreadcrumbs()
        {
            var breadcrumbs = new List<Breadcrumb>
            {
                new Breadcrumb {Label = "Service Directory", Url = "/ServiceDirectory"},
                new Breadcrumb {Label = Phec.Name, Url = $"/ServiceDirectory/Region/{Phec.Code}"}
            };

            ViewData["Breadcrumbs"] = breadcrumbs;
        }
    }
}
