using System;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models
{
    [Owned]
    public class PatientTBHistory : ModelBase
    {
        public bool NotPreviouslyHadTB { get; set; }
        [Range(1900, 2000, ErrorMessage = ValidationMessages.PreviousTBDiagnosisYear)]
        public int? PreviousTBDiagnosisYear { get; set; }
    }
}