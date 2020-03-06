using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ntbs_service.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class Index : PageModel
    {
        public void OnGet()
        {
            
        }
    }
}
