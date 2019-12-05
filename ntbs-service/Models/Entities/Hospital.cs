using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models.Entities
{
    public class Hospital
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid HospitalId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string CountryCode { get; set; }
        public string TBServiceCode { get; set; }

        [ForeignKey("TBServiceCode")]
        public virtual TBService TBService { get; set; }
    }
}
