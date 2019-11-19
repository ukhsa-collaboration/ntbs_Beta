using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.DataAccess;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ntbs_service.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IAlertRepository alertRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserService userService;

        public IndexModel(INotificationRepository notificationRepository, IAlertRepository alertRepository, 
            IAuthorizationService authorizationService, IUserService userService)
        {
            this.notificationRepository = notificationRepository;
            this.authorizationService = authorizationService;
            this.alertRepository = alertRepository;
            this.userService = userService;
        }

        public IList<Alert> Alerts { get; set; }
        public IList<Notification> DraftNotifications { get;set; }
        public IList<Notification> RecentNotifications { get;set; }

        public async Task OnGetAsync()
        {
            var draftNotificationsQueryable = await notificationRepository.GetDraftNotificationsAsync();
            var recentNotifications = await notificationRepository.GetRecentNotificationsAsync();
            DraftNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, draftNotificationsQueryable)).Take(10).ToList();
            RecentNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, recentNotifications)).Take(10).ToList();
            var services = await userService.GetTbServicesAsync(User);
            Alerts = await alertRepository.GetAlertsByTbServiceCodesAsync(services.Select(x => x.Code));
        }

        // public async Task<JsonResult> OnGetGetFilteredListsByTbService(string tbServiceCode)
        // {
        //     var tbServiceCodeAsList = new List<string> { tbServiceCode };
        //     Alerts = await alertRepository.GetAlertsByTbServiceCodesAsync(tbServiceCodeAsList);

        //     return new JsonResult(
        //         new FilteredEpisodePageSelectLists
        //         {
        //             Hospitals = filteredHospitals.Select(n => new ListEntry
        //             {
        //                 Value = n.HospitalId.ToString(),
        //                 Text = n.Name
        //             }),
        //             CaseManagers = filteredCaseManagers.Select(n => new ListEntry
        //             {
        //                 Value = n.Email,
        //                 Text = n.FullName
        //             })
        //         });
        // }
    }
}
