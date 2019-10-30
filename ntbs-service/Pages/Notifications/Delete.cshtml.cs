using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ntbs_service.Pages.Notifications
{
    public class DeleteModel : NotificationModelBase
    {
        public ValidationService  ValidationService { get; set; }
        [BindProperty]
        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine, 
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        public string DeletionReason { get; set; }

        public DeleteModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService)
        {
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;
            if (Notification.NotificationStatus != NotificationStatus.Draft)
            {
                return RedirectToPage("/Notifications/Overview", new { id = NotificationId });
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            Notification = await service.GetNotificationAsync(NotificationId);
            if (!ModelState.IsValid)
            {
                NotificationBannerModel = new NotificationBannerModel(Notification);
                return Page();
            }

            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                await service.DeleteNotification(NotificationId, DeletionReason);
                return Partial("_DeleteConfirmation", this);
            }

            return RedirectToPage("/Notifications/Overview", new { id = NotificationId });
        }
    }
}