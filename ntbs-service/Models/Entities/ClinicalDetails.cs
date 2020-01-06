using System;
using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class ClinicalDetails : ModelBase, IOwnedEntity
    {
        public bool? IsSymptomatic { get; set; }

        [Display(Name = "Symptom onset date")]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        public DateTime? SymptomStartDate { get; set; }

        [Display(Name = "Presentation to any health service")]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        public DateTime? FirstPresentationDate { get; set; }

        [Display(Name = "Presentation to TB service")]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        public DateTime? TBServicePresentationDate { get; set; }

        [Display(Name = "Diagnosis date")]
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        public DateTime? DiagnosisDate { get; set; }

        [Display(Name = "Treatment start date")]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        public DateTime? TreatmentStartDate { get; set; }

        [Display(Name = "Date of death")]
        [RequiredIf(@"ShouldValidateFull && IsPostMortem == true", ErrorMessage = ValidationMessages.FieldRequired)]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        public DateTime? DeathDate { get; set; }

        public bool? DidNotStartTreatment { get; set; }
        public bool? IsPostMortem { get; set; }

        public Status? BCGVaccinationState { get; set; }
        [Display(Name = "BCG vaccination year")]
        [RequiredIf(@"ShouldValidateFull && BCGVaccinationState == Enums.Status.Yes", ErrorMessage = ValidationMessages.FieldRequired)]
        public int? BCGVaccinationYear { get; set; }
        public HIVTestStatus? HIVTestState { get; set; }

        [OnlyOneTrue("IsMDRTreatment", ErrorMessage = ValidationMessages.ValidTreatmentOptions)]
        public bool? IsShortCourseTreatment { get; set; }

        [OnlyOneTrue("IsShortCourseTreatment", ErrorMessage = ValidationMessages.ValidTreatmentOptions)]
        public bool? IsMDRTreatment { get; set; }

        [Display(Name = "RR/MDR/XDR treatment date")]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        public DateTime? MDRTreatmentStartDate { get; set; }
        public Status? DotStatus { get; set; }
        public Status? EnhancedCaseManagementStatus { get; set; }

        string IOwnedEntity.RootEntityType => RootEntities.Notification;
    }
}
