using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

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

        public IList<TBService> TbServices;
        public PHEC Phec;

        public async Task<IActionResult> OnGetAsync()
        {
            TbServices = await _referenceDataRepository.GetTbServicesWithCaseManagersFromPhecCodeAsync(PhecCode);
            TbServices.ForEach(x => x.CaseManagerTbServices = x.CaseManagerTbServices
                .Where(c => c.CaseManager.IsCaseManager)
                .ToList());

            Phec = await _referenceDataRepository.GetPhecByCode(PhecCode);
            
            return Page();
        }
    }
}
