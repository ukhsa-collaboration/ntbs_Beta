using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.ReferenceEntities
{
    public class TreatmentOutcome
    {
        public int TreatmentOutcomeId { get; set; }
        [Display(Name = "Outcome value")]
        public TreatmentOutcomeType TreatmentOutcomeType { get; set; }
        [Display(Name = "Additional information")]
        public TreatmentOutcomeSubType? TreatmentOutcomeSubType { get; set; }
    }
}
