using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ntbs_service.Pages
{
    public abstract class ValidationModel : PageModel
    {
        public ContentResult OnPostValidateProperty(object model, string key, string value)
        {
            model.GetType().GetProperty(key).SetValue(model, value);
            return GetValidationResult(model, key);
        }

        public ContentResult OnPostValidateDate(object model, string key, string day, string month, string year)
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
                return Content("");
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

        public ContentResult ValidateContactTracing(object model, string key, string adultsIdentified, string childrenIdentified, string adultsScreened, string childrenScreened,
                            string adultsActiveTB, string childrenActiveTB, string adultsLatentTB, string childrenLatentTB, string adultsStartedTreatment, 
                            string childrenStartedTreatment, string adultsFinishedTreatment, string childrenFinishedTreatment) 
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
                        //comment here TODO
                        if(!modelStateKey.StartsWith("ContactTracing")) {
                            keyErrorDictionary.Add(modelStateKey, error.ErrorMessage);
                        }
                    }
                }
                return Content(JsonConvert.SerializeObject(keyErrorDictionary));
            }
        }
    }
}