using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models;
using ntbs_service.Models.Validations;

namespace ntbs_service.Pages
{
    public abstract class ValidationModel : PageModel
    {
        protected ContentResult ValidateProperty(object model, string key, string value)
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

        protected ContentResult ValidateYearComparison(string yearToValidate, int yearToCompare)
        {
            if (IsValidYear(yearToValidate)) 
            {
                if (int.Parse(yearToValidate) < yearToCompare)
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

        private bool IsValidYear(string year)
        {
            if (int.TryParse(year, out int parsedYear))
            {
                return parsedYear >= ValidDates.EarliestYear && parsedYear <= DateTime.Now.Year;
            }
            else 
            {
                return false;
            }
        }

        protected void ValidateYearComparisonOnModel(object model, string key, string yearToValidate, int? yearToCompare)
        {
            if (string.IsNullOrEmpty(yearToValidate) || yearToCompare == null) 
            {
                return;
            }

            string modelTypeName = model.GetType().Name;

            if (IsValidYear(yearToValidate)) 
            {
                if (int.Parse(yearToValidate) < (int)yearToCompare)
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