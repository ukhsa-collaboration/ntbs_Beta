using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [NotMapped]
    public class SearchParameters
    {
        
        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NumberFormat)]
        public string IdFilter { get; set; }
        public int? SexId { get; set; }
        public PartialDate PartialDob { get; set; }

        [MinLength(2)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string FamilyName { get; set; }

        [MinLength(2)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string GivenName { get; set; }

        public bool SearchParamsExist => !string.IsNullOrEmpty(GivenName) || !string.IsNullOrEmpty(FamilyName) || !string.IsNullOrEmpty(IdFilter) || 
                                          SexId != null || (PartialDob == null ? false : !PartialDob.IsEmpty());
    }
}