using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    [Owned]
    public class ComorbidityDetails : ModelBase
    {
        public Status? DiabetesStatus { get; set; }
        public Status? HepatitisBStatus { get; set; }
        public Status? HepatitisCStatus { get; set; }
        public Status? LiverDiseaseStatus { get; set; }
        public Status? RenalDiseaseStatus { get; set; }
    }
}