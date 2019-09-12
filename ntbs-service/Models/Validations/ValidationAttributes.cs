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
}