using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{    
    [Owned]
    public class Episode
    {        
        [MaxLength(200)]
        [RegularExpression(ValidationRegexes.ValidCharactersForName, ErrorMessage = ValidationMessages.NameFormat)]
        public string Consultant { get; set; }

        [MaxLength(200)]
        [RegularExpression(ValidationRegexes.ValidCharactersForName, ErrorMessage = ValidationMessages.NameFormat)]
        public string CaseManager { get; set; }

        public string TBServiceCode { get; set; }
        public virtual TBService TBService { get; set; }

        public Guid? HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }
    }
}
