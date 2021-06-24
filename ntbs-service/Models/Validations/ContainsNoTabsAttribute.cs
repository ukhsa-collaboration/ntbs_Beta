using System;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainsNoTabsAttribute : ValidationAttribute
    {
        public ContainsNoTabsAttribute()
        {
            ErrorMessage = ValidationMessages.StringCannotContainTabs;
        }

        public override bool IsValid(object value)
        {
            var notes = (string)value;

            return string.IsNullOrEmpty(notes) || !notes.Contains('\t');
        }
    }
}
