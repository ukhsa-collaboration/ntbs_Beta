using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class CaseManagerDetailsModel : NotificationModelBase
    {
        private readonly IUserService _userService;
        public User CaseManagerDetails { get; set; }

        public CaseManagerDetailsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IUserService userService,
            INotificationRepository notificationRepository) 
            : base(service, authorizationService, notificationRepository)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }
            
            await AuthorizeAndSetBannerAsync();
            CaseManagerDetails = await _userService.GetUser(User);

            return Page();
        }
    }
}
