using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ntbs_service.Helpers
{
    public static class ReflectionExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? string.Empty;
        }

        public static string GetMemberDisplayNameValue<T>(this T _, string propertyName)
        {
            return typeof(T).GetMember(propertyName).FirstOrDefault()
                       ?.GetCustomAttribute<DisplayAttribute>()
                       ?.Name ?? string.Empty;
        }
    }
}
