using System;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Validations
{
    public class ValidDateAttribute : RangeAttribute
    {
        public static DateTime EarliestDate = new DateTime(1900, 1, 1);
        public ValidDateAttribute() : base(typeof(DateTime),
            new DateTime(1900, 1, 1).ToShortDateString(), DateTime.Now.ToShortDateString()) {}

         public override string FormatErrorMessage(string name)
         {
             return $"Must be between {EarliestDate.ToShortDateString()} and {DateTime.Now.ToShortDateString()}";
         }
    }
}