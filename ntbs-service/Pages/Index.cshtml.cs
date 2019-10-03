using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;

namespace ntbs_service.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INotificationService Service;
        private readonly ITbServiceManager TbServicesManager;

        public IndexModel(INotificationService service, ITbServiceManager tbServiceManager)
        {
            Service = service;
            TbServicesManager = tbServiceManager;
        }

        public IList<Notification> DraftNotifications { get;set; }
        public IList<Notification> RecentNotifications { get;set; }

        public async Task OnGetAsync()
        {
            List<TBService> services = await TbServicesManager.ForUser(User);
            DraftNotifications = await Service.GetDraftNotificationsAsync(services);
            RecentNotifications = await Service.GetRecentNotificationsAsync(services);
        }
    }
}
