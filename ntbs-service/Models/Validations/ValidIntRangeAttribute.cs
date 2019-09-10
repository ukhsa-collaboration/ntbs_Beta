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
                int valueToValidate;
                int maxValue;
                if(Int32.TryParse(propertyValue.ToString(), out maxValue) && Int32.TryParse(value.ToString(), out valueToValidate)) {
                    if(valueToValidate >= 0 && valueToValidate <= maxValue) {
                    return null;
                    }
                }
            }
            return new ValidationResult("Invalid entry");
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
                int valueToValidate;
                int valueToSum;
                int maxValue;
                if(Int32.TryParse(propertyValueMax.ToString(), out maxValue) && Int32.TryParse(propertyValueToSum.ToString(), out valueToSum) && Int32.TryParse(value.ToString(), out valueToValidate)) {
                    if(valueToValidate >= 0 && valueToValidate <= maxValue - valueToSum) {
                    return null;
                    }
                }
            }
            return new ValidationResult("Invalid entry");
        }
    }
}