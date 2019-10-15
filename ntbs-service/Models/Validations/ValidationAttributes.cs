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

    public class ValidPartialDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is PartialDate partialDate) || partialDate.IsEmpty())
            {
                return null;
            }
            if (!string.IsNullOrEmpty(partialDate.Day)) {
                if(string.IsNullOrEmpty(partialDate.Month) || string.IsNullOrEmpty(partialDate.Year)) {
                    return new ValidationResult(ValidationMessages.YearIfMonthRequired);
                }
            }
            if(!string.IsNullOrEmpty(partialDate.Month)) {
                if(string.IsNullOrEmpty(partialDate.Year)) {
                    return new ValidationResult(ValidationMessages.YearRequired);
                }
            }
            bool canConvert = partialDate.TryConvertToDateTimeRange(out DateTime? x, out DateTime? y);
            if (!canConvert) {
                return new ValidationResult(ErrorMessage);
            }
            return null;
        }
    }

    public class ValidFormattedDateCanConvertToDatetimeAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is FormattedDate formattedDate) || formattedDate.IsEmpty())
            {
                return null;
            }
            bool canConvert = formattedDate.TryConvertToDateTime(out DateTime? x);
            if (!canConvert) {
                return new ValidationResult(ErrorMessage);
            }
            return null;
        }
    }
}