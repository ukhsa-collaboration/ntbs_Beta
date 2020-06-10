using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public partial class PreviousTbHistory : ModelBase, IOwnedEntityForAuditing
    {
        [Display(Name = "Has the patient had any previous occurrence of TB which has not been notified to PHE through NTBS (or the legacy systems ETS/LTBR)?")]
        public Status? PreviouslyHadTb { get; set; }
        
        [AssertThat(@"DobYear == null || PreviousTbDiagnosisYear >= DobYear", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDobYear)]
        [AssertThat(nameof(IsYearBeforeCurrentYear), ErrorMessage = ValidationMessages.BeforeCurrentYear)]
        [Range(ValidDates.EarliestYear, 2100, ErrorMessage = ValidationMessages.ValidYear)]
        [Display(Name = "Previous year of diagnosis")]
        public int? PreviousTbDiagnosisYear { get; set; }
        
        [Display(Name = "Previously treated?")]
        public Status? PreviouslyTreated { get; set; }
        
        [Display(Name = "Country of treatment")]
        public int? PreviousTreatmentCountryId { get; set; }
        public virtual Country PreviousTreatmentCountry { get; set; }

        [NotMapped]
        public int? DobYear { get; set; }

        public bool IsYearBeforeCurrentYear => PreviousTbDiagnosisYear <= DateTime.Now.Year;

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
