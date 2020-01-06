using System;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public class CultureAndResistance
    {
        public int NotificationId { get; set; }
        public string CulturePositive { get; set; }
        public string Species { get; set; }
        public DateTime? EarliestSpecimenDate { get; set; }
        public string DrugResistanceProfile { get; set; }
        public string Isoniazid { get; set; }
        public string Rifampicin { get; set; }
        public string Pyrazinamide { get; set; }
        public string Ethambutol { get; set; }
        public string Aminoglycocide { get; set; }
        public string Quinolone { get; set; }
        public string MDR { get; set; }
        public string XDR { get; set; }

        public string FormattedEarliestSpecimenDate => EarliestSpecimenDate.ConvertToString();
    }
}