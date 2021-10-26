using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Models.Entities;

namespace ntbs_service.Models.Validations
{
    public class ValidDateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _startDate;
        public ValidDateRangeAttribute(string startDate)
        {
            _startDate = DateTime.Parse(startDate);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var date = (DateTime)value;
                if (date < _startDate)
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
            return ValidationMessages.DateValidityRangeStart(name, _startDate.ToShortDateString());
        }
    }

    /// <summary>
    /// This attribute should only be used on DateTime properties on entity models inheriting from ModelBase
    /// 
    /// The minimum date is checked for the annotated date property only when it is on a non-legacy notification
    /// </summary>
    public class ValidClinicalDateAttribute : ValidationAttribute
    {
        private readonly DateTime _startDate = DateTime.Parse(ValidDates.EarliestClinicalDate);
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var date = (DateTime)value;
            var isLegacy = ((ModelBase)validationContext.ObjectInstance).IsLegacy;
            if (date < _startDate && isLegacy == false)
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
            return ValidationMessages.DateValidityRangeStart(name, _startDate.ToShortDateString());
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOnePropertyAttribute : ValidationAttribute
    {
        private string[] PropertyList { get; }

        public AtLeastOnePropertyAttribute(params string[] propertyList)
        {
            PropertyList = propertyList;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueFound = PropertyList
                .Select(propertyName => value.GetType().GetProperty(propertyName))
                .Where(propertyInfo => propertyInfo != null)
                .Any(propertyInfo => propertyInfo.GetValue(value, null) != null);
            return valueFound
                ? null
                : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
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
                    return new ValidationResult(ValidationMessages.YearAndMonthRequired);
                }
            }
            if (!string.IsNullOrEmpty(partialDate.Month))
            {
                if (string.IsNullOrEmpty(partialDate.Year))
                {
                    return new ValidationResult(ValidationMessages.YearRequired);
                }
            }

            if (int.Parse(partialDate.Year) < 1900)
            {
                return new ValidationResult(ValidationMessages.YearAfter1900);
            }

            var canConvert = partialDate.TryConvertToDateTimeRange(out _, out _);
            if (!canConvert)
            {
                return new ValidationResult(ValidationMessages.InvalidDate(validationContext.DisplayName));
            }
            return null;
        }
    }

    public class ValidDurationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var duration = ((string)value).Trim();
            if (duration.Contains("+"))
            {
                var numbers = duration.Split("+").Where(x => x != "");
                if (numbers.Count() != 1 ||
                    !int.TryParse(numbers.Single(), out _))
                {
                    return new ValidationResult(ValidationMessages.MDRDuration);
                }
            }

            else if (duration.Contains("-"))
            {
                var numbers = duration.Split("-");
                if (numbers.Length != 2 ||
                    numbers.Any(num => !int.TryParse(num, out _)) ||
                    int.Parse(numbers[0]) > int.Parse(numbers[1]))
                {
                    return new ValidationResult(ValidationMessages.MDRDuration);
                }
            }

            else if (!int.TryParse(duration, out _))
            {
                return new ValidationResult(ValidationMessages.MDRDuration);
            }

            return null;
        }
    }
}
