using System;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Validations
{
    public class ValidDateAttribute : RangeAttribute
    {
        public ValidDateAttribute() : base(typeof(DateTime),
            new DateTime(1900, 1, 1).ToShortDateString(), DateTime.Now.ToShortDateString()) {}
    }
}