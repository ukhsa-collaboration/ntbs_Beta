using System;
using ntbs_service.Models;

namespace ntbs_service.Helpers
{
    public static class DateTimeExtension
    {
        public static FormattedDate ConvertToFormattedDate(this DateTime? dateTime)
        {
            return dateTime == null ? new FormattedDate() : dateTime.Value.ConvertToFormattedDate();
        }

        public static FormattedDate ConvertToFormattedDate(this DateTime dateTime)
        {
            return new FormattedDate
            {
                Day = dateTime.Day.ToString(),
                Month = dateTime.Month.ToString(),
                Year = dateTime.Year.ToString()
            };
        }

        public static string ConvertToString(this DateTime? dateTime)
        {
            return dateTime?.ConvertToString();
        }

        public static string ConvertToString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMM yyyy");
        }
    }       

    public static class NullableBoolExtensions
    {
        public static string FormatYesNo(this bool? x)
        {
            if (x == null)
            {
                return string.Empty;
            }
            else
            {
                return x.Value ? "Yes" : "No";
            }
        }
    }

    public static class StringExtensions
    {
        public static string FormatStringToNhsNumberFormat(this string nhsNumber)
        {
            if (nhsNumber == null)
            {
                return "Not known";
            }
            if (string.IsNullOrEmpty(nhsNumber))
            {
                return string.Empty;
            }
            return string.Join(" ",
                nhsNumber.Substring(0, 3),
                nhsNumber.Substring(3, 3),
                nhsNumber.Substring(6, 4)
            );
        }
    }
}
