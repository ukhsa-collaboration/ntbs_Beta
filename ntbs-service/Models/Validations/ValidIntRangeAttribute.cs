using System;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Validations
{
     public class PositiveIntegerSmallerThanValue : ValidationAttribute
    {

        private readonly int maxValue;
        public PositiveIntegerSmallerThanValue(string maxValue)
        {
            Int32.TryParse(maxValue, out this.maxValue);
        }

        public override bool IsValid(object value)
        {
            int i;
            int.TryParse(value.ToString(), out i);
            return i >= 0 && i <= maxValue;
        }
    }

    public class PositiveIntegerSmallerThanDifferenceOfValues : ValidationAttribute
    {
        private readonly int maxValue;
        private readonly int smallerValue;
        public PositiveIntegerSmallerThanDifferenceOfValues(string maxValue, string smallerValue)
        {
            Int32.TryParse(maxValue, out this.maxValue);
            Int32.TryParse(smallerValue, out this.smallerValue);
        }

        public override bool IsValid(object value)
        {
            int i;
            int.TryParse(value.ToString(), out i);
            return i >= 0 && i <= (maxValue - smallerValue);
        }
    }
}