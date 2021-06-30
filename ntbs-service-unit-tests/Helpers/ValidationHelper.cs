using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service_unit_tests.Helpers
{
    internal static class ValidationHelper
    {
        public static List<ValidationResult> ValidateObject<T>(T target)
        {
            var context = new ValidationContext(target);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(target, context, results, true);
            return results;
        }
    }
}
