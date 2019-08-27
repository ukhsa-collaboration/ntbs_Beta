using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Pages_Notifications
{
    public class CreateModel : PageModel
    {
        private readonly INotificationRepository _repository;

        public CreateModel(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var Notification = new Notification();
            Notification.Patient = new Patient();
            await _repository.AddNotificationAsync(Notification);
            return RedirectToPage("/Patients/Edit", new {id = Notification.Patient.PatientId });
        }

    }
}