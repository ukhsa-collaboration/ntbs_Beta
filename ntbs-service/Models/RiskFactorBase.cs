using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [ValidRiskFactorAttribute]
    public class RiskFactorBase
    {
        public Status? Status { get; set; }
        public bool IsCurrent { get; set; }
        public bool InPastFiveYears { get; set; }
        public bool MoreThanFiveYearsAgo { get; set; }
    }
}