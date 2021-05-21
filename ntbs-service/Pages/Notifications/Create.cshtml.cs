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
        private readonly IUserService _userService;

        public CreateModel(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if ((await _userService.GetUser(User)).IsReadOnly)
            {
                return RedirectToPage(RouteHelper.AccessDeniedPath);
            }
            var notification = await _notificationService.CreateNewNotificationForUserAsync(User);

            return RedirectToPage("./Edit/PatientDetails", new { notification.NotificationId });
        }
    }
}
