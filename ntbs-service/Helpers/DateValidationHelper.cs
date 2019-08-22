using System;

namespace ntbs_service.Helpers
{
    public static class DateValidationHelper
    {
        public static DateTime EarliestDate = new DateTime(1900, 1, 1);
        public static string ErrorMessage = $"Must be between {EarliestDate.ToShortDateString()} and {DateTime.Now.ToShortDateString()}";

         public static bool IsValidDate(DateTime dateTime)
        {
            return dateTime >= EarliestDate && dateTime <= DateTime.Now;
        }
    }
}