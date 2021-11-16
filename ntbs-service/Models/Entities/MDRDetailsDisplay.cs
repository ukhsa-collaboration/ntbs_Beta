using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public partial class MDRDetails
    {
        public string MDRCaseCountryName => Country?.Name;

        [NotMapped]
        public bool DatesHaveBeenSet { get; set; }

        public string FormattedTreatmentStartDate => MDRTreatmentStartDate.ConvertToString();

        public string FormattedExpectedDuration => ExpectedTreatmentDurationInMonths == null ? "" : $"{ExpectedTreatmentDurationInMonths} months";
    }
}
