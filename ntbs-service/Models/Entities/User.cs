using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public string AdGroups { get; set; }
        public bool IsActive { get; set; }
        public bool IsReadOnly { get; set; }

        [Display(Name = "Case manager")]
        public bool IsCaseManager { get; set; }

        [MaxLength(100)]
        [Display(Name = "Job title")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasic,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string JobTitle { get; set; }

        [MaxLength(100)]
        [Display(Name = "Email #1")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasic,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string EmailPrimary { get; set; }

        [MaxLength(100)]
        [Display(Name = "Email #2")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasic,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string EmailSecondary { get; set; }

        [MaxLength(100)]
        [Display(Name = "Phone number #1")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasic,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string PhoneNumberPrimary { get; set; }

        [MaxLength(100)]
        [Display(Name = "Phone number #2")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasic,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string PhoneNumberSecondary { get; set; }

        [MaxLength(500)]
        [ContainsNoTabs]
        [Display(Name = "Notes")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasicWithNewline,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string Notes { get; set; }

        public virtual ICollection<CaseManagerTbService> CaseManagerTbServices { get; set; }

        public bool ArePrimaryContactDetailsMissing => string.IsNullOrEmpty(JobTitle)
                                                       && string.IsNullOrEmpty(PhoneNumberPrimary)
                                                       && string.IsNullOrEmpty(EmailPrimary);
    }
}
