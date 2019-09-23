using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Enums;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;
using System;

namespace ntbs_service.Pages_Notifications
{
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
        public Dictionary<string, List<string>> ErrorDictionary { get; set; }

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

            if (!IsModelValid())
            {
                GetModelStateErrors();
                return Partial("./NotificationErrorSummary", this);
            } 

            await service.SubmitNotification(Notification);
            
            return RedirectToPage("../Overview", new {id = NotificationId});
        }

        private bool IsModelValid()
        {
            return TryValidateModel(Notification);
        }

        private void SetShouldValidateFull() 
        {
            Notification.ShouldValidateFull = true;
            Notification.SocialRiskFactors.ShouldValidateFull = true;
            Notification.PatientDetails.ShouldValidateFull = true;
            Notification.PatientTBHistory.ShouldValidateFull = true;
            Notification.ContactTracing.ShouldValidateFull = true;
            Notification.ClinicalDetails.ShouldValidateFull = true;
            Notification.Episode.ShouldValidateFull = true;
            Notification.NotificationSites.ForEach(x => x.Notification = Notification);
        }

        // This method converts Model State Errors to Dictionary of Errors to be used in view
        private void GetModelStateErrors() 
        {
            ErrorDictionary = new Dictionary<string, List<string>>();

            foreach (var key in ModelState.Keys) 
            {
                // Splitting '[' as well due to List properties having index [0]
                var propertyKey = key.Split(new Char[] {'.', '['})[0];
                // Since NotificationSites is part of Notification class we need to change the key name to get correct mapping
                propertyKey = propertyKey == "NotificationSites" ? "ClinicalDetails" : propertyKey;

                if (!ErrorDictionary.ContainsKey(propertyKey))
                {
                    ErrorDictionary.Add(propertyKey, new List<string>());
                }

                foreach (var error in ModelState[key].Errors) 
                {
                    ErrorDictionary[propertyKey].Add(error.ErrorMessage);
                }
            }
        }

        public async Task<IActionResult> OnPostSaveAsync(int? notificationId)
        {
            bool isValid = await ValidateAndSave(notificationId);

            if (!isValid) {
                return await OnGetAsync(notificationId);
            }

            return RedirectToNextPage(notificationId);
        }

        protected abstract Task<bool> ValidateAndSave(int? notificationId);

        public abstract Task<IActionResult> OnGetAsync(int? notificationId, bool isBeingSubmitted = false);

        protected abstract IActionResult RedirectToNextPage(int? notificationId);
    }
}
