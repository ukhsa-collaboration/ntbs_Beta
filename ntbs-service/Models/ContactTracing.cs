using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class ContactTracing
    {
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public static int AdultsIdentified { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int ChildrenIdentified { get; set; }

        [PositiveIntegerSmallerThanValue("AdultsIdentified")]
        public int AdultsScreened { get; set; }

        [PositiveIntegerSmallerThanValue("ChildrenIdentified")]
        public int ChildrenScreened { get; set; }

        [PositiveIntegerSmallerThanDifferenceOfValues("AdultsScreened", "AdultsLatentTB")]        
        public int AdultsActiveTB { get; set; }

        public int ChildrenActiveTB { get; set; }

        public int AdultsLatentTB { get; set; }

        public int ChildrenLatentTB { get; set; }
    
        
        [PositiveIntegerSmallerThanValue("AdultsLatentTB")]
        public int AdultsStartedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("ChildrenLatentTB")]
        public int ChildrenStartedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("AdultsStartedTreatment")]
        public int AdultsFinishedTreatment { get; set; }

        
        [PositiveIntegerSmallerThanValue("ChildrenStartedTreatment")]
        public int ChildrenFinishedTreatment { get; set; }
    }
}