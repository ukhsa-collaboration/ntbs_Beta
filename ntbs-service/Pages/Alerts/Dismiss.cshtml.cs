using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using System.Security.Claims;
using ntbs_service.DataAccess;
using System.Linq;

namespace ntbs_service.Pages_Notifications
{
    public class DismissModel : PageModel
    {
        private readonly IAlertRepository alertRepository;
        private readonly IAlertService alertService;
        private readonly IAuthorizationService authorizationService;
        [BindProperty]
        public int AlertId { get; set; }
        [BindProperty]
        public string TbServiceCode { get; set; }

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
            var alertToDismiss = await alertRepository.GetAlertByIdAsync(AlertId);
            if(await authorizationService.IsUserAuthorizedToManageAlert(User, alertToDismiss))
            {
                await alertService.DismissAlertAsync(AlertId, User.FindFirstValue(ClaimTypes.Email));
            }
            // TODO:NTBS-376 This will need to be changed to link to the correct place instead of just the home page
            return RedirectToPage("/Index");
        }
    }
}