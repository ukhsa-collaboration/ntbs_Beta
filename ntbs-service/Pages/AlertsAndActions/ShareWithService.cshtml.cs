using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Pages.Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.AlertsAndActions
{
    public class ShareWithServiceModel : NotificationModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        public ValidationService ValidationService;
        
        [BindProperty]
        public ServiceShareRequestModel ServiceShareModel { get; set; }
        
        public SelectList TbServices { get; set; }

        public ShareWithServiceModel(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IUserHelper userHelper)
            : base(notificationService, authorizationService, userHelper, notificationRepository)
        {
            _referenceDataRepository = referenceDataRepository;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await SetNotificationAndAuthorize();
            if (PermissionLevel != PermissionLevel.Edit || Notification.IsShared)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            ServiceShareModel = new ServiceShareRequestModel { TBServiceCode = Notification.HospitalDetails.TBServiceCode };
            
            await SetDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SetNotificationAndAuthorize();
            if (PermissionLevel != PermissionLevel.Edit || Notification.IsShared)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }
            
            ModelState.Clear();
            TryValidateModel(ServiceShareModel, nameof(ServiceShareModel));
            
            if (!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                await AuthorizeAndSetBannerAsync();
                return Page();
            }

            await Service.ShareNotificationWithService(NotificationId, ServiceShareModel.SecondaryTBServiceCode, ServiceShareModel.ReasonForTBServiceShare);
            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

        private async Task SetNotificationAndAuthorize()
        {
            Notification = await GetNotificationAsync(NotificationId);
            await AuthorizeAndSetBannerAsync();
        }
        
        private async Task SetDropdownsAsync()
        {
            var tbServices = await _referenceDataRepository.GetAllActiveTbServicesAsync();
            TbServices = new SelectList(tbServices, nameof(TBService.Code), nameof(TBService.Name));
        }
    }
}
