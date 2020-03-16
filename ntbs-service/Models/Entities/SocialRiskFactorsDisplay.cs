using System.Collections.Generic;

namespace ntbs_service.Models.Entities
{
    public partial class SocialRiskFactors
    {
        public string DrugRiskFactorTimePeriods => CreateTimePeriodsString(RiskFactorDrugs);
        public string HomelessRiskFactorTimePeriods => CreateTimePeriodsString(RiskFactorHomelessness);
        public string ImprisonmentRiskFactorTimePeriods => CreateTimePeriodsString(RiskFactorImprisonment);
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
