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
    public class ClinicalDetails : ModelBase, IOwnedEntity
    {
        #region DB Mapped Fields
        public bool? IsSymptomatic { get; set; }

        [Display(Name = "Symptom onset date")]
        [ValidClinicalDate]
        [AssertThat(@"AfterDob(SymptomStartDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? SymptomStartDate { get; set; }

        [Display(Name = "Presentation to any health service")]
        [ValidClinicalDate]
        [AssertThat(@"AfterDob(FirstPresentationDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? FirstPresentationDate { get; set; }

        [Display(Name = "Presentation to TB service")]
        [ValidClinicalDate]
        [AssertThat(@"AfterDob(TBServicePresentationDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? TBServicePresentationDate { get; set; }

        [Display(Name = "Diagnosis date")]
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(@"AfterDob(DiagnosisDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [ValidClinicalDate]
        public DateTime? DiagnosisDate { get; set; }

        [Display(Name = "Treatment start date")]
        [ValidClinicalDate]
        [AssertThat(@"AfterDob(TreatmentStartDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? TreatmentStartDate { get; set; }

        [Display(Name = "Date of death")]
        [RequiredIf(@"ShouldValidateFull && IsPostMortem == true", ErrorMessage = ValidationMessages.FieldRequired)]
        [ValidClinicalDate]
        [AssertThat(@"AfterDob(DeathDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
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
        [ValidClinicalDate]
        [AssertThat(@"AfterDob(MDRTreatmentStartDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? MDRTreatmentStartDate { get; set; }
        
        [Display(Name="Notes")]
        [MaxLength(1000)]
        [RegularExpression(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtendedWithNewLine, 
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string Notes { get; set; }

        [Display(Name = "DOT offered")] 
        public bool? IsDotOffered { get; set; }
        
        public DotStatus? DotStatus { get; set; }
        public Status? EnhancedCaseManagementStatus { get; set; }
        
        [Display(Name = "Home visit carried out?")]
        public Status? HomeVisitCarriedOut { get; set; }
        
        [Display(Name = "First home visit date")]
        [ValidClinicalDate]
        [AssertThat(@"AfterDob(FirstHomeVisitDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? FirstHomeVisitDate { get; set; } 
        
        [Display(Name = "Healthcare setting")]
        public HealthcareSetting? HealthcareSetting { get; set; }
        
        [MaxLength(100)]
        [Display(Name = "Other description")]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string HealthcareDescription { get; set; }
        
        #endregion
        
        [NotMapped]
        public DateTime? Dob { get; set; }
        public bool AfterDob(DateTime date) => Dob == null || date >= Dob;

        string IOwnedEntity.RootEntityType => RootEntities.Notification;
    }
}
