using System;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Entities;

namespace ntbs_service.Models.Validations
{
    public class ValidDateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime StartDate;
        public ValidDateRangeAttribute(string startDate)
        {
            StartDate = DateTime.Parse(startDate);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var date = (DateTime)value;
                if (date < StartDate)
                {
                    return new ValidationResult(ErrorMessage);
                }
                if (date > DateTime.Today)
                {
                    return new ValidationResult(ValidationMessages.TodayOrEarlier(validationContext.DisplayName));
                }
            }
            return null;
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationMessages.DateValidityRangeStart(name, StartDate.ToShortDateString());
        }
    }
    
    /// <summary>
    /// This attribute should only be used on DateTime properties on entity models inheriting from ModelBase
    /// 
    /// The minimum date is checked for the annotated date property only when it is on a non-legacy notification
    /// </summary>
    public class ValidClinicalDateAttribute : ValidationAttribute
    {
        private readonly DateTime StartDate = DateTime.Parse(ValidDates.EarliestClinicalDate);
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            
            var date = (DateTime)value;
            var isLegacy = ((ModelBase)validationContext.ObjectInstance).IsLegacy;
            if (date < StartDate && isLegacy == false)
            {
                return new ValidationResult(ErrorMessage);
            }
            if (date > DateTime.Today)
            {
                return new ValidationResult(ValidationMessages.TodayOrEarlier(validationContext.DisplayName));
            }
            return null;
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationMessages.DateValidityRangeStart(name, StartDate.ToShortDateString());
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

            if (IsTruthy(valueToCompare) && IsTruthy(value))
            {
                return new ValidationResult(ErrorMessage);
            }
            return null;
        }

        private bool IsTruthy(object value)
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
            this.PropertyList = propertyList;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var propertyName in PropertyList)
            {
                var propertyInfo = value.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
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
            if (!string.IsNullOrEmpty(partialDate.Day))
            {
                if (string.IsNullOrEmpty(partialDate.Month) || string.IsNullOrEmpty(partialDate.Year))
                {
                    return new ValidationResult(ValidationMessages.YearIfMonthRequired);
                }
            }
            if (!string.IsNullOrEmpty(partialDate.Month))
            {
                if (string.IsNullOrEmpty(partialDate.Year))
                {
                    return new ValidationResult(ValidationMessages.YearRequired);
                }
            }

            bool canConvert = partialDate.TryConvertToDateTimeRange(out _, out _);
            if (!canConvert)
            {
                return new ValidationResult(ValidationMessages.InvalidDate(validationContext.DisplayName));
            }
            return null;
        }
    }
}
