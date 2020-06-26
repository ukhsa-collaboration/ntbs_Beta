using System.Linq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using ntbs_service.DataAccess;
using Microsoft.Extensions.Primitives;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.ContactDetails
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IReferenceDataRepository _userRepository;
        
        public IndexModel(IUserService userService, IReferenceDataRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }
        
        public User ContactDetails { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string Username { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            ContactDetails = (Username == null)
                ? await _userService.GetUser(User)
                : await _userRepository.GetCaseManagerByUsernameAsync(Username);
            
            if (ContactDetails == null)
            {
                return NotFound();
            }
            
            ViewData["IsEditable"] = Username == null || Username == User.FindFirstValue(ClaimTypes.Upn);
            
            ContactDetails.CaseManagerTbServices = ContactDetails.CaseManagerTbServices
                .OrderBy(x => x.TbService.PHEC.Name)
                .ThenBy(x => x.TbService.Name)
                .ToList();

            ViewData["Referer"] = StringValues.IsNullOrEmpty(Request.Headers["Referer"])
                ? "/"
                : Request.Headers["Referer"].ToString(); 

            return Page();
        }
    }
}
