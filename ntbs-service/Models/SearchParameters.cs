using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [NotMapped]
    [AtLeastOneProperty("IdFilter", "FamilyName", "PartialDobExists", "Postcode", ErrorMessage = ValidationMessages.SupplyAParameter)]
    public class SearchParameters
    {
        [Display(Name = "Id filter")]
        [RegularExpression(ValidationRegexes.NumbersHyphenAndSpaceValidation, ErrorMessage = ValidationMessages.NumberHyphenAndSpaceFormat)]
        public string IdFilter { get; set; }
        public int? SexId { get; set; }
        public int? CountryId { get; set; }
        public string TBServiceCode { get; set; }
        [Display(Name = "Date of birth")]
        public PartialDate PartialDob { get; set; }
        [Display(Name = "Notification date")]
        public PartialDate PartialNotificationDate { get; set; }

        [MinLength(2, ErrorMessage = ValidationMessages.MinTwoCharacters)]
        [RegularExpression(ValidationRegexes.CharacterValidationWithNumbers, ErrorMessage = ValidationMessages.StandardStringWithNumbersFormat)]
        public string Postcode { get; set; }

        [Display(Name = "Family name")]
        [MinLength(2, ErrorMessage = ValidationMessages.MinTwoCharacters)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string FamilyName { get; set; }

        [Display(Name = "Given name")]
        [MinLength(2, ErrorMessage = ValidationMessages.MinTwoCharacters)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string GivenName { get; set; }

        // This returns null or a dummy value to be used in the [AtLeastOneProperty] attribute
        public string PartialDobExists => (PartialDob == null || PartialDob.IsEmpty()) ? null : "exists";

        public bool SearchParamsExist =>
            !string.IsNullOrEmpty(GivenName) ||
            !string.IsNullOrEmpty(FamilyName) ||
            !string.IsNullOrEmpty(IdFilter) ||
            !string.IsNullOrEmpty(TBServiceCode) ||
            !string.IsNullOrEmpty(Postcode) ||
            CountryId != null ||
            SexId != null ||
            !(PartialDob == null || PartialDob.IsEmpty()) ||
            !(PartialNotificationDate == null || PartialNotificationDate.IsEmpty());
    }
}
