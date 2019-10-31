using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ntbs_service.Pages.Account
{
    public class AccessDenied : PageModel 
    {
        public AccessDenied()
        {
        }

        public PageResult OnGet()
        {
            return Page();
        }
    }
}
