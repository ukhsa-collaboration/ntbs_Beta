using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.Entities
{
    [NotMapped]
    public class HomepageKpi
    {
        public float PercentPositive { get; set; }
        public float PercentResistant { get; set; }
        public float PercentHivOffered { get; set; }
        public float PercentTreatmentDelay { get; set; }
        public string Code { get; set; }
    }
}
