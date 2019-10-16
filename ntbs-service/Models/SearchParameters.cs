using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [NotMapped]
    [AtLeastOneProperty("IdFilter", "FamilyName", ErrorMessage = ValidationMessages.SupplyAParameter)]
    public class SearchParameters
    {
        
        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NumberFormat)]
        public string IdFilter { get; set; }
        public int? SexId { get; set; }
        public int? CountryId { get; set; }
        public string TBServiceCode { get; set; }
        public PartialDate PartialDob { get; set; }
        public PartialDate PartialNotificationDate { get; set; }

        [MinLength(2)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string FamilyName { get; set; }

        [MinLength(2)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string GivenName { get; set; }

        public bool SearchParamsExist => 
            !string.IsNullOrEmpty(GivenName) || 
            !string.IsNullOrEmpty(FamilyName) || 
            !string.IsNullOrEmpty(IdFilter) ||
            !string.IsNullOrEmpty(TBServiceCode) ||
            CountryId != null ||
            SexId != null || 
            !(PartialDob == null || PartialDob.IsEmpty()) ||
            !(PartialNotificationDate == null || PartialNotificationDate.IsEmpty());
    }
}