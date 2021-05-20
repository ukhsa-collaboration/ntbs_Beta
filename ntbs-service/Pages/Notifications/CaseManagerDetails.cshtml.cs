using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class CaseManagerDetailsModel : NotificationModelBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;
        public User CaseManagerDetails { get; set; }
        
        public IList<PHEC> RegionalMemberships { get; set; }
 
        public CaseManagerDetailsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IUserRepository userRepository,
            IReferenceDataRepository referenceDataRepository,
            INotificationRepository notificationRepository)
            : base(service, authorizationService, notificationRepository)
        {
            _userRepository = userRepository;
            _referenceDataRepository = referenceDataRepository;
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
            CaseManagerDetails = await _userRepository.GetUserByUsername(Notification.HospitalDetails.CaseManager.Username);
            CaseManagerDetails.CaseManagerTbServices = CaseManagerDetails.CaseManagerTbServices
                .OrderBy(x => x.TbService.PHEC.Name)
                .ThenBy(x => x.TbService.Name)
                .ToList();
            RegionalMemberships = await this._referenceDataRepository.GetPhecsByAdGroups(CaseManagerDetails.AdGroups);

            if (CaseManagerDetails == null)
            {
                return NotFound();
            }

            PrepareBreadcrumbs();
            return Page();
        }
    }
}
