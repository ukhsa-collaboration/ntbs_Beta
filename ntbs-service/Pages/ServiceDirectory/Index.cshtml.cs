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
        
        public IList<PHEC> AllRegions { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            AllRegions = await _referenceDataRepository.GetAllPhecs();

            return Page();
        }
    }
}
