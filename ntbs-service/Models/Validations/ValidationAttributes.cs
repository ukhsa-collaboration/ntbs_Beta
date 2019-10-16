using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ntbs_service.Models;
using ntbs_service.Models.Validations;

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

    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOnePropertyAttribute : ValidationAttribute
    {
        private string[] PropertyList { get; set; }

        public AtLeastOnePropertyAttribute(params string[] propertyList)
        {
            this.PropertyList  = propertyList;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) 
        {
            foreach (var propertyName in PropertyList)
            {
                var propertyInfo = value.GetType().GetProperty(propertyName);
                var property = propertyInfo.GetValue(value, null);
                Type partialDateType = typeof(PartialDate);
                if(property != null && property.GetType() == partialDateType) {
                    MethodInfo partialDateIsEmptyMethod = partialDateType.GetMethod("IsEmpty");
                    var isEmpty = partialDateIsEmptyMethod.Invoke(property, new object[] {});
                    if(isEmpty.Equals(false)) {
                        return null;
                    }
                }
                else if (property != null)
                {
                    return null;
                }
            }
            return new ValidationResult(ErrorMessage);
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
