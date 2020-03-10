using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Entities
{
    public partial class HospitalDetails
    {
        [Display(Name = "TB Service")]
        public string TBServiceName => TBService?.Name;
        public string HospitalName => Hospital?.Name;
        public string TreatmentPHECName => TBService?.PHEC?.Name;
    }
}
