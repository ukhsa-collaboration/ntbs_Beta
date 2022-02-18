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
        public Dictionary<TBService, List<Hospital>> TbServicesWithHospitals;
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
                        .Where(cm => cm.IsActive)
                        .OrderBy(cm => cm.DisplayName)
                        .ToList()
                    );

            var tbServices = (await _referenceDataRepository.GetTbServicesFromPhecCodeAsync(PhecCode)).Select(t => t.Code);
            TbServicesWithHospitals = (await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(tbServices))
                .Where(h => h.IsLegacy != true)
                .GroupBy(h => h.TBService)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList());

            Phec = await _referenceDataRepository.GetPhecByCode(PhecCode);

            RegionalCaseManagers = await _referenceDataRepository.GetActiveUsersByPhecAdGroup(Phec.AdGroup);

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
