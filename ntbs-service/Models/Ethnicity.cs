using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class Ethnicity
    {
        public int EthnicityId { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public int Order { get; set; }
    }
}