using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Validations
{
    public class ValidDateAttribute : RangeAttribute
    {
        private string StartDate;
        public ValidDateAttribute(string startDateString) : base(typeof(DateTime),
            startDateString, DateTime.Now.ToShortDateString())
        {
            StartDate = startDateString;
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationMessages.DateValidityRange(StartDate);
        }
    }

    public class OnlyOneTrue : ValidationAttribute
    {

        public string ComparisonValue { get; set; }
        public OnlyOneTrue(string comparisonValue)
        {
            ComparisonValue = comparisonValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            var propertyToCompare = instance.GetType().GetProperty(ComparisonValue);
            object valueToCompare = propertyToCompare.GetValue(instance);

            if (isTruthy(valueToCompare) && isTruthy(value)) {
                return new ValidationResult(ErrorMessage);
            }
            return null;
        }

        private bool isTruthy(object value)
        {
            return value != null && (bool)value;
        }
    }

    public class ValidPartialDateCanConvertToDatetimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PartialDate valueCopy = (PartialDate) value;
            if(valueCopy==null || (valueCopy.Year == null && valueCopy.Day == null && valueCopy.Month == null)) {
                return null;
            }
            if (!string.IsNullOrEmpty(valueCopy.Day)) {
                if(string.IsNullOrEmpty(valueCopy.Month) || string.IsNullOrEmpty(valueCopy.Year)) {
                    return new ValidationResult("Year and month must be provided if a day has been provided");
                }
            }
            if(!string.IsNullOrEmpty(valueCopy.Month)) {
                if(string.IsNullOrEmpty(valueCopy.Year)) {
                    return new ValidationResult("A year must be provided");
                }
            }
            bool canConvert = valueCopy.TryConvertToDateTimeRange(out DateTime? x, out DateTime? y);
            if (!canConvert) {
                return new ValidationResult("Invalid date selection");
            }
            return null;
        }
    }

    public class ValidFormattedDateCanConvertToDatetimeAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            FormattedDate valueCopy = (FormattedDate) value;
            if(valueCopy==null) {
                return null;
            }
            bool canConvert = valueCopy.TryConvertToDateTime(out DateTime? x);
            if (!canConvert) {
                return new ValidationResult("Invalid date selection");
            }
            return null;
        }
    }
}