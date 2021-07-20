using System.ComponentModel.DataAnnotations;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    [Display(Name = "Contact tracing")]
    public partial class ContactTracing : ModelBase, IOwnedEntityForAuditing
    {
        [Range(0, int.MaxValue, ErrorMessage = ValidationMessages.PositiveNumbersOnly)]
        public int? AdultsIdentified { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ValidationMessages.PositiveNumbersOnly)]
        public int? ChildrenIdentified { get; set; }

        [PositiveIntegerSmallerThanValue("AdultsIdentified", ErrorMessage = ValidationMessages.ContactTracingAdultsScreened)]
        public int? AdultsScreened { get; set; }

        [PositiveIntegerSmallerThanValue("ChildrenIdentified", ErrorMessage = ValidationMessages.ContactTracingChildrenScreened)]
        public int? ChildrenScreened { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("AdultsScreened", "AdultsLatentTB", ErrorMessage = ValidationMessages.ContactTracingAdultsActiveTB)]
        public int? AdultsActiveTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("ChildrenScreened", "ChildrenLatentTB", ErrorMessage = ValidationMessages.ContactTracingChildrenActiveTB)]
        public int? ChildrenActiveTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("AdultsScreened", "AdultsActiveTB", ErrorMessage = ValidationMessages.ContactTracingAdultsLatentTB)]
        public int? AdultsLatentTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("ChildrenScreened", "ChildrenActiveTB", ErrorMessage = ValidationMessages.ContactTracingChildrenLatentTB)]
        public int? ChildrenLatentTB { get; set; }

        [PositiveIntegerSmallerThanValue("AdultsLatentTB", ErrorMessage = ValidationMessages.ContactTracingAdultsStartedTreatment)]
        public int? AdultsStartedTreatment { get; set; }

        [PositiveIntegerSmallerThanValue("ChildrenLatentTB", ErrorMessage = ValidationMessages.ContactTracingChildrenStartedTreatment)]
        public int? ChildrenStartedTreatment { get; set; }

        [PositiveIntegerSmallerThanValue("AdultsStartedTreatment", ErrorMessage = ValidationMessages.ContactTracingAdultsFinishedTreatment)]
        public int? AdultsFinishedTreatment { get; set; }

        [PositiveIntegerSmallerThanValue("ChildrenStartedTreatment", ErrorMessage = ValidationMessages.ContactTracingChildrenFinishedTreatment)]
        public int? ChildrenFinishedTreatment { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
