using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public bool HasHighTbOccurence { get; set; }
    }
}
