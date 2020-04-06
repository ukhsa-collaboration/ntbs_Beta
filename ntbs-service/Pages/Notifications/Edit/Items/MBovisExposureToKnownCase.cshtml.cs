using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit.Items
{
    public class MBovisExposureToKnownCaseModel : NotificationEditModelBase
    {
        private readonly IItemRepository<MBovisExposureToKnownCase> _mBovisExposureToKnownCasesRepository;

        [BindProperty(SupportsGet = true)] 
        public int? RowId { get; set; }

        [BindProperty] 
        public MBovisExposureToKnownCase MBovisExposureToKnownCase { get; set; }

        public MBovisExposureToKnownCaseModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IItemRepository<MBovisExposureToKnownCase> mBovisExposureToKnownCasesRepository,
            IAlertRepository alertRepository) : base(service, authorizationService, notificationRepository, alertRepository)
        {
            _mBovisExposureToKnownCasesRepository = mBovisExposureToKnownCasesRepository;
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

            MBovisExposureToKnownCase = Notification.MBovisDetails.MBovisExposureToKnownCases
                .SingleOrDefault(m => m.MBovisExposureToKnownCaseId == RowId);
            if (MBovisExposureToKnownCase == null)
            {
                return NotFound();
            }

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            UpdateFlags();
            MBovisExposureToKnownCase.SetValidationContext(Notification);
            MBovisExposureToKnownCase.NotificationId = NotificationId;
            MBovisExposureToKnownCase.DobYear = Notification.PatientDetails.Dob?.Year;

            await ValidateExposureNotification();

            if (TryValidateModel(MBovisExposureToKnownCase, nameof(MBovisExposureToKnownCase)))
            {
                if (RowId == null)
                {
                    await _mBovisExposureToKnownCasesRepository.AddAsync(MBovisExposureToKnownCase);
                }
                else
                {
                    MBovisExposureToKnownCase.MBovisExposureToKnownCaseId = RowId.Value;
                    await _mBovisExposureToKnownCasesRepository.UpdateAsync(Notification, MBovisExposureToKnownCase);
                }
            }
        }

        private void UpdateFlags()
        {
            if (MBovisExposureToKnownCase.NotifiedToPheStatus != Status.Yes)
            {
                MBovisExposureToKnownCase.ExposureNotificationId = null;
                ModelState.Remove("MBovisExposureToKnownCase.ExposureNotificationId");
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Notification = await GetNotificationAsync(NotificationId);
            if (await AuthorizationService.GetPermissionLevelForNotificationAsync(User, Notification) != Models.Enums.PermissionLevel.Edit)
            {
                return ForbiddenResult();
            }

            var mBovisExposureToKnownCase = Notification.MBovisDetails.MBovisExposureToKnownCases
                .SingleOrDefault(m => m.MBovisExposureToKnownCaseId == RowId);
            if (mBovisExposureToKnownCase == null)
            {
                return NotFound();
            }

            await _mBovisExposureToKnownCasesRepository.DeleteAsync(mBovisExposureToKnownCase);

            return RedirectToPage("/Notifications/Edit/MBovisExposureToKnownCases", new { NotificationId });
        }
        
        public ContentResult OnGetValidateMBovisExposureToKnownCaseProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<MBovisExposureToKnownCase>(key, value, shouldValidateFull);
        }
        
        private async Task ValidateExposureNotification()
        {
            if (MBovisExposureToKnownCase.ExposureNotificationId != null)
            {
                if (MBovisExposureToKnownCase.ExposureNotificationId == NotificationId)
                {
                    ModelState.AddModelError(
                        $"{nameof(MBovisExposureToKnownCase)}.{nameof(MBovisExposureToKnownCase.ExposureNotificationId)}",
                        ValidationMessages.RelatedNotificationIdCannotBeSameAsNotificationId);   
                }

                var exposureNotification = await NotificationRepository.GetNotificationAsync(
                    MBovisExposureToKnownCase.ExposureNotificationId.Value);
                if (exposureNotification == null)
                {
                    ModelState.AddModelError(
                        $"{nameof(MBovisExposureToKnownCase)}.{nameof(MBovisExposureToKnownCase.ExposureNotificationId)}",
                        ValidationMessages.IdDoesNotMatchNtbsRecord);
                }
            }
        }

        protected override IActionResult RedirectForNotified()
        {
            return RedirectToPage("/Notifications/Edit/MBovisExposureToKnownCases", new {NotificationId});
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("/Notifications/Edit/MBovisExposureToKnownCases", new {NotificationId});
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithMBovisExposureToKnownCasesAsync(notificationId);
        }
    }
}
