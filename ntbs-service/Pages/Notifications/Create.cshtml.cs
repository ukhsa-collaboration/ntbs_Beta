using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class CreateModel : PageModel
    {
        private readonly INotificationService service;

        public CreateModel(INotificationService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var notification = await service.CreateNewNotificationForUser(User);

            return RedirectToPage("./Edit/Patient", new { id = notification.NotificationId });
        }

    }
}