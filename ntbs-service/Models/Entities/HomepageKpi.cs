using System.ComponentModel.DataAnnotations.Schema;

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
        public string Name { get; set; }
    }
}
