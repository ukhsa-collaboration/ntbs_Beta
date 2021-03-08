using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public string AdGroups { get; set; }
        public bool IsActive { get; set; }
        public bool IsCaseManager { get; set; }

        [Display(Name = "Job Title")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string JobTitle { get; set; }

        [Display(Name = "Email #1")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string EmailPrimary { get; set; }

        [Display(Name = "Email #2")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string EmailSecondary { get; set; }

        [Display(Name = "Phone number #1")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string PhoneNumberPrimary { get; set; }

        [Display(Name = "Phone number #2")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string PhoneNumberSecondary { get; set; }

        [Display(Name = "Notes")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtendedWithNewLine,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string Notes { get; set; }

        public virtual ICollection<CaseManagerTbService> CaseManagerTbServices { get; set; }

        public bool ArePrimaryContactDetailsMissing => string.IsNullOrEmpty(JobTitle)
                                                       && string.IsNullOrEmpty(PhoneNumberPrimary)
                                                       && string.IsNullOrEmpty(EmailPrimary);
    }
}
