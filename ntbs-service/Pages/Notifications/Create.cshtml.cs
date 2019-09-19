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
            var notification = new Notification();
            notification.PatientDetails = new PatientDetails();
            notification.Episode = new Episode();
            notification.ClinicalDetails = new ClinicalDetails();
            notification.SocialRiskFactors = new SocialRiskFactors();
            notification.ContactTracing = new ContactTracing();
            notification.PatientTBHistory = new PatientTBHistory();
            await repository.AddNotificationAsync(notification);

            return RedirectToPage("./Edit/Patient", new { id = notification.NotificationId });
        }

    }
}