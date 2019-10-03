using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ntbs_service.Helpers
{
    public static class ValidationExtensions
    {
        public static ContentResult ValidateProperty(this PageModel pageModel, object model, string key, object value)
        {
            SetProperty(model, key, value);
            return GetValidationResult(pageModel, model, key);
        }

        private static void SetProperty(object model, string key, object value)
        {
            if (value == null)
            {
                return;
            }
            var property = model.GetType().GetProperty(key);

            var converter = TypeDescriptor.GetConverter(property.PropertyType);
            try
            {
                /*
                 This will convert strings to boolean and numeric types if appropriate ...
                */
                value = converter.ConvertFrom(value);
            }
            catch (NotSupportedException)
            {
                /*
                 ... but it will throw an error for complex object types (e.g.: List<T>)
                 If that's the case, then we're safe to ignore that error, as `value` is already of the correct type.

                 Any type discrepencies that still exist will cause `SetValue` to throw errors anyways.
                */

            }
            property.SetValue(model, value);
        }

        private static ContentResult GetValidationResult(PageModel pageModel, object model, string key)
        {
            pageModel.TryValidateModel(model);

            return pageModel.ModelState[key] == null ? pageModel.Content("") : pageModel.Content(pageModel.ModelState[key].Errors[0].ErrorMessage);
        }
    }
}