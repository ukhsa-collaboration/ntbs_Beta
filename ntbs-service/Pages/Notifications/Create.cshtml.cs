using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Helpers;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class CreateModel : PageModel
    {
        private readonly INotificationService _notificationService;
        private readonly IUserHelper _userHelper;

        public CreateModel(INotificationService notificationService, IUserHelper userHelper)
        {
            _notificationService = notificationService;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_userHelper.UserIsReadOnly(User))
            {
                return RedirectToPage(RouteHelper.AccessDeniedPath);
            }
            var notification = await _notificationService.CreateNewNotificationForUserAsync(User);

            return RedirectToPage("./Edit/PatientDetails", new { notification.NotificationId });
        }
    }
}
