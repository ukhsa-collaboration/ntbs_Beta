using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Pages_Notifications
{
    public class CreateModel : PageModel
    {
        private readonly INotificationRepository repository;

        public CreateModel(INotificationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var notification = new Notification
            {
                CreationDate = DateTime.Now
            };
            await repository.AddNotificationAsync(notification);

            return RedirectToPage("./Edit/Patient", new { id = notification.NotificationId });
        }

    }
}