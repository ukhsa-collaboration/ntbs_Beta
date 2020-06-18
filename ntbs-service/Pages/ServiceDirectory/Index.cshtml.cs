using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Pages.ServiceDirectory
{
    public class IndexModel : PageModel
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public IndexModel(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }
        
        public List<List<PHEC>> AllRegionsGrouped;
        
        public async Task<IActionResult> OnGetAsync()
        {
            var allRegions = await _referenceDataRepository.GetAllPhecs();
            
            const int chunkSize = 4;
            AllRegionsGrouped = allRegions.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            return Page();
        }
    }
}
