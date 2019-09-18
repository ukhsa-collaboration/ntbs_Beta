using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    [Owned]
    public class SocialRiskFactors
    {
        public SocialRiskFactors() {
            RiskFactorHomelessness = new RiskFactorHomelessness();
            RiskFactorDrugs = new RiskFactorDrugs();
            RiskFactorImprisonment = new RiskFactorImprisonment();
        }
        public Status? AlcoholMisuseStatus { get; set; }
        public Status? SmokingStatus { get; set; }
        public Status? MentalHealthStatus { get; set; }
        public RiskFactorBase RiskFactorDrugs { get; set; }
        public RiskFactorBase RiskFactorHomelessness { get; set; }
        public RiskFactorBase RiskFactorImprisonment { get; set; }
    }
}