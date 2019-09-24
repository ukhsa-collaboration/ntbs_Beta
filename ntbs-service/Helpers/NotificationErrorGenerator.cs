using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ntbs_service.Pages_Notifications;

namespace ntbs_service.Helpers
{
    public static class NotificationValidationErrorGenerator {
        private static Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }

        // This method converts Model State Errors to Dictionary of Errors to be used in view
        public static Dictionary<string, NotifyError> MapToDictionary(ModelStateDictionary modelState, int? notificationId) 
        {
            NotifyErrorDictionary = new Dictionary<string, NotifyError>();

            foreach (var key in modelState.Keys) 
            {
                // Splitting on '[' as well due to List properties having index, ex. NotificationSites[0]
                var propertyKey = key.Split(new Char[] {'.', '['})[0];

                string url;
                string displayName;
                switch (propertyKey) {
                    case "PatientDetails":
                        url = getUrl("Patient", notificationId);
                        displayName = "Patient Details";
                        break;
                    // NotificationSites is part of Clinical Details page despite being property of Notification
                    case "NotificationSites":
                        url = getUrl("ClinicalDetails", notificationId);
                        displayName = "Clinical Details";
                        break;
                    case "ClinicalDetails":
                        url = getUrl("ClinicalDetails", notificationId);
                        displayName = "Clinical Details";
                        break;
                    case "Episode":
                        url = getUrl("Episode", notificationId);
                        displayName = "Hospital Details";
                        break;
                    case "PatientTBHistory":
                        url = getUrl("PreviousHistory", notificationId);
                        displayName = "Previous History";
                        break;
                    default:
                        continue;
                }

                AddErrorMessagesIntoDictonary(displayName, url, 
                    modelState[key]?.Errors?.Select(e => e.ErrorMessage).ToList());
            }

            return NotifyErrorDictionary;
        }

        private static void AddErrorMessagesIntoDictonary(string displayName, string url, List<string> errorMessages) {
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


        private static string getUrl(string viewModelName, int? notificationId) => 
            $"/Notifications/Edit/{viewModelName}?id={notificationId}&isBeingSubmitted=True";
    }
}