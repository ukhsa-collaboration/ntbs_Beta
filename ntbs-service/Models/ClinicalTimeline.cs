using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class ClinicalTimeline
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
    }
}