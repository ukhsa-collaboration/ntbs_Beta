using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
    }

    public enum CountryCode
    {
        UK = 1,
        UNKNOWN = 2
    }
}
