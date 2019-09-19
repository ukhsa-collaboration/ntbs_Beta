using System;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class ClinicalDetails
    {
        [ValidDate(ValidDates.EarliestClinicalDate)]
        public DateTime? SymptomStartDate { get; set; }

        [ValidDate(ValidDates.EarliestClinicalDate)]
        public DateTime? PresentationDate { get; set; }

        [ValidDate(ValidDates.EarliestClinicalDate)]
        public DateTime? DiagnosisDate { get; set; }

        [ValidDate(ValidDates.EarliestClinicalDate)]
        public DateTime? TreatmentStartDate { get; set; }

        [ValidDate(ValidDates.EarliestClinicalDate)]
        public DateTime? DeathDate { get; set; }

        public bool DidNotStartTreatment { get; set; }
        public bool IsPostMortem { get; set; }

        public bool NoSampleTaken { get; set; }

        public Status BCGVaccinationState { get; set; }
        public int? BCGVaccinationYear { get; set; }

        [OnlyOneTrue("IsMDRTreatment", ErrorMessage = ValidationMessages.ValidTreatmentOptions)]
        public bool IsShortCourseTreatment { get; set; }

        [OnlyOneTrue("IsShortCourseTreatment", ErrorMessage = ValidationMessages.ValidTreatmentOptions)]
        public bool IsMDRTreatment { get; set; }
        [ValidDate(ValidDates.EarliestClinicalDate)]

        public DateTime? MDRTreatmentStartDate { get; set; }
    }
}