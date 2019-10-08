using System;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{    
    [Owned]
    public class Episode : ModelBase
    {        
        [MaxLength(200)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string Consultant { get; set; }

        [Display(Name = "Case Manager")]
        [MaxLength(200)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string CaseManager { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.TBServiceIsRequired)]
        public string TBServiceCode { get; set; }
        public virtual TBService TBService { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.HospitalIsRequired)]
        public Guid? HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }
    }
}
