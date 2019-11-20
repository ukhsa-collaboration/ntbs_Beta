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
        private readonly IUserService userService;
        [BindProperty]
        public int AlertId { get; set; }

        public DismissModel(IAlertService alertService, IAlertRepository alertRepository, IUserService userService)
        {
            this.alertService = alertService;
            this.alertRepository = alertRepository;
            this.userService = userService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var alertToDismiss = await alertRepository.GetAlertByIdAsync(AlertId);
            var userTbServiceCodes = (await userService.GetTbServicesAsync(User)).Select(s => s.Code);
            if(userTbServiceCodes.Contains(alertToDismiss.TbServiceCode))
            {
                await alertService.DismissAlertAsync(AlertId, "user");
            }
            return RedirectToPage("/Index");
        }
    }
}