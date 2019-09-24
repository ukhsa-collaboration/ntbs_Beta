using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Enums;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;
using System;
using System.Linq;

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

        private void AddErrorMessagesIntoDictonary(string displayName, string url, List<string> errorMessages) {
            if (errorMessages == null || errorMessages.Count == 0) {
                return;
            }

            if (!NotifyErrorDictionary.ContainsKey(displayName))
            {
                NotifyErrorDictionary.Add(displayName, new NotifyError {
                    Url = url,
                    ErrorMessages = errorMessages
                });
            }
            else 
            {
                NotifyErrorDictionary[displayName].ErrorMessages.AddRange(errorMessages);
            }
        }

        // This method converts Model State Errors to Dictionary of Errors to be used in view
        private void GetModelStateErrors() 
        {
            NotifyErrorDictionary = new Dictionary<string, NotifyError>();

            foreach (var key in ModelState.Keys) 
            {
                // Splitting on '[' as well due to List properties having index, ex. NotificationSites[0]
                var propertyKey = key.Split(new Char[] {'.', '['})[0];

                string url;
                string displayName;
                switch (propertyKey) {
                    case "PatientDetails":
                        url = getUrl("Patient");
                        displayName = "Patient Details";
                        break;
                    // NotificationSites is part of Clinical Details page despite being property of Notification
                    case "NotificationSites":
                        url = getUrl("ClinicalDetails");
                        displayName = "Clinical Details";
                        break;
                    case "ClinicalDetails":
                        url = getUrl("ClinicalDetails");
                        displayName = "Clinical Details";
                        break;
                    case "Episode":
                        url = getUrl("Episode");
                        displayName = "Hospital Details";
                        break;
                    case "PatientTBHistory":
                        url = getUrl("PreviousHistory");
                        displayName = "Previous History";
                        break;
                    default:
                        continue;
                }

                AddErrorMessagesIntoDictonary(displayName, url, 
                    ModelState[key]?.Errors?.Select(e => e.ErrorMessage).ToList());
            }
        }

        private string getUrl(string viewModelName) => $"/Notifications/Edit/{viewModelName}?id={NotificationId}&isBeingSubmitted=True";

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
