using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class CreateModel : PageModel
    {
        private readonly INotificationService notificationService;

        public CreateModel(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var notification = await notificationService.CreateNewNotificationForUser(User);

            return RedirectToPage("./Edit/PatientDetails", new { notification.NotificationId });
        }
    }
}
