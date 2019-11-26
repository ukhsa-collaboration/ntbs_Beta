using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Services
{
    public class ValidationService
    {
        private readonly PageModel pageModel;

        public ValidationService(PageModel pageModel)
        {
            this.pageModel = pageModel;
        }

        public ContentResult ValidateModelProperty<T>(string key, object value, bool shouldValidateFull) where T : ModelBase
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            model.ShouldValidateFull = shouldValidateFull;
            return ValidateProperty(model, key, value);
        }

        public ContentResult ValidateProperty(object model, string key, object value)
        {
            SetProperty(model, key, value);
            return GetValidationResult(model, key);
        }

        public ContentResult ValidateMultipleProperties<T>(IEnumerable<Tuple<string, object>> propertyValueTuples, bool shouldValidateFull = false) where T : ModelBase
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            model.ShouldValidateFull = shouldValidateFull;

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

        public ContentResult ValidateDate<T>(string key, string day, string month, string year)
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            return ValidateDate<T>(model, key, day, month, year);
        }

        public ContentResult ValidateDate<T>(T model, string key, string day, string month, string year)
        {
            var formattedDate = new FormattedDate() { Day = day, Month = month, Year = year };
            if (formattedDate.TryConvertToDateTime(out DateTime? convertedDob))
            {
                model.GetType().GetProperty(key).SetValue(model, convertedDob);
                return GetValidationResult(model, key);
            }
            else
            {
                var x = model.GetType().GetProperty(key).GetCustomAttribute<DisplayAttribute>()?.Name;
                return pageModel.Content(ValidationMessages.InvalidDate(x));
            }
        }

        private ContentResult GetValidationResult(object model, string key)
        {
            pageModel.TryValidateModel(model);

            var modelStateByKey = ModelState()[key];

            if (modelStateByKey?.ValidationState == ModelValidationState.Invalid)
            {
                return pageModel.Content(modelStateByKey.Errors[0].ErrorMessage);
            }
            return ValidContent();
        }

        private ContentResult GetValidationResult(object model, IEnumerable<string> keys)
        {
            if (!pageModel.TryValidateModel(model))
            {
                var errorMessageMap = new Dictionary<int, string>();
                var errorIndex = 0;

                foreach (var key in keys)
                {
                    var modelStateByKey = ModelState()[key];
                    if (modelStateByKey?.ValidationState == ModelValidationState.Invalid)
                    {
                        errorMessageMap.Add(errorIndex, modelStateByKey.Errors[0].ErrorMessage);
                        errorIndex++;
                    }
                }
                if (errorIndex > 0)
                {
                    return pageModel.Content(JsonConvert.SerializeObject(errorMessageMap), "application/json");
                }
            }

            return ValidContent();
        }

        private ContentResult ValidContent()
        {
            return pageModel.Content("");
        }

        public bool IsValid(string key)
        {
            if (ModelState()[key] == null)
            {
                return true;
            }
            return ModelState()[key].Errors.Count == 0;
        }

        public void TrySetAndValidateDateOnModel(object model, string key, FormattedDate formattedDate)
        {
            string modelTypeName = model.GetType().Name;

            if (formattedDate.TryConvertToDateTime(out DateTime? convertedDob))
            {
                model.GetType().GetProperty(key).SetValue(model, convertedDob);
                pageModel.TryValidateModel(model, modelTypeName);
            }
            else
            {
                var x = model.GetType().GetProperty(key).GetCustomAttribute<DisplayAttribute>()?.Name;
                ModelState().AddModelError($"{modelTypeName}.{key}", ValidationMessages.InvalidDate(x));
                return;
            }
        }

        public ContentResult ValidateFullModel(object model)
        {
            if (pageModel.TryValidateModel(model, model.GetType().Name))
            {
                return ValidContent();
            }
            else
            {
                Dictionary<string, string> keyErrorDictionary = new Dictionary<string, string>();
                var modelState = pageModel.ViewData.ModelState;
                foreach (var modelStateKey in modelState.Keys)
                {
                    var modelStateVal = modelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        // Only add the first error per key to the dictionary.
                        keyErrorDictionary.TryAdd(modelStateKey, error.ErrorMessage);
                    }
                }
                return pageModel.Content(JsonConvert.SerializeObject(keyErrorDictionary));
            }
        }

        public ContentResult ValidateYearComparison(int yearToValidate, int yearToCompare)
        {
            if (!IsValidYear(yearToValidate))
            {
                return pageModel.Content(ValidationMessages.InvalidYear);
            }

            if (yearToValidate < yearToCompare)
            {
                return pageModel.Content(ValidationMessages.ValidYearLaterThanBirthYear(yearToCompare));
            }
            else
            {
                return ValidContent();
            }
        }

        // We could do this validation using a custom data annotation, but as we already need to do another comparison
        // it is simpler to do it here
        private bool IsValidYear(int year)
        {
            return year >= ValidDates.EarliestYear && year <= DateTime.Now.Year;
        }

        public void ValidateYearComparisonOnModel(object model, string key, int yearToValidate, int? yearToCompare)
        {
            string modelTypeName = model.GetType().Name;

            if (!IsValidYear(yearToValidate))
            {
                ModelState().AddModelError($"{modelTypeName}.{key}", ValidationMessages.InvalidYear);
                return;
            }

            if (yearToCompare != null && yearToValidate < (int)yearToCompare)
            {
                ModelState().AddModelError($"{modelTypeName}.{key}", ValidationMessages.ValidYearLaterThanBirthYear((int)yearToCompare));
            }
        }

        private ModelStateDictionary ModelState()
        {
            return pageModel.ModelState;
        }
    }
}