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

        public NotificationModelBase(INotificationService service) 
        {
            this.service = service;
        }

        public Notification Notification { get; set; }
        public NotificationBannerModel NotificationBannerModel { get; set; }

        [BindProperty]
        public int NotificationId { get; set; }

        [ViewData]
        public Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }

        /* 
        Post method accepts name of action specified by button clicked.
        Using handler adds handler onto url and therefore breaking javascript 
        validation hapening after form is submitted
        */ 
        public async Task<IActionResult> OnPostAsync(string actionName)
        {
            // Get Notifications with all owned properties to check for 
            Notification = await service.GetNotificationWithAllInfoAsync(NotificationId); 
            if (Notification == null)
            {
                return NotFound();
            }
            NotificationBannerModel = new NotificationBannerModel(Notification);

            switch (actionName) 
            {
                case "Save":
                    return await Save();
                case "Submit":
                    return await Submit();
                default:
                    return Page();
            }
        }

        public async Task<IActionResult> Submit()
        {
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

        public async Task<IActionResult> OnPostCreateLinkAsync()
        {
            var notification = await service.GetNotificationAsync(NotificationId);
            var linkedNotification = await service.CreateLinkedNotificationAsync(notification);

            return RedirectToPage("/Notifications/Edit/Patient", new {id = linkedNotification.NotificationId});
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

        public async Task<IActionResult> Save()
        {           
            Notification.SetFullValidation(Notification.NotificationStatus);
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

        protected void SetNotificationProperties<T>(bool isBeingSubmitted, T ownedModel) where T : ModelBase 
        {
            NotificationId = Notification.NotificationId;
            Notification.SetFullValidation(Notification.NotificationStatus, isBeingSubmitted);
            ownedModel.ShouldValidateFull = Notification.ShouldValidateFull;
        }

        public ContentResult ValidateModelProperty<T>(string key, object value, bool shouldValidateFull) where T : ModelBase
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            model.ShouldValidateFull = shouldValidateFull;
            return ValidateProperty(model, key, value);
        }

        protected abstract Task<bool> ValidateAndSave();

        public abstract Task<IActionResult> OnGetAsync(int notificationId, bool isBeingSubmitted = false);

        protected abstract IActionResult RedirectToNextPage(int? notificationId);
    }
}
