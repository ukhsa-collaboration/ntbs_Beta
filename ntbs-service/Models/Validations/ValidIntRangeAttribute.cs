using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty(ComparisonValue);
            if (property != null)
            {
                object propertyValue = property.GetValue(instance);
                // TryParse will set the value to 0 if it is given null to parse (this is to allow for input field being empty)
                int.TryParse(propertyValue?.ToString(), out int maxValue);
                int.TryParse(value?.ToString(), out int valueToValidate);
                if(valueToValidate >= 0 && valueToValidate <= maxValue) {
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
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            PropertyInfo propertyMax = type.GetProperty(MaxComparisonValue);
            PropertyInfo propertyToSum = type.GetProperty(SummedComparisonValue);
            if (propertyMax != null && propertyToSum != null)
            {
                object propertyValueMax = propertyMax.GetValue(instance);
                object propertyValueToSum = propertyToSum.GetValue(instance);

                // TryParse will set the value to 0 if it is given null to parse (this is to allow for input field being empty)
                int.TryParse(propertyValueMax?.ToString(), out int maxValue);
                int.TryParse(propertyValueToSum?.ToString(), out int valueToSum);
                int.TryParse(value?.ToString(), out int valueToValidate);

                if(valueToValidate >= 0 && valueToValidate <= maxValue - valueToSum) {
                    return null;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}