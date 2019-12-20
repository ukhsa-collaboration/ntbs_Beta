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
        public static string FormatStringToPostcodeFormat(this string postcode)
        {
            if(postcode != null)
            {
                if (postcode.Length < 3) // If the postcode is too short (e.g. from the legacy database) the Substring methods will fail
                {
                    return postcode;
                }
                else {
                    return postcode.Substring(0, postcode.Length - 3) + " " + postcode.Substring(postcode.Length - 3, 3);
                }
            }
            else
            {
                return null;
            }
        }

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
            if(nhsNumber.Length != 10) // If the NHS number is the wrong length (e.g. from the legacy database) the Substring methods will fail
            {
                return nhsNumber;
            }
            return string.Join(" ",
                nhsNumber.Substring(0, 3),
                nhsNumber.Substring(3, 3),
                nhsNumber.Substring(6, 4)
            );
        }

    }
}
