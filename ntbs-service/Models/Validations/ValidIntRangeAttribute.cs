using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Validations
{
    public class PositiveIntegerSmallerThanValue : ValidationAttribute
    {

        public string ComparisonValue { get; set; }
        public PositiveIntegerSmallerThanValue(string comparisonValue)
        {
            ComparisonValue = comparisonValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var property = type.GetProperty(ComparisonValue);
            if (property != null)
            {
                var propertyValue = property.GetValue(instance);
                // TryParse will set the value to 0 if it is given null to parse (this is to allow for input field being empty)
                int.TryParse(propertyValue?.ToString(), out var maxValue);
                int.TryParse(value?.ToString(), out var valueToValidate);
                if (valueToValidate >= 0 && valueToValidate <= maxValue)
                {
                    return null;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }

    public class PositiveIntegerSmallerThanDifferenceOfValues : ValidationAttribute
    {
        public string MaxComparisonValue { get; set; }
        public string SummedComparisonValue { get; set; }
        public PositiveIntegerSmallerThanDifferenceOfValues(string maxComparisonValue, string summedComparisonValue)
        {
            MaxComparisonValue = maxComparisonValue;
            SummedComparisonValue = summedComparisonValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var propertyMax = type.GetProperty(MaxComparisonValue);
            var propertyToSum = type.GetProperty(SummedComparisonValue);
            if (propertyMax != null && propertyToSum != null)
            {
                var propertyValueMax = propertyMax.GetValue(instance);
                var propertyValueToSum = propertyToSum.GetValue(instance);

                // TryParse will set the value to 0 if it is given null to parse (this is to allow for input field being empty)
                int.TryParse(propertyValueMax?.ToString(), out var maxValue);
                int.TryParse(propertyValueToSum?.ToString(), out var valueToSum);
                int.TryParse(value?.ToString(), out var valueToValidate);

                if (valueToValidate >= 0 && valueToValidate <= maxValue - valueToSum)
                {
                    return null;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}