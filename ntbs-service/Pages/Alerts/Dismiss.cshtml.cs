using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using System.Security.Claims;

namespace ntbs_service.Pages_Notifications
{
    public class DismissModel : PageModel
    {
        private readonly IAlertService alertService;
        [BindProperty]
        public int AlertId { get; set; }

        public DismissModel(IAlertService alertService)
        {
            this.alertService = alertService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await alertService.DismissAlertAsync(AlertId, "user");

            return RedirectToPage("/");
        }
    }
}