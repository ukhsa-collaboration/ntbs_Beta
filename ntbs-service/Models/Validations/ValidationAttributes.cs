using System;
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
}