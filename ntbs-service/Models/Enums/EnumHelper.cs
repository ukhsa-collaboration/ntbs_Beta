using System;
using System.Collections.Generic;
using System.Linq;

namespace ntbs_service.Models.Enums
{
    public static class EnumHelper
    {
        public static List<TEnum> GetEnumList<TEnum>() where TEnum : Enum
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
    }
}
