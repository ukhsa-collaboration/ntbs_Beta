using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.ContactDetails
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        
        public User ContactDetails { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            ContactDetails = await _userService.GetUser(User);
            ContactDetails.CaseManagerTbServices = ContactDetails.CaseManagerTbServices
                .OrderBy(x => x.TbService.Name)
                .ThenBy(x => x.TbService.PHEC.Name)
                .ToList();
            return Page();
        }
    }
}
