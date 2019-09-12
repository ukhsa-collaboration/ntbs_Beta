using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class ContactTracing
    {
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive value")]
        public int AdultsIdentified { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive value")]
        public int ChildrenIdentified { get; set; }

        [PositiveIntegerSmallerThanValue("AdultsIdentified", ErrorMessage = "Must be smaller or equal to the number identified")]
        public int AdultsScreened { get; set; }

        [PositiveIntegerSmallerThanValue("ChildrenIdentified", ErrorMessage = "Must be smaller or equal to the number identified")]
        public int ChildrenScreened { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("AdultsScreened", "AdultsLatentTB", ErrorMessage = "Must be smaller or equal to than number screened minus those with latent TB")]      
        public int AdultsActiveTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("ChildrenScreened", "ChildrenLatentTB", ErrorMessage = "Must be smaller or equal to than number screened minus those with latent TB")]
        public int ChildrenActiveTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("AdultsScreened", "AdultsActiveTB", ErrorMessage = "Must be smaller or equal to than number of adults screened minus those with active TB")]
        public int AdultsLatentTB { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("ChildrenScreened", "ChildrenActiveTB", ErrorMessage = "Must be smaller or equal to than number of children screened minus those with active TB")]
        public int ChildrenLatentTB { get; set; }
    
        
        [PositiveIntegerSmallerThanValue("AdultsLatentTB", ErrorMessage = "Must be smaller or equal to number of adults with latent TB")]
        public int AdultsStartedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("ChildrenLatentTB", ErrorMessage = "Must be smaller or equal to number of children with latent TB")]
        public int ChildrenStartedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("AdultsStartedTreatment", ErrorMessage = "Must be smaller or equal to number of adults the started treatment")]
        public int AdultsFinishedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("ChildrenStartedTreatment", ErrorMessage = "Must be smaller or equal to number of children the started treatment")]
        public int ChildrenFinishedTreatment { get; set; }
    }
}