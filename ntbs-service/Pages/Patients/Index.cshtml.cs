using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models;
using ntbs_service.DataAccess;

namespace ntbs_service.Pages_Patients
{
    public class IndexModel : PageModel
    {
        private readonly INotificationRepository _repository;

        public IndexModel(INotificationRepository repository)
        {
            _repository = repository;
        }

        public IList<Notification> Notifications { get;set; }

        public async Task OnGetAsync()
        {
            Notifications = await _repository.GetNotificationsWithPatientsAsync();
        }
    }
}
