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
        private readonly IUserRepository _userRepository;
        public User CaseManagerDetails { get; set; }

        public CaseManagerDetailsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IUserRepository userRepository,
            INotificationRepository notificationRepository) 
            : base(service, authorizationService, notificationRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }
            
            await TryGetLinkedNotificationsAsync();
            await AuthorizeAndSetBannerAsync();
            CaseManagerDetails = await _userRepository.GetUserByUsername(Notification.HospitalDetails.CaseManagerUsername);

            if (CaseManagerDetails == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
