using System;
using System.Collections.Generic;
using System.Linq;

namespace ntbs_service.Models.Enums
{
    public enum Result
    {
        Positive,
        Negative,
        Awaiting
    }

    public static class ResultEnumHelper
    {
        public static List<Result> GetAll()
        {
            return Enum.GetValues(typeof(Result)).Cast<Result>().ToList();
        }
    }
}
