using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Castle.Core.Internal;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Display(Name = "M. bovis details")]
    public class MBovisDetails : ModelBase, IOwnedEntityForAuditing
    {
        public int NotificationId { get; set; }

        [AssertThat(nameof(ExposureToKnownCasesStatusIsValid), ErrorMessage = ValidationMessages.HasNoExposureRecords)]
        [Display(Name = "Has the patient been exposed to a known TB case?")]
        public Status? ExposureToKnownCasesStatus { get; set; }
        public virtual ICollection<MBovisExposureToKnownCase> MBovisExposureToKnownCases { get; set; }

        [NotMapped]
        public bool ExposureToKnownCasesStatusIsValid =>
            // Test only relevant if collection is loaded
            MBovisExposureToKnownCases == null
            // Test only relevant if HasExposure is yes
            || ExposureToKnownCasesStatus == Status.No
            || ExposureToKnownCasesStatus == Status.Unknown
            // Confirm collection is populated...
            || MBovisExposureToKnownCases.Any()
            // ...unless about to add entry, in which case that's fine too
            || ProceedingToAdd;


        [AssertThat(nameof(UnpasteurisedMilkConsumptionStatusIsValid), ErrorMessage = ValidationMessages.HasNoUnpasteurisedMilkConsumptionRecords)]
        [Display(Name = "Has the patient consumed unpasteurised milk products?")]
        public Status? UnpasteurisedMilkConsumptionStatus { get; set; }
        public virtual ICollection<MBovisUnpasteurisedMilkConsumption> MBovisUnpasteurisedMilkConsumptions { get; set; }

        [NotMapped]
        public bool UnpasteurisedMilkConsumptionStatusIsValid =>
            // Test only relevant if collection is loaded
            MBovisUnpasteurisedMilkConsumptions == null
            // Test only relevant if HasConsumption is yes
            || UnpasteurisedMilkConsumptionStatus == Status.No
            || UnpasteurisedMilkConsumptionStatus == Status.Unknown
            // Confirm collection is populated...
            || MBovisUnpasteurisedMilkConsumptions.Any()
            // ...unless about to add entry, in which case that's fine too
            || ProceedingToAdd;


        [AssertThat(nameof(OccupationExposureStatusIsValid), ErrorMessage = ValidationMessages.HasNoOccupationExposureRecords)]
        [Display(Name = "Is there a possible occupational exposure to M. bovis?")]
        public Status? OccupationExposureStatus { get; set; }
        public virtual ICollection<MBovisOccupationExposure> MBovisOccupationExposures { get; set; }

        [NotMapped]
        public bool OccupationExposureStatusIsValid =>
            // Test only relevant if collection is loaded
            MBovisOccupationExposures == null
            // Test only relevant if HasExposure is yes
            || OccupationExposureStatus == Status.No
            || OccupationExposureStatus == Status.Unknown
            // Confirm collection is populated...
            || MBovisOccupationExposures.Any()
            // ...unless about to add entry, in which case that's fine too
            || ProceedingToAdd;


        [AssertThat(nameof(AnimalExposureStatusIsValid), ErrorMessage = ValidationMessages.HasNoAnimalExposureRecords)]
        [Display(Name = "Has the patient been exposed to animals?")]
        public Status? AnimalExposureStatus { get; set; }
        public virtual ICollection<MBovisAnimalExposure> MBovisAnimalExposures { get; set; }

        [NotMapped]
        public bool AnimalExposureStatusIsValid =>
            // Test only relevant if collection is loaded
            MBovisAnimalExposures == null
            // Test only relevant if HasExposure is yes
            || AnimalExposureStatus == Status.No
            || AnimalExposureStatus == Status.Unknown
            // Confirm collection is populated...
            || MBovisAnimalExposures.Any()
            // ...unless about to add entry, in which case that's fine too
            || ProceedingToAdd;


        // Only used to inform validation, much like the `ShouldValidateFull` property
        [NotMapped]
        public bool ProceedingToAdd { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;

        public bool DataEntered =>
            AnimalExposureStatus != null
            || !MBovisAnimalExposures.IsNullOrEmpty()
            || ExposureToKnownCasesStatus != null
            || !MBovisExposureToKnownCases.IsNullOrEmpty()
            || OccupationExposureStatus != null
            || !MBovisOccupationExposures.IsNullOrEmpty()
            || UnpasteurisedMilkConsumptionStatus != null
            || !MBovisUnpasteurisedMilkConsumptions.IsNullOrEmpty();
    }
}
