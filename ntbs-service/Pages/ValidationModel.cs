using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using System.Collections.Generic;
using Newtonsoft.Json;
using EFAuditer;
using System.Threading.Tasks;

namespace ntbs_service.Pages
{
    public abstract class ValidationModel : PageModel
    {
        protected async Task OnGetAuditAsync(int notificationId, object model)
        {
            await Auditer.AuditReadOperation("NotificationId", notificationId, model);
        }

        protected ContentResult ValidateProperty(object model, string key, object value)
        {
            model.GetType().GetProperty(key).SetValue(model, value);
            return GetValidationResult(model, key);
        }

        protected ContentResult ValidateDate(object model, string key, string day, string month, string year)
        {
            DateTime? convertedDob;
            var formattedDate = new FormattedDate() { Day = day, Month = month, Year = year };
            if (formattedDate.TryConvertToDateTime(out convertedDob)) {
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
            if (TryValidateModel(model))
            {
                return ValidContent();
            }
            else
            {
                return Content(ModelState[key].Errors[0].ErrorMessage);
            }
        }

        private ContentResult ValidContent()
        {
            return Content("");
        }

        public bool IsValid(string key)
        {
            return ModelState[key] == null ? true : ModelState[key].Errors.Count == 0;
        }

        protected void SetAndValidateDateOnModel(object model, string key, FormattedDate formattedDate)
        {
            if (formattedDate.IsEmpty()) {
                return;
            }

            DateTime? convertedDob;
            string modelTypeName = model.GetType().Name;

            if (formattedDate.TryConvertToDateTime(out convertedDob)) {
                model.GetType().GetProperty(key).SetValue(model, convertedDob);
                TryValidateModel(model, modelTypeName);
            }
            else
            {
                ModelState.AddModelError($"{modelTypeName}.{key}", ValidationMessages.ValidDate);
                return;
            }
        }

        public ContentResult ValidateFullModel(object model, string key, string modelName) 
        {
            if(TryValidateModel(model, model.GetType().Name)) {
                return Content("");
            } else {
                Dictionary<string, string> keyErrorDictionary = new Dictionary<string, string>();
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        // Currently this double counts each property as "property" and "model.property", the below if clause 
                        // removes the instances of "model.property"
                        if(!modelStateKey.StartsWith(modelName)) {
                            keyErrorDictionary.Add(modelStateKey, error.ErrorMessage);
                        }
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
                    return;
                }
            } 
            else
            {
                ModelState.AddModelError($"{modelTypeName}.{key}", ValidationMessages.ValidYear);
                return;
            }
        }
    }
}