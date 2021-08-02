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
    [Display(Name = "Clinical details")]
    public partial class ClinicalDetails : ModelBase, IOwnedEntityForAuditing
    {
        #region DB Mapped Fields
        [Display(Name = "Does the patient have TB symptoms?")]
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

        /// <summary>
        /// "Starting" date is a bit of an informal name for the concept for a date which is important from analytical
        /// perspective for the notification. This is the date that corresponds to the "starting event" created for the
        /// notification, and is the date based on which further history of the notification is assessed, e.g.
        /// the "outcome at 12 months" means 12 months after the starting date.
        /// </summary>
        public DateTime? StartingDate => TreatmentStartDate ?? DiagnosisDate;

        [Display(Name = "Has the patient started treatment?")]
        public bool? StartedTreatment { get; set; }

        [Display(Name = "Was the TB diagnosis made at post-mortem?")]
        public bool? IsPostMortem { get; set; }

        [Display(Name = "Has the patient had a BCG vaccination?")]
        public Status? BCGVaccinationState { get; set; }

        [Display(Name = "BCG vaccination year")]
        public int? BCGVaccinationYear { get; set; }
        public HIVTestStatus? HIVTestState { get; set; }

        public TreatmentRegimen? TreatmentRegimen { get; set; }
        public bool IsMDRTreatment => TreatmentRegimen == Enums.TreatmentRegimen.MdrTreatment;

        [ValidClinicalDate]
        [Display(Name = "RR/MDR/XDR treatment date")]
        [RequiredIf(@"IsMDRTreatment && IsLegacy != true", ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(@"AfterDob(MDRTreatmentStartDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? MDRTreatmentStartDate { get; set; }

        [MaxLength(100)]
        [Display(Name = "Treatment regimen other description")]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Column]
        public string TreatmentRegimenOtherDescription { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(1000, ErrorMessage = ValidationMessages.MaximumTextLength)]
        [ContainsNoTabs]
        [RegularExpression(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtendedWithNewLine,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string Notes { get; set; }

        [Display(Name = "DOT offered")]
        public Status? IsDotOffered { get; set; }

        public DotStatus? DotStatus { get; set; }
        public Status? EnhancedCaseManagementStatus { get; set; }

        [Display(Name = "Enhanced case management level")]
        public byte EnhancedCaseManagementLevel { get; set; }

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

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
