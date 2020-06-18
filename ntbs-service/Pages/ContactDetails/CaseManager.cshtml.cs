using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.ContactDetails
{
    public class CaseManagerModel : PageModel
    {
        private readonly IReferenceDataRepository _userRepository;

        public CaseManagerModel(IReferenceDataRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [BindProperty(SupportsGet = true)]
        public string Username { get; set; }
        
        public User ContactDetails { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            ContactDetails = await _userRepository.GetCaseManagerByUsernameAsync(Username);

            return Page();
        }
    }
}
