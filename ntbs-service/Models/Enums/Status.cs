using System;
using System.Collections.Generic;
using System.Linq;

namespace ntbs_service.Models.Enums 
{
    public enum Status {
        Yes,
        No,
        Unknown
    }

    public static class StatusHelper
    {
        public static List<Status> GetAll()
        {
            return Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
        }
    }
}