using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit.Items
{
    public class MBovisOccupationExposureModel : NotificationEditModelBase
    {
        private readonly IItemRepository<MBovisOccupationExposure> _mBovisOccupationExposureRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;

        [BindProperty(SupportsGet = true)] 
        public int? RowId { get; set; }

        [BindProperty] 
        public MBovisOccupationExposure MBovisOccupationExposure { get; set; }

        public SelectList Countries { get; set; }

        public MBovisOccupationExposureModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IItemRepository<MBovisOccupationExposure> mBovisOccupationExposureRepository,
            IAlertRepository alertRepository) : base(service, authorizationService, notificationRepository, alertRepository)
        {
            _referenceDataRepository = referenceDataRepository;
            _mBovisOccupationExposureRepository = mBovisOccupationExposureRepository;

            Countries = new SelectList(
                _referenceDataRepository.GetAllCountriesAsync().Result,
                nameof(Country.CountryId),
                nameof(Country.Name));
        }

        // Pragma disabled 'not using async' to allow auto-magical wrapping in a Task
#pragma warning disable 1998
        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
#pragma warning restore 1998
        {
            if (!Notification.IsMBovis)
            {
                return NotFound();
            }
            
            if (RowId == null)
            {
                return Page();
            }

            MBovisOccupationExposure = Notification.MBovisDetails.MBovisOccupationExposures
                .SingleOrDefault(m => m.MBovisOccupationExposureId == RowId);
            if (MBovisOccupationExposure == null)
            {
                return NotFound();
            }

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            MBovisOccupationExposure.SetValidationContext(Notification);
            MBovisOccupationExposure.NotificationId = NotificationId;
            MBovisOccupationExposure.DobYear = Notification.PatientDetails.Dob?.Year;

            if (TryValidateModel(MBovisOccupationExposure, nameof(MBovisOccupationExposure)))
            {
                if (RowId == null)
                {
                    await _mBovisOccupationExposureRepository.AddAsync(MBovisOccupationExposure);
                }
                else
                {
                    MBovisOccupationExposure.MBovisOccupationExposureId = RowId.Value;
                    await _mBovisOccupationExposureRepository.UpdateAsync(Notification, MBovisOccupationExposure);
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Notification = await GetNotificationAsync(NotificationId);
            if (await AuthorizationService.GetPermissionLevelForNotificationAsync(User, Notification) != Models.Enums.PermissionLevel.Edit)
            {
                return ForbiddenResult();
            }

            var mBovisOccupationExposure = Notification.MBovisDetails.MBovisOccupationExposures
                .SingleOrDefault(m => m.MBovisOccupationExposureId == RowId);
            if (mBovisOccupationExposure == null)
            {
                return NotFound();
            }

            await _mBovisOccupationExposureRepository.DeleteAsync(mBovisOccupationExposure);

            return RedirectToPage("/Notifications/Edit/MBovisOccupationExposures", new {NotificationId});
        }

        public ContentResult OnGetValidateMBovisOccupationExposureProperty(string key, string value,
            bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<MBovisOccupationExposure>(key, value, shouldValidateFull);
        }

        protected override IActionResult RedirectForNotified()
        {
            return RedirectToPage("/Notifications/Edit/MBovisOccupationExposures", new {NotificationId});
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("/Notifications/Edit/MBovisOccupationExposures", new {NotificationId});
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithMBovisOccupationExposureAsync(notificationId);
        }
    }
}
