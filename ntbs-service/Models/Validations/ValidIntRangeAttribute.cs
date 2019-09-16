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
                int valueToValidate;
                int maxValue;
                object propertyValue = property.GetValue(instance);
                if(propertyValue == null) {
                    maxValue = 0;
                } else {
                    Int32.TryParse(propertyValue.ToString(), out maxValue);
                }
                if(value == null) {
                    valueToValidate = 0;
                } else {
                    Int32.TryParse(value.ToString(), out valueToValidate);
                }
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
                int valueToValidate;
                int valueToSum;
                int maxValue;
                object propertyValueMax = propertyMax.GetValue(instance);
                object propertyValueToSum = propertyToSum.GetValue(instance);

                if(propertyValueMax == null) {
                    maxValue = 0;
                } else {
                    Int32.TryParse(propertyValueMax.ToString(), out maxValue);
                }
                if(propertyValueToSum == null) {
                    valueToSum = 0;
                } else {
                    Int32.TryParse(propertyValueToSum.ToString(), out valueToSum);
                }
                if(value == null) {
                    valueToValidate = 0;
                } else {
                    Int32.TryParse(value.ToString(), out valueToValidate);
                }

                if(valueToValidate >= 0 && valueToValidate <= maxValue - valueToSum) {
                    return null;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}