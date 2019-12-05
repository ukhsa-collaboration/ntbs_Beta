using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class SocialRiskFactors : ModelBase
    {
        public SocialRiskFactors()
        {
            RiskFactorHomelessness = new RiskFactorDetails(RiskFactorType.Homelessness);
            RiskFactorDrugs = new RiskFactorDetails(RiskFactorType.Drugs);
            RiskFactorImprisonment = new RiskFactorDetails(RiskFactorType.Imprisonment);
        }

        public Status? AlcoholMisuseStatus { get; set; }
        public Status? SmokingStatus { get; set; }
        public Status? MentalHealthStatus { get; set; }
        public virtual RiskFactorDetails RiskFactorDrugs { get; set; }
        public virtual RiskFactorDetails RiskFactorHomelessness { get; set; }
        public virtual RiskFactorDetails RiskFactorImprisonment { get; set; }
    }
}
