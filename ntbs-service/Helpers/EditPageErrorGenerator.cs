using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ntbs_service.Helpers
{

    public static class EditPageValidationErrorGenerator
    {
        private static Dictionary<string, string> NotifyErrorDictionary { get; set; }

        // This method converts Model State Errors to Dictionary of Errors 
        // to be used in _SinglePageErrorSummaryPartial
        public static Dictionary<string, string> MapToDictionary(ModelStateDictionary modelState)
        {
            NotifyErrorDictionary = new Dictionary<string, string>();

            foreach (var key in modelState.Keys)
            {
                var errorMessageList = modelState[key]?.Errors?.Select(e => e.ErrorMessage).ToList();
                    foreach(var errorMessage in errorMessageList)
                    {
                        AddErrorMessagesIntoDictionary(key, errorMessage);
                    }
            }

            return NotifyErrorDictionary;
        }

        private static void AddErrorMessagesIntoDictionary(string property, string errorMessage)
        {
            if (errorMessage == null)
            {
                return;
            }
            var hyphenatedProperty = property.Replace(".", "-");

            if (!NotifyErrorDictionary.ContainsKey(hyphenatedProperty))
            {
                NotifyErrorDictionary.Add(hyphenatedProperty, errorMessage);
            }
            else
            {
                return;
            }
        }
    }
}
