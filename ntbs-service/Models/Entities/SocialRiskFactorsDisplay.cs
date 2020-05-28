using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Entities
{
    public partial class SocialRiskFactors
    {
        [Display(Name = "Time periods")]
        public string DrugRiskFactorTimePeriods => CreateTimePeriodsString(RiskFactorDrugs);
        [Display(Name = "Time periods")]
        public string HomelessRiskFactorTimePeriods => CreateTimePeriodsString(RiskFactorHomelessness);
        [Display(Name = "Time periods")]
        public string ImprisonmentRiskFactorTimePeriods => CreateTimePeriodsString(RiskFactorImprisonment);
        [Display(Name = "Time periods")]
        public string SmokingRiskFactorTimePeriods => CreateTimePeriodsString(RiskFactorSmoking);
        
        private static string CreateTimePeriodsString(RiskFactorDetails riskFactor)
        {
            var timeStrings = new List<string>();
            if (riskFactor.IsCurrent == true)
            {
                timeStrings.Add("current");
            }
            if (riskFactor.InPastFiveYears == true)
            {
                timeStrings.Add("less than 5 years ago");
            }
            if (riskFactor.MoreThanFiveYearsAgo == true)
            {
                timeStrings.Add("more than 5 years ago");
            }
            return string.Join(", ", timeStrings);
        }
    }
}
