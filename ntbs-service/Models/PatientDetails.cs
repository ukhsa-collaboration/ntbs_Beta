using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class PatientDetails : ModelBase
    {
        [StringLength(35)]
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [StringLength(35)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Display(Name = "Given name")]
        public string GivenName { get; set; }

        [RequiredIf(@"ShouldValidateFull && !NhsNumberNotKnown", ErrorMessage = ValidationMessages.FieldRequired)]
        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NumberFormat)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = ValidationMessages.NhsNumberLength)]
        [Display(Name = "NHS number")]
        public string NhsNumber { get; set; }

        [MaxLength(50)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        [Display(Name = "Local Patient Id")]
        public string LocalPatientId { get; set; }

        [Display(Name = "Date of birth")]
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [ValidDateRange(ValidDates.EarliestBirthDate)]
        public DateTime? Dob { get; set; }
        public bool? UkBorn { get; set; }

        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        public string Address { get; set; }

        [RequiredIf(@"ShouldValidateFull && !NoFixedAbode", ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(@"PostcodeToLookup != null", ErrorMessage = ValidationMessages.PostcodeIsNotValid)]
        public string Postcode { get; set; }

        public string PostcodeToLookup { get; set; }
        public virtual PostcodeLookup PostcodeLookup { get; set; }
        [NotMapped]
        public string LegacyCountryName { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [DisplayName("Birth country")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Range(1900, 2100, ErrorMessage = ValidationMessages.InvalidYearForAttribute)]
        [AssertThat("UkEntryAfterBirth == true", ErrorMessage = ValidationMessages.YearOfUkEntryMustBeAfterDob)]
        [AssertThat("UkEntryNotInFuture == true", ErrorMessage = ValidationMessages.YearOfUkEntryMustNotBeInFuture)]
        [DisplayName("Year of uk entry")]
        public int? YearOfUkEntry { get; set; }

        public bool UkEntryAfterBirth => !Dob.HasValue || YearOfUkEntry >= Dob.Value.Year;
        public bool UkEntryNotInFuture => YearOfUkEntry <= DateTime.Now.Year;

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [DisplayName("Ethnic group")]
        public int? EthnicityId { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [DisplayName("Sex")]
        public int? SexId { get; set; }
        public virtual Sex Sex { get; set; }
        public bool NhsNumberNotKnown { get; set; }
        public bool NoFixedAbode { get; set; }

        public int? OccupationId { get; set; }
        public virtual Occupation Occupation { get; set; }

        [MaxLength(50)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [DisplayName("Occupation")]
        public string OccupationOther { get; set; }

        public string FormatOccupationString()
        {
            if (Occupation == null)
            {
                return string.Empty;
            }

            if (Occupation.HasFreeTextField && !string.IsNullOrEmpty(OccupationOther))
            {
                return $"{Occupation.Sector} - {OccupationOther}";
            }

            return Occupation.Sector == "Other" ? Occupation.Role : $"{Occupation.Sector} - {Occupation.Role}";
        }
    }
}
