using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class MBovisAnimalExposuresModel : NotificationEditModelBase
    {
        public MBovisAnimalExposuresModel(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(notificationService, authorizationService,
            notificationRepository)
        {
            CurrentPage = NotificationSubPaths.EditMBovisAnimalExposures;
        }

        [BindProperty] 
        public MBovisDetails MBovisDetails { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            if (!Notification.IsMBovis)
            {
                return NotFound();
            }
            
            MBovisDetails = Notification.MBovisDetails;
            
            await SetNotificationProperties(isBeingSubmitted, MBovisDetails);

            if (MBovisDetails.ShouldValidateFull)
            {
                TryValidateModel(this);
            }

            return Page();
        }
        
        protected override IActionResult RedirectToCreate()
        {
            return RedirectToPage("./Items/NewMBovisAnimalExposure", new { NotificationId });
        }
        
        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./MBovisAnimalExposures", new { NotificationId, isBeingSubmitted });
        }
        
        protected override async Task ValidateAndSave()
        {
            // Set the collection so it can be included in the validation
            MBovisDetails.MBovisAnimalExposures = Notification.MBovisDetails.MBovisAnimalExposures;
            MBovisDetails.ProceedingToAdd = ActionName == "Create";
            MBovisDetails.SetValidationContext(Notification);
            
            if (TryValidateModel(MBovisDetails, nameof(MBovisDetails)))
            {
                await Service.UpdateMBovisDetailsAnimalExposure(Notification, MBovisDetails);
            }
        }
        
        public ContentResult OnGetValidateMBovisDetailsProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<MBovisDetails>(key, value, shouldValidateFull);
        }
        
        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithMBovisAnimalExposures(notificationId);
        }
    }
}
