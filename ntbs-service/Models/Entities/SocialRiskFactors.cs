using System.ComponentModel.DataAnnotations;
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
            RiskFactorSmoking = new RiskFactorDetails(RiskFactorType.Smoking);
        }

        public Status? AlcoholMisuseStatus { get; set; }
        public Status? MentalHealthStatus { get; set; }
        public Status? AsylumSeekerStatus { get; set; }
        public Status? ImmigrationDetaineeStatus { get; set; }
        
        [Display(Name = "History of smoking")]
        public virtual RiskFactorDetails RiskFactorSmoking { get; set; }
        [Display(Name = "History of drug misuse")]
        public virtual RiskFactorDetails RiskFactorDrugs { get; set; }
        [Display(Name = "History of drugs")]
        public virtual RiskFactorDetails RiskFactorHomelessness { get; set; }
        [Display(Name = "History of homelessness")]
        public virtual RiskFactorDetails RiskFactorImprisonment { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
