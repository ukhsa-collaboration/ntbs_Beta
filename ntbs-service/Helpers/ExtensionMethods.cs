using System;
using System.Linq;
using System.Reflection;
using ntbs_service.Models;

namespace ntbs_service.Helpers
{
    public static class DateTimeExtension
    {
        public static FormattedDate ConvertToFormattedDate(this DateTime? dateTime) {

            return new FormattedDate() { Day = dateTime?.Day.ToString(), Month = dateTime?.Month.ToString(), Year = dateTime?.Year.ToString() };
        }
    }

    public static class EnumExtension
    {
        public static TAttribute GetAttribute<TAttribute>(Enum enumValue) 
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}