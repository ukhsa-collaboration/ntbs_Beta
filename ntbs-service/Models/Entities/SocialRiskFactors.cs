using System.Diagnostics.CodeAnalysis;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    [Owned]
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public partial class SocialRiskFactors : ModelBase, IOwnedEntityForAuditing
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
        public Status? AsylumSeekerStatus { get; set; }
        public Status? ImmigrationDetaineeStatus { get; set; }
        public virtual RiskFactorDetails RiskFactorDrugs { get; set; }
        public virtual RiskFactorDetails RiskFactorHomelessness { get; set; }
        public virtual RiskFactorDetails RiskFactorImprisonment { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
