using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Enums;
using ntbs_service.Models;
using ntbs_service.Services;
using ntbs_service.Helpers;

namespace ntbs_service.Pages_Notifications
{
    public class NotifyError {
        public string Url { get; set; }
        public List<string> ErrorMessages { get; set; }
    }

    // Needed by all Notification edit pages
    public abstract class NotificationEditModelBase : NotificationModelBase
    {
        protected ValidationService validationService;

        public NotificationEditModelBase(INotificationService service) : base(service)
        {
            validationService = new ValidationService(this);
        }

        [ViewData]
        public Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            // Get Notifications with all owned properties to check for 
            Notification = await service.GetNotificationWithAllInfoAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }
            
            // First Validate and Save current page details
            bool isValid = await ValidateAndSave();
            if (!isValid) 
            {
                return await OnGetAsync(NotificationId);
            }


            // IsRequired fields on Models requires ShouldValidateFull flag
            SetShouldValidateFull();

            if (!TryValidateModel(Notification))
            {
                NotifyErrorDictionary = NotificationValidationErrorGenerator.MapToDictionary(ModelState, NotificationId);
                return Partial("./NotificationErrorSummary", this);
            } 

            await service.SubmitNotification(Notification);
            
            return RedirectToOverview();
        }

        private void SetShouldValidateFull() 
        {
            Notification.ShouldValidateFull = true;
            foreach (var property in Notification.GetType().GetProperties()) 
            {
                if (property.PropertyType.IsSubclassOf(typeof(ModelBase))) 
                {
                    var ownedModel = property.GetValue(Notification);
                    ownedModel.GetType().GetProperty("ShouldValidateFull").SetValue(ownedModel, true);
                }
            }
            Notification.NotificationSites.ForEach(x => x.Notification = Notification);
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {            
            Notification = await service.GetNotificationAsync(NotificationId);
            bool isValid = await ValidateAndSave();

            if (!isValid) 
            {
                return await OnGetAsync(NotificationId);
            }

            if (Notification?.NotificationStatus == NotificationStatus.Notified) 
            {
                return RedirectToOverview();
            }

            return RedirectToNextPage(NotificationId);
        }

        private IActionResult RedirectToOverview() 
        {
            return RedirectToPage("../Overview", new {id = NotificationId});
        }

        protected async Task SetNotificationProperties<T>(bool isBeingSubmitted, T ownedModel) where T : ModelBase
        {
            NotificationId = Notification.NotificationId;
            Notification.SetFullValidation(Notification.NotificationStatus, isBeingSubmitted);
            ownedModel.ShouldValidateFull = Notification.ShouldValidateFull;

            await GetLinkedNotifications();
        }

        public bool IsValid(string key)
        {
            return validationService.IsValid(key);
        }

        protected abstract Task<bool> ValidateAndSave();

        public abstract Task<IActionResult> OnGetAsync(int notificationId, bool isBeingSubmitted = false);

        protected abstract IActionResult RedirectToNextPage(int? notificationId);
    }
}
