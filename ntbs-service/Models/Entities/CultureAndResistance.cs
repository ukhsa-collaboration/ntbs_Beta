using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    [NotMapped]
    public class CultureAndResistance
    {
        public int NotificationId { get; set; }

        [Display(Name = "Culture positive")]
        public string CulturePositive { get; set; }

        [Display(Name = "Species")]
        public string Species { get; set; }

        [Display(Name = "Earliest specimen date")]
        public DateTime? EarliestSpecimenDate { get; set; }

        [Display(Name = "Drug Resitance")]
        public string DrugResistanceProfile { get; set; }

        [Display(Name = "Isoniazid")]
        public string Isoniazid { get; set; }

        [Display(Name = "Rifampicin")]
        public string Rifampicin { get; set; }

        [Display(Name = "Pyrazinamide")]
        public string Pyrazinamide { get; set; }

        [Display(Name = "Ethambutol")]
        public string Ethambutol { get; set; }

        [Display(Name = "Aminoglycoside")]
        public string Aminoglycoside { get; set; }

        [Display(Name = "Quinolone")]
        public string Quinolone { get; set; }

        [Display(Name = "MDR")]
        public string MDR { get; set; }

        [Display(Name = "XDR")]
        public string XDR { get; set; }

        public string FormattedEarliestSpecimenDate => EarliestSpecimenDate.ConvertToString();
    }
}
