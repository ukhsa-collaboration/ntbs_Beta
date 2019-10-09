using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [NotMapped]
    public class SearchParameters
    {
        
        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NumberFormat)]
        public string IdFilter { get; set; }
        public int? SexId { get; set; }
        public DateTime? Dob;
    }
}