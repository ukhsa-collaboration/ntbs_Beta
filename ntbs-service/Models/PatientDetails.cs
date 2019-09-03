using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class PatientDetails
    {
        private const string NameRegex = @"[a-zA-Z \-,.']+";

        [StringLength(35)]
        [RegularExpression(NameRegex, ErrorMessage = ValidationMessages.NameFormat)]
        public string FamilyName { get; set; }

        [StringLength(35)]
        [RegularExpression(NameRegex, ErrorMessage = ValidationMessages.NameFormat)]
        public string GivenName { get; set; }

        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NhsNumberFormat)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = ValidationMessages.NhsNumberLength)]
        public string NhsNumber { get; set; }

        [ValidDate(ValidDates.EarliestBirthDate)]
        public DateTime? Dob { get; set; }
        public bool? UkBorn { get; set; }

        public string Postcode {get; set; }

        public int? CountryId { get; set;}
        public virtual Country Country { get; set; }

        public int? EthnicityId { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }

        public int? SexId { get; set; }
        public virtual Sex Sex { get; set; }
        public bool NhsNumberNotKnown { get; set; }
        public bool NoFixedAbode { get; set; }
    }

}
