using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class DeleteModel : NotificationModelBase
    {
        public ValidationService ValidationService { get; set; }
        [BindProperty]
        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        public string DeletionReason { get; set; }

        public DeleteModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository)
        {
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await NotificationRepository.GetNotificationAsync(id);
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
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (!ModelState.IsValid)
            {
                NotificationBannerModel = new NotificationBannerModel(Notification);
                return Page();
            }

            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                await Service.DeleteNotificationAsync(NotificationId, DeletionReason);
                return Partial("_DeleteConfirmation", this);
            }

            return RedirectToPage("/Notifications/Overview", new { id = NotificationId });
        }
    }
}
