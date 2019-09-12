using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class ContactTracing
    {
        [Range(0, int.MaxValue, ErrorMessage = ValidationMessages.PositiveNumbersOnly)]
        public int AdultsIdentified { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ValidationMessages.PositiveNumbersOnly)]
        public int ChildrenIdentified { get; set; }

        [PositiveIntegerSmallerThanValue("AdultsIdentified", ErrorMessage = ValidationMessages.ContactTracingContactsScreened)]
        public int AdultsScreened { get; set; }

        [PositiveIntegerSmallerThanValue("ChildrenIdentified", ErrorMessage = ValidationMessages.ContactTracingContactsScreened)]
        public int ChildrenScreened { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("AdultsScreened", "AdultsLatentTB", ErrorMessage = ValidationMessages.ContactTracingContactsActiveTB)]      
        public int AdultsActiveTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("ChildrenScreened", "ChildrenLatentTB", ErrorMessage = ValidationMessages.ContactTracingContactsActiveTB)]
        public int ChildrenActiveTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("AdultsScreened", "AdultsActiveTB", ErrorMessage = ValidationMessages.ContactTracingContactsLatentTB)]
        public int AdultsLatentTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("ChildrenScreened", "ChildrenActiveTB", ErrorMessage = ValidationMessages.ContactTracingContactsLatentTB)]
        public int ChildrenLatentTB { get; set; }
    
        
        [PositiveIntegerSmallerThanValue("AdultsLatentTB", ErrorMessage = ValidationMessages.ContactTracingContactsStartedTreatment)]
        public int AdultsStartedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("ChildrenLatentTB", ErrorMessage = ValidationMessages.ContactTracingContactsStartedTreatment)]
        public int ChildrenStartedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("AdultsStartedTreatment", ErrorMessage = ValidationMessages.ContactTracingContactsFinishedTreatment)]
        public int AdultsFinishedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("ChildrenStartedTreatment", ErrorMessage = ValidationMessages.ContactTracingContactsFinishedTreatment)]
        public int ChildrenFinishedTreatment { get; set; }
    }
}