using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoreLinq;
using ntbs_service.Models.Entities;
using ntbs_service.Pages.Notifications;

namespace ntbs_service.Helpers
{
    public static class NotificationValidationErrorGenerator
    {
        private static Dictionary<NotificationSection, List<string>> NotifyErrorDictionary { get; set; }

        // This method converts Model State Errors to Dictionary of Errors to be used in view
        public static Dictionary<NotificationSection, List<string>> MapToDictionary(ModelStateDictionary modelState)
        {
            NotifyErrorDictionary = new Dictionary<NotificationSection, List<string>>();

            modelState.Keys
                .Where(key => modelState[key].ValidationState == ModelValidationState.Invalid)
                .ForEach(key =>
                {
                    // Splitting on '[' as well due to List properties having index, ex. NotificationSites[0]
                    var modelName = key.Split('.', '[')[0];

                    Type model;
                    switch (modelName)
                    {
                        // First accommodate for some edge-cases based on properties and not models...
                        case "NotificationSites":
                            model = typeof(ClinicalDetails);
                            break;
                        case "NotificationDate":
                            model = typeof(HospitalDetails);
                            break;
                        case "ImmunosuppressionDetails":
                            model = typeof(ComorbidityDetails);
                            break;
                        case "HasDeathEventForPostMortemCase":
                            model = typeof(TreatmentEvent);
                            break;
                        default:
                            // ... and figure out the rest based on reflection
                            model = Type.GetType($"ntbs_service.Models.Entities.{modelName}", true);
                            break;
                    }

                    AddErrorMessagesIntoDictionary(
                        NotificationSectionFactory.FromModel(model),
                        modelState[key]?.Errors?.Select(e => e.ErrorMessage).ToList());
                });

            return NotifyErrorDictionary;
        }

        private static void AddErrorMessagesIntoDictionary(NotificationSection displayName, List<string> errorMessages)
        {
            if (errorMessages == null || errorMessages.Count == 0)
            {
                return;
            }

            if (!NotifyErrorDictionary.ContainsKey(displayName))
            {
                NotifyErrorDictionary.Add(displayName, errorMessages);
            }
            else
            {
                NotifyErrorDictionary[displayName].AddRange(errorMessages);
            }
        }
    }
}
