using System;

namespace ntbs_service.Models.Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationChildAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationChildEnumerableAttribute : Attribute
    {
    }
}
