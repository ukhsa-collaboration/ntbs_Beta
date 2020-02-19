using System;
using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public class StringToValueConverter
    {
        public static Status? GetStatusFromString(string status)
        {
            return GetEnumValue<Status>(status);
        }

        public static bool GetBoolValue(int? value)
        {
            return value == 1;
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

        public static HIVTestStatus? GetHivStatusValue(string hivTestStatusRaw)
        {
            return GetEnumValue<HIVTestStatus>(hivTestStatusRaw);
        }

        public static DotStatus? GetDotStatusValue(string dotStatus)
        {
            return GetEnumValue<DotStatus>(dotStatus);
        }
        
        
        static T? GetEnumValue<T>(string raw) where T : struct
        {
            return string.IsNullOrEmpty(raw) ? 
                null : 
                (T?)Enum.Parse<T>(raw);
        }
    }
}
