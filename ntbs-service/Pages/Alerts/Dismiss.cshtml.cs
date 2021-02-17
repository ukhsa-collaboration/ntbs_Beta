using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Services;

namespace ntbs_service.Pages.Alerts
{
    public class DismissModel : PageModel
    {
        private readonly IAlertRepository alertRepository;
        private readonly IAlertService alertService;
        private readonly IAuthorizationService authorizationService;
        [BindProperty]
        public int AlertId { get; set; }

        public DismissModel(IAlertService alertService, 
            IAlertRepository alertRepository,
            IAuthorizationService authorizationService)
        {
            this.alertService = alertService;
            this.alertRepository = alertRepository;
            this.authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var alertToDismiss = await alertRepository.GetOpenAlertByIdAsync(AlertId);
            
            if(await authorizationService.IsUserAuthorizedToManageAlert(User, alertToDismiss))
            {
                await alertService.DismissAlertAsync(AlertId, User.Username());
            }

            if (Request.Query["page"] == "Overview")
            {
                return RedirectToPage("/Notifications/Overview", new { alertToDismiss.NotificationId });
            }

            return RedirectToPage("/Index");
        }
    }
}
