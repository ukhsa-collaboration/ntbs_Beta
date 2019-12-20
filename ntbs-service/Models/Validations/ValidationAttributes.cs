using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ntbs_service.Models.Validations
{
    public class ValidDateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime StartDate;
        public ValidDateRangeAttribute(string startDate) : base()
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

    public class ValidFormattedDateCanConvertToDatetimeAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is FormattedDate formattedDate) || formattedDate.IsEmpty())
            {
                return null;
            }

            bool canConvert = formattedDate.TryConvertToDateTime(out _);
            if (!canConvert)
            {
                return new ValidationResult(ValidationMessages.InvalidDate(validationContext.DisplayName));
            }
            return null;
        }
    }

    public class ValidNhsNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var nhsNumber = value.ToString();
            var match = Regex.Match(nhsNumber, "(\\d+)");
            if(!match.Success)
            {
                return new ValidationResult(ValidationMessages.NumberFormat);
            }
            else if(nhsNumber.Length != 10)
            {
                return new ValidationResult(ValidationMessages.NhsNumberLength);
            }
            
            var firstDigit = nhsNumber.Substring(0, 1);
            var  scottishAndTestDigits = new List<string> {"0", "1", "2", "9"};
            if (scottishAndTestDigits.Contains(firstDigit))
            {
                return null;
            }

            if(!ValidateNhsNumber(nhsNumber))
            {
                return new ValidationResult("invalid numero m8");
            }

            return null;
        }

        public bool ValidateNhsNumber(string nhsNumber)
        {
            return false;
        }
    }
}
