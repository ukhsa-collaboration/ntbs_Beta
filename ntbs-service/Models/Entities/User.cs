using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [AtLeastOneProperty(
        nameof(JobTitle),
        nameof(PhoneNumberPrimary),
        nameof(EmailPrimary),
        ErrorMessage = ValidationMessages.SupplyCaseManagerPrimaryParameter)]
    public class User
    {
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public string AdGroups { get; set; }
        public bool IsActive { get; set; }
        public bool IsCaseManager { get; set; }
        
        [Display(Name="Job Title")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string JobTitle { get; set; }
        
        [Display(Name="Email #1")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string EmailPrimary { get; set; }
        
        [Display(Name="Email #2")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string EmailSecondary { get; set; }
        
        [Display(Name="Phone number #1")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string PhoneNumberPrimary { get; set; }
        
        [Display(Name="Phone number #2")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string PhoneNumberSecondary { get; set; }
        
        [Display(Name="Notes")]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtendedWithNewLine,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        public string Notes { get; set; }

        public virtual ICollection<CaseManagerTbService> CaseManagerTbServices { get; set; }

        public string FullName => GivenName + " " + FamilyName;
    }
}
