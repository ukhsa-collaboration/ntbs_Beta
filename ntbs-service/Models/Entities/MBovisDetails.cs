using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class MBovisDetails : ModelBase, IOwnedEntityForAuditing
    {
        public int NotificationId { get; set; }

        [AssertThat(nameof(ExposureToKnownCasesIsPopulatedIfTrue), ErrorMessage = ValidationMessages.HasNoExposureRecords)]
        [Display(Name = "Has exposure to known TB case")]
        public bool? HasExposureToKnownCases { get; set; }
        public virtual ICollection<MBovisExposureToKnownCase> MBovisExposureToKnownCases { get; set; }

        [NotMapped]
        public bool ExposureToKnownCasesIsPopulatedIfTrue =>
            // Test only relevant if collection is loaded
            MBovisExposureToKnownCases == null
            // Test only relevant if HasExposure is true
            || HasExposureToKnownCases == false 
            // Confirm collection is populated...
            || MBovisExposureToKnownCases.Any()
            // ...unless about to add entry, in which case that's fine too
            || ProceedingToAdd;
        
        
        [AssertThat(nameof(UnpasteurisedMilkConsumptionsIsPopulatedIfTrue), ErrorMessage = ValidationMessages.HasNoUnpasteurisedMilkConsumptionRecords)]
        [Display(Name = "Has consumption of unpasteurised milk products")]
        public bool? HasUnpasteurisedMilkConsumption { get; set; }
        public virtual ICollection<MBovisUnpasteurisedMilkConsumption> MBovisUnpasteurisedMilkConsumptions { get; set; }

        [NotMapped]
        public bool UnpasteurisedMilkConsumptionsIsPopulatedIfTrue =>
            // Test only relevant if collection is loaded
            MBovisUnpasteurisedMilkConsumptions == null
            // Test only relevant if HasConsumption is true
            || HasUnpasteurisedMilkConsumption == false 
            // Confirm collection is populated...
            || MBovisUnpasteurisedMilkConsumptions.Any()
            // ...unless about to add entry, in which case that's fine too
            || ProceedingToAdd;
        
        
        [AssertThat(nameof(OccupationExposuresIsPopulatedIfTrue), ErrorMessage = ValidationMessages.HasNoOccupationExposureRecords)]
        [Display(Name = "Has occupation exposure")]
        public bool? HasOccupationExposure { get; set; }
        public virtual ICollection<MBovisOccupationExposure> MBovisOccupationExposures { get; set; }
        
        [NotMapped]
        public bool OccupationExposuresIsPopulatedIfTrue =>
            // Test only relevant if collection is loaded
            MBovisOccupationExposures == null
            // Test only relevant if HasExposure is true
            || HasOccupationExposure == false 
            // Confirm collection is populated...
            || MBovisOccupationExposures.Any()
            // ...unless about to add entry, in which case that's fine too
            || ProceedingToAdd;

        
        // Only used to inform validation, much like the `ShouldValidateFull` property
        [NotMapped]
        public bool ProceedingToAdd { get; set; }
        
        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
