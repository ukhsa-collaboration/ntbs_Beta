using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Enums;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;
using System;
using System.Linq;
using ntbs_service.Helpers;

namespace ntbs_service.Pages_Notifications
{
    public class NotifyError {
        public string Url { get; set; }
        public List<string> ErrorMessages { get; set; }
    }

    public abstract class NotificationModelBase : ValidationModel
    {
        protected INotificationService service;

        public NotificationModelBase(INotificationService service) {
            this.service = service;
        }

        public Notification Notification { get; set; }

        [BindProperty]
        public int? NotificationId { get; set; }
        
        // This can be thrown away once proper banner work completes
        public NotificationStatus NotificationStatus { get; set; }

        [ViewData]
        public Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            // First Validate and Save current page details
            bool isValid = await ValidateAndSave(NotificationId);
            if (!isValid) 
            {
                return await OnGetAsync(NotificationId);
            }

            // Get Notifications with all owned properties to check for 
            Notification = await service.GetNotificationWithAllInfoAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }
            
            // IsRequired fields on Models requires ShouldValidateFull flag
            SetShouldValidateFull();

            if (!TryValidateModel(Notification))
            {
                NotifyErrorDictionary = NotificationValidationErrorGenerator.MapToDictionary(ModelState, NotificationId);
                return Partial("./NotificationErrorSummary", this);
            } 

            await service.SubmitNotification(Notification);
            
            return RedirectToPage("../Overview", new {id = NotificationId});
        }

        private void SetShouldValidateFull() 
        {
            Notification.ShouldValidateFull = true;
            foreach (var property in Notification.GetType().GetProperties()) {
                if (property.PropertyType.IsSubclassOf(typeof(ModelBase))) {
                    var ownedModel = property.GetValue(Notification);
                    ownedModel.GetType().GetProperty("ShouldValidateFull").SetValue(ownedModel, true);
                }
            }
            Notification.NotificationSites.ForEach(x => x.Notification = Notification);
        }

        public async Task<IActionResult> OnPostSaveAsync(int? notificationId)
        {
            bool isValid = await ValidateAndSave(notificationId);

            if (!isValid) {
                return await OnGetAsync(notificationId);
            }

            return RedirectToNextPage(notificationId);
        }

        protected void SetNotificationProperties<T>(bool isBeingSubmitted, T ownedModel) where T : ModelBase 
        {
            NotificationId = Notification.NotificationId;
            NotificationStatus = Notification.NotificationStatus;
            Notification.SetFullValidation(NotificationStatus, isBeingSubmitted);
            ownedModel.ShouldValidateFull = Notification.ShouldValidateFull;
        }

        protected abstract Task<bool> ValidateAndSave(int? notificationId);

        public abstract Task<IActionResult> OnGetAsync(int? notificationId, bool isBeingSubmitted = false);

        protected abstract IActionResult RedirectToNextPage(int? notificationId);
    }
}
