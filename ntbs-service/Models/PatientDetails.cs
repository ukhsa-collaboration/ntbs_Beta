using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class PatientDetails : ModelBase
    {
        [StringLength(35)]
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FamilyNameIsRequired)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string FamilyName { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.GivenNameIsRequired)]
        [StringLength(35)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string GivenName { get; set; }

        [RequiredIf(@"ShouldValidateFull && !NhsNumberNotKnown", ErrorMessage = ValidationMessages.NHSNumberIsRequired)]
        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NhsNumberFormat)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = ValidationMessages.NhsNumberLength)]
        public string NhsNumber { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.BirthDateIsRequired)]
        [ValidDate(ValidDates.EarliestBirthDate)]
        public DateTime? Dob { get; set; }
        public bool? UkBorn { get; set; }

        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine, 
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        public string Address { get; set; }

        [RequiredIf(@"ShouldValidateFull && !NoFixedAbode", ErrorMessage = ValidationMessages.PostcodeIsRequired)]
        public string Postcode {get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.BirthCountryIsRequired)]
        public int? CountryId { get; set;}
        public virtual Country Country { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.EthnicGroupIsRequired)]
        public int? EthnicityId { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.SexIsRequired)]
        public int? SexId { get; set; }
        public virtual Sex Sex { get; set; }
        public bool NhsNumberNotKnown { get; set; }
        public bool NoFixedAbode { get; set; }
    }

}
