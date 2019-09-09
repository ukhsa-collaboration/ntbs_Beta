using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models;
using ntbs_service.Models.Validations;

namespace ntbs_service.Pages
{
    public class ValidationModel : PageModel
    {
        protected ContentResult OnPostValidateProperty(object model, string key, string value)
        {
            model.GetType().GetProperty(key).SetValue(model, value);
            return GetValidationResult(model, key);
        }

        protected ContentResult OnPostValidateDate(object model, string key, string day, string month, string year)
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

        protected ContentResult GetValidationResult(object model, string key)
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

        public bool IsValid(string key)
        {
            return ModelState[key] == null ? true : ModelState[key].Errors.Count == 0;
        }

        public ContentResult OnPostValidatePatientProperty(string key, string value)
        {
            return OnPostValidateProperty(new PatientDetails(), key, value);
        }

        public ContentResult OnPostValidatePatientDate(string key, string day, string month, string year)
        {
            return OnPostValidateDate(new PatientDetails(), key, day, month, year);
        }

        public ContentResult OnPostValidateClinicalDetailsProperty(string key, string value)
        {
            return OnPostValidateProperty(new ClinicalDetails(), key, value);
        }
        public ContentResult OnPostValidateClinicalDetailsDate(string key, string day, string month, string year)
        {
            return OnPostValidateDate(new ClinicalDetails(), key, day, month, year);
        }

        public ContentResult OnPostValidateNotificationSiteProperty(string key, string value)
        {
            return OnPostValidateProperty(new NotificationSite(), key, value);
        }

        public ContentResult OnPostValidateYearComparison(string newYear, int existingYear)
        {
            if (IsValidYear(newYear)) 
            {
                if (int.Parse(newYear) < existingYear)
                {
                    return Content(ValidationMessages.ValidYearLaterThanBirthYear(existingYear));
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

        protected bool IsValidYear(string year)
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

        private ContentResult ValidContent()
        {
            return Content("");
        }

        public void SetAndValidateDate(object model, string key, FormattedDate formattedDate)
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

        public void ValidateYearAgainstOtherYear(object model, string key, string yearToValidate, int? yearToCompare)
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