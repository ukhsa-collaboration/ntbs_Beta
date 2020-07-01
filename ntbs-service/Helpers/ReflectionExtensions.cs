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
            return enumValue?.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.GetName() ?? string.Empty;
        }
        
        public static string GetDisplayName(this Type model)
        {
            return model.GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? String.Empty;
        }

        public static string GetMemberDisplayName(this Type baseType, string propertyName)
        {
            return baseType.GetMember(propertyName).FirstOrDefault()
                       ?.GetCustomAttribute<DisplayAttribute>()
                       ?.GetName() ?? string.Empty;
        }

        public static string GetMemberDisplayName<T>(this T _, string propertyName)
        {
            return typeof(T).GetMemberDisplayName(propertyName);
        }
    }
}
