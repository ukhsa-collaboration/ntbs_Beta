using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ntbs_service.Helpers
{

    public static class EditPageValidationErrorGenerator
    {
        private static Dictionary<string, string> NotifyErrorDictionary { get; set; }

        // This method converts Model State Errors to Dictionary of Errors to be used in view
        public static Dictionary<string, string> MapToDictionary(ModelStateDictionary modelState)
        {
            NotifyErrorDictionary = new Dictionary<string, string>();

            foreach (var key in modelState.Keys)
            {
                var x = modelState[key]?.Errors?.Select(e => e.ErrorMessage).ToList();
                    foreach(var y in x)
                    {
                        AddErrorMessagesIntoDictionary(key, y);
                    }
            }

            return NotifyErrorDictionary;
        }

        private static void AddErrorMessagesIntoDictionary(string displayName, string url)
        {
            if (url == null)
            {
                return;
            }
            var splitDisplayName = displayName.Split(".");
            var item = splitDisplayName.Last();

            if (!NotifyErrorDictionary.ContainsKey(item))
            {
                NotifyErrorDictionary.Add(item, url);
            }
            else
            {
                return;
            }
        }

        public static string GenerateLinkFromKey(string key)
        {
            return $"#{key}-form-group";
        }
    }
}
