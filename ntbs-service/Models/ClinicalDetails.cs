using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
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

        public State BCGVaccinationState { get; set; }

        // TODO: change this to actual validation including patient year of birth once ValidationService has been refactored
        [ValidYear(ValidDates.EarliestYear)]
        public string BCGVaccinationYear { get; set; }
    }
}