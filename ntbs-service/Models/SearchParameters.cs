using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [NotMapped]
    [AtLeastOneProperty("IdFilter", "FamilyName", ErrorMessage = ValidationMessages.SupplyAParameter)]
    public class SearchParameters
    {
        
        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NumberFormat)]
        public string IdFilter { get; set; }

        [MinLength(2)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string FamilyName { get; set; }

        [MinLength(2)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string GivenName { get; set; }

        public bool SearchParamsExist => !String.IsNullOrEmpty(GivenName) || !String.IsNullOrEmpty(FamilyName) || !String.IsNullOrEmpty(IdFilter);
    }
}