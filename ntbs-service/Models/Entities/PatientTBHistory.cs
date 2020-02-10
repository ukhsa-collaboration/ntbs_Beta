using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class PatientTBHistory : ModelBase, IOwnedEntity
    {
        public bool? PreviouslyHadTB { get; set; }
        
        [AssertThat(@"DobYear == null || PreviousTBDiagnosisYear >= DobYear", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDobYear)]
        [AssertThat(nameof(IsYearBeforeCurrentYear), ErrorMessage = ValidationMessages.BeforeCurrentYear)]
        [Range(ValidDates.EarliestYear, 2100, ErrorMessage = ValidationMessages.ValidYear)]
        [Display(Name = "Previous year of diagnosis")]
        public int? PreviousTBDiagnosisYear { get; set; }
        
        [NotMapped]
        public int? DobYear { get; set; }

        public bool IsYearBeforeCurrentYear => PreviousTBDiagnosisYear <= DateTime.Now.Year;

        string IOwnedEntity.RootEntityType => RootEntities.Notification;
    }
}
