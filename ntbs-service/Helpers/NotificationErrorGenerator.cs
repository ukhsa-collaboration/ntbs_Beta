using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ntbs_service.Helpers
{
    public class NotifyError
    {
        public string Url { get; set; }
        public List<string> ErrorMessages { get; set; }
    }

    public static class NotificationValidationErrorGenerator
    {
        private static Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }

        // This method converts Model State Errors to Dictionary of Errors to be used in view
        public static Dictionary<string, NotifyError> MapToDictionary(ModelStateDictionary modelState, int notificationId)
        {
            NotifyErrorDictionary = new Dictionary<string, NotifyError>();

            foreach (var key in modelState.Keys)
            {
                // Splitting on '[' as well due to List properties having index, ex. NotificationSites[0]
                var propertyKey = key.Split(new Char[] { '.', '[' })[0];

                string displayName;
                string subPath;
                switch (propertyKey)
                {
                    case "PatientDetails":
                        subPath = NotificationSubPaths.EditPatientDetails;
                        displayName = "Patient Details";
                        break;
                    // NotificationSites is part of Clinical Details page despite being property of Notification
                    // Fall-through is intentional
                    case "NotificationSites":
                    case "ClinicalDetails":
                        subPath = NotificationSubPaths.EditClinicalDetails;
                        displayName = "Clinical Details";
                        break;
                    case "TestData":
                        subPath = NotificationSubPaths.EditTestResults;
                        displayName = "Test Results";
                        break;
                    case "NotificationDate":
                    case "Episode":
                        subPath = NotificationSubPaths.EditEpisode;
                        displayName = "Hospital Details";
                        break;
                    case "PatientTBHistory":
                        subPath = NotificationSubPaths.EditPreviousHistory;
                        displayName = "Previous History";
                        break;
                    case "ImmunosuppressionDetails":
                        subPath = NotificationSubPaths.EditImmunosuppression;
                        displayName = "Immunosuppression Details";
                        break;
                    // Travel Details and Visitor Details are editable on the Travel/visitor history screen
                    // Fall-through is intentional
                    case "TravelDetails":
                    case "VisitorDetails":
                        subPath = NotificationSubPaths.EditTravel;
                        displayName = "Travel and visitor history";
                        break;
                    default:
                        continue;
                }

                var url = RouteHelper.GetSubmittingNotificationPath(notificationId, subPath);
                AddErrorMessagesIntoDictionary(displayName, url,
                    modelState[key]?.Errors?.Select(e => e.ErrorMessage).ToList());
            }

            return NotifyErrorDictionary;
        }

        private static void AddErrorMessagesIntoDictionary(string displayName, string url, List<string> errorMessages)
        {
            if (errorMessages == null || errorMessages.Count == 0)
            {
                return;
            }

            if (!NotifyErrorDictionary.ContainsKey(displayName))
            {
                NotifyErrorDictionary.Add(displayName, new NotifyError
                {
                    Url = url,
                    ErrorMessages = errorMessages
                });
            }
            else
            {
                NotifyErrorDictionary[displayName].ErrorMessages.AddRange(errorMessages);
            }
        }
    }
}
