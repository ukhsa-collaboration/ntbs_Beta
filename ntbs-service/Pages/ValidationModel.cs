using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ntbs_service.Pages
{
    public abstract class ValidationModel : PageModel
    {
        protected ContentResult ValidateProperty(object model, string key, object value)
        {
            SetProperty(model, key, value);
            return GetValidationResult(model, key);
        }

        protected ContentResult ValidateMultipleProperties(object model, IEnumerable<Tuple<string, object>> propertyValueTuples)
        {
            var keys = new List<string>();
            foreach (var tuple in propertyValueTuples)
            {
                SetProperty(model, tuple.Item1, tuple.Item2);
                keys.Add(tuple.Item1);
            }
            return GetValidationResult(model, keys);
        }

        private void SetProperty(object model, string key, object value)
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

        protected ContentResult ValidateDate(object model, string key, string day, string month, string year)
        {
            var formattedDate = new FormattedDate() { Day = day, Month = month, Year = year };
            if (formattedDate.TryConvertToDateTime(out var convertedDob))
            {
                model.GetType().GetProperty(key).SetValue(model, convertedDob);
                return GetValidationResult(model, key);
            }
            else
            {
                return Content(ValidationMessages.ValidDate);
            }
        }

        private ContentResult GetValidationResult(object model, string key)
        {
            TryValidateModel(model);

            return ModelState[key] == null ? ValidContent() : Content(ModelState[key].Errors[0].ErrorMessage);
        }

        private ContentResult GetValidationResult(object model, IEnumerable<string> keys)
        {
            if (TryValidateModel(model))
            {
                return ValidContent();
            }
            else
            {
                var errorMessageMap = new Dictionary<int, string>();
                var errorIndex = 0;
                foreach (var key in keys)
                {
                    errorMessageMap.Add(errorIndex, ModelState[key].Errors[0].ErrorMessage);
                    errorIndex++;
                }
                return Content(JsonConvert.SerializeObject(errorMessageMap), "application/json");
            }
        }

        private ContentResult ValidContent()
        {
            return Content("");
        }

        public bool IsValid(string key)
        {
            if (ModelState[key] == null)
            {
                return true;
            }
            return ModelState[key].Errors.Count == 0;
        }

        protected void SetAndValidateDateOnModel(object model, string key, FormattedDate formattedDate)
        {
            if (formattedDate.IsEmpty())
            {
                return;
            }

            string modelTypeName = model.GetType().Name;

            if (formattedDate.TryConvertToDateTime(out var convertedDob))
            {
                model.GetType().GetProperty(key).SetValue(model, convertedDob);
                TryValidateModel(model, modelTypeName);
            }
            else
            {
                ModelState.AddModelError($"{modelTypeName}.{key}", ValidationMessages.ValidDate);
            }
        }

        public ContentResult ValidateFullModel(object model)
        {
            if (TryValidateModel(model, model.GetType().Name))
            {
                return Content("");
            }
            else
            {
                Dictionary<string, string> keyErrorDictionary = new Dictionary<string, string>();
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        keyErrorDictionary.Add(modelStateKey, error.ErrorMessage);
                    }
                }
                return Content(JsonConvert.SerializeObject(keyErrorDictionary));
            }
        }

        protected ContentResult ValidateYearComparison(int yearToValidate, int yearToCompare)
        {
            if (IsValidYear(yearToValidate))
            {
                if (yearToValidate < yearToCompare)
                {
                    return Content(ValidationMessages.ValidYearLaterThanBirthYear(yearToCompare));
                }
                else
                {
                    return ValidContent();
                }
            }
            else
            {
                return Content(ValidationMessages.ValidYear);
            }
        }

        // We could do this validation using a custom data annotation, but as we already need to do another comparison
        // it is simpler to do it here
        private bool IsValidYear(int year)
        {
            return year >= ValidDates.EarliestYear && year <= DateTime.Now.Year;
        }

        protected void ValidateYearComparisonOnModel(object model, string key, int? yearToValidate, int? yearToCompare)
        {
            if (yearToValidate == null || yearToCompare == null)
            {
                return;
            }

            string modelTypeName = model.GetType().Name;

            if (IsValidYear((int)yearToValidate))
            {
                if ((int)yearToValidate < (int)yearToCompare)
                {
                    ModelState.AddModelError($"{modelTypeName}.{key}", ValidationMessages.ValidYearLaterThanBirthYear((int)yearToCompare));
                }
            }
            else
            {
                ModelState.AddModelError($"{modelTypeName}.{key}", ValidationMessages.ValidYear);
            }
        }
    }
}