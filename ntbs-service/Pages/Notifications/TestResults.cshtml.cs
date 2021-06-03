using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class ViewTestResultsModel : NotificationModelBase
    {
        private readonly ICultureAndResistanceService _cultureAndResistanceService;
        private readonly ISpecimenService _specimenService;

        public ViewTestResultsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            ICultureAndResistanceService cultureAndResistanceService,
            ISpecimenService specimenService) : base(service, authorizationService, notificationRepository)
        {
            _cultureAndResistanceService = cultureAndResistanceService;
            _specimenService = specimenService;
        }

        [BindProperty]
        public TestData TestData { get; set; }
        public CultureAndResistance CultureAndResistance { get; set; }
        public IEnumerable<MatchedSpecimen> Specimens { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationWithAllInfoAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }

            await GetLinkedNotificationsAsync();
            await AuthorizeAndSetBannerAsync();
            if (PermissionLevel == PermissionLevel.None)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }
            
            TestData = Notification.TestData;
            CultureAndResistance = await _cultureAndResistanceService.GetCultureAndResistanceDetailsAsync(NotificationId);
            Specimens = await _specimenService.GetMatchedSpecimenDetailsForNotificationAsync(NotificationId);

            return Page();
        }
    }
}
