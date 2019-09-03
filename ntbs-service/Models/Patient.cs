using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    public class Patient
    {
        private const string NameRegex = @"[a-zA-Z \-,.']+";

        public int PatientId { get; set; }

        [StringLength(35)]
        [RegularExpression(NameRegex, ErrorMessage = ValidationMessages.NameFormat)]
        public string FamilyName { get; set; }

        [StringLength(35)]
        [RegularExpression(NameRegex, ErrorMessage = ValidationMessages.NameFormat)]
        public string GivenName { get; set; }

        [RegularExpression(@"[0-9]+", ErrorMessage = ValidationMessages.NhsNumberFormat)]
        [StringLength(10, MinimumLength = 10, ErrorMessage= ValidationMessages.NhsNumberLength)]
        public string NhsNumber { get; set; }

        [ValidDate]
        public DateTime? Dob { get; set; }
        public bool? UkBorn { get; set; }

        public string Postcode {get; set; }

        public int? CountryId { get; set;}
        public virtual Country Country { get; set; }

        public int? EthnicityId { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }

        public int? SexId { get; set; }
        public virtual Sex Sex { get; set; }
        public bool NhsNumberNotKnown { get; set; }
        public bool NoFixedAbode { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }

}
