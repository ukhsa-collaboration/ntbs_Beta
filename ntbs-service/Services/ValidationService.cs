using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Services
{
    public class ValidationService
    {
        private readonly PageModel _pageModel;

        private ModelStateDictionary ModelState => _pageModel.ModelState;

        public ValidationService(PageModel pageModel)
        {
            _pageModel = pageModel;
        }

        public ContentResult GetPropertyValidationResult<T>(string key, object value, bool shouldValidateFull) where T : ModelBase
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            model.ShouldValidateFull = shouldValidateFull;
            return GetPropertyValidationResult(model, key, value);
        }

        public ContentResult GetPropertyValidationResult(object model, string key, object value)
        {
            SetProperty(model, key, value);
            return GetValidationResult(model, key);
        }

        public ContentResult GetMultiplePropertiesValidationResult<T>(
            IEnumerable<(string, object)> propertyValueTuples,
            bool shouldValidateFull = false,
            bool isLegacy = false)
            where T : ModelBase
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            model.ShouldValidateFull = shouldValidateFull;
            model.IsLegacy = isLegacy;

            var keys = new List<string>();
            foreach (var tuple in propertyValueTuples)
            {
                SetProperty(model, tuple.Item1, tuple.Item2);
                keys.Add(tuple.Item1);
            }
            return GetValidationResult(model, keys);
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

                 Any type discrepancies that still exist will cause `SetValue` to throw errors anyways.
                */

            }
            property.SetValue(model, value);
        }

        public ContentResult GetDateValidationResult<T>(string key, string day, string month, string year)
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            return GetDateValidationResult<T>(model, key, day, month, year);
        }

        public ContentResult GetDateValidationResult<T>(T model, string key, string day, string month, string year)
        {
            var formattedDate = new FormattedDate() { Day = day, Month = month, Year = year };
            var modelType = model.GetType();
            if (formattedDate.TryConvertToDateTime(out DateTime? convertedDob))
            {
                modelType.GetProperty(key).SetValue(model, convertedDob);
                return GetValidationResult(model, key);
            }
            else
            {
                var propertyDisplayName = modelType.GetProperty(key).GetCustomAttribute<DisplayAttribute>()?.Name;
                return _pageModel.Content(ValidationMessages.InvalidDate(propertyDisplayName));
            }
        }

        private ContentResult GetValidationResult(object model, string key)
        {
            _pageModel.TryValidateModel(model);

            var modelStateByKey = ModelState[key];

            if (modelStateByKey?.ValidationState == ModelValidationState.Invalid)
            {
                return _pageModel.Content(modelStateByKey.Errors[0].ErrorMessage);
            }
            return ValidContent();
        }

        private ContentResult GetValidationResult(object model, IEnumerable<string> keys)
        {
            if (!_pageModel.TryValidateModel(model))
            {
                var errorMessageMap = new Dictionary<int, string>();
                var errorIndex = 0;

                foreach (var key in keys)
                {
                    var modelStateByKey = ModelState[key];
                    if (modelStateByKey?.ValidationState == ModelValidationState.Invalid)
                    {
                        errorMessageMap.Add(errorIndex, modelStateByKey.Errors[0].ErrorMessage);
                    }
                    errorIndex++;
                }
                if (errorMessageMap.Count > 0)
                {
                    return _pageModel.Content(JsonConvert.SerializeObject(errorMessageMap), "application/json");
                }
            }

            return ValidContent();
        }

        public ContentResult ValidContent()
        {
            return _pageModel.Content("");
        }

        public bool IsValid(string key)
        {
            if (ModelState[key] == null)
            {
                return true;
            }
            return ModelState[key].Errors.Count == 0;
        }

        /// <param name="model"> The model on which date gets set </param>
        /// <param name="modelKey"> Prefix for model state errors </param>
        /// <param name="key"> Date key for model state errors </param>
        /// <param name="formattedDate"> The FormattedDate to covert and set </param>
        public void TrySetFormattedDate(object model, string modelKey, string key, FormattedDate formattedDate)
        {
            var modelType = model.GetType();
            if (formattedDate.TryConvertToDateTime(out DateTime? convertedDob))
            {
                modelType.GetProperty(key)?.SetValue(model, convertedDob);
            }
            else
            {
                var propertyDisplayName = modelType.GetProperty(key).GetCustomAttribute<DisplayAttribute>()?.Name;
                ModelState.AddModelError($"{modelKey}.{key}", ValidationMessages.InvalidDate(propertyDisplayName));
            }
        }

        public ContentResult GetFullModelValidationResult(object model)
        {
            if (_pageModel.TryValidateModel(model, model.GetType().Name))
            {
                return ValidContent();
            }
            else
            {
                Dictionary<string, string> keyErrorDictionary = new Dictionary<string, string>();
                var modelState = _pageModel.ViewData.ModelState;
                foreach (var modelStateKey in modelState.Keys)
                {
                    var modelStateVal = modelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        // Only add the first error per key to the dictionary.
                        keyErrorDictionary.TryAdd(modelStateKey, error.ErrorMessage);
                    }
                }
                return _pageModel.Content(JsonConvert.SerializeObject(keyErrorDictionary));
            }
        }

        public ContentResult GetYearComparisonValidationResult(int yearToValidate, int yearToCompare, string propertyName)
        {
            if (!IsValidYear(yearToValidate))
            {
                return _pageModel.Content(ValidationMessages.InvalidYear(propertyName));
            }

            if (yearToValidate < yearToCompare)
            {
                return _pageModel.Content(ValidationMessages.ValidYearLaterThanBirthYear(propertyName, yearToCompare));
            }
            else
            {
                return ValidContent();
            }
        }

        public void ValidateProperty(object model, string modelKey, object modelProperty, string propertyKey)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            validationContext.MemberName = propertyKey;

            Validator.TryValidateProperty(modelProperty, validationContext, validationResults);

            if (validationResults.Any())
            {
                var errorKey = $"{modelKey}.{propertyKey}";
                ModelState.AddModelError(errorKey, validationResults[0].ErrorMessage);
            }
        }

        public void ValidateYearComparison(object model, string key, int yearToValidate, int? yearToCompare)
        {
            var modelType = model.GetType();
            var modelTypeName = modelType.Name;

            if (!IsValidYear(yearToValidate))
            {
                var propertyDisplayName = modelType.GetProperty(key).GetCustomAttribute<DisplayAttribute>()?.Name;
                ModelState.AddModelError($"{modelTypeName}.{key}", ValidationMessages.InvalidYear(propertyDisplayName));
                return;
            }

            if (yearToCompare != null && yearToValidate < (int)yearToCompare)
            {
                var propertyDisplayName = modelType.GetProperty(key).GetCustomAttribute<DisplayAttribute>()?.Name;
                ModelState.AddModelError($"{modelTypeName}.{key}", ValidationMessages.ValidYearLaterThanBirthYear(propertyDisplayName, (int)yearToCompare));
            }
        }

        private bool IsValidYear(int year)
        {
            return year >= ValidDates.EarliestYear && year <= DateTime.Now.Year;
        }
    }
}
