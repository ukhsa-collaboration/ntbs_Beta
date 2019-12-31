using System;
using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public class StringToValueConverter
    {
        public static Status? GetStatusFromString(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return null;
            }

            return Enum.Parse<Status>(status);
        }

        public static bool GetBoolValue(int? value)
        {
            return value == 1 ? true : false;
        }

        public static bool? GetNullableBoolValue(int? value)
        {
            if (value == null)
            {
                return null;
            }
            return GetBoolValue(value);
        }

        public static int? ToNullableInt(string stringValue)
        {
            return int.TryParse(stringValue, out var intValue) ? (int?)intValue : null;
        }
    }
}