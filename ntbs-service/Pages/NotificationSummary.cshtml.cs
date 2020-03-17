using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NotificationSummary : PageModel
    {
        private readonly ValidationService _validationService;
        private readonly INotificationRepository _notificationRepository;

        public NotificationSummary(INotificationRepository notificationRepository)
        {
            _validationService = new ValidationService(this);
            _notificationRepository = notificationRepository;
        }

        // ReSharper disable once UnusedMember.Global
        public async Task<ContentResult> OnGetAsync(string NotificationId, bool allowDraft = false)
        {
            if (string.IsNullOrEmpty(NotificationId))
            {
                return _validationService.ValidContent();
            }

            if (!int.TryParse(NotificationId, out var notificationId))
            {
                return CreateJsonResponse(new
                {
                    validationMessage = ValidationMessages.RelatedNotificationIdMustBeInteger
                });
            }

            var relatedNotification = await _notificationRepository.GetNotificationAsync(notificationId);
            if (!(relatedNotification != null && (allowDraft || relatedNotification.HasBeenNotified)))
            {
                return CreateJsonResponse(new { validationMessage = ValidationMessages.IdDoesNotMatchNtbsRecord });
            }
            var info = NotificationInfo.CreateFromNotification(relatedNotification);
            return CreateJsonResponse(new { relatedNotification = info });
        }

        private ContentResult CreateJsonResponse(object content)
        {
            return Content(JsonConvert.SerializeObject(content), "application/json");
        }
    }
}
