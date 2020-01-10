using System;
using ntbs_service.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Entities
{
    [NotMapped]
    public class Specimen
    {
        public int NotificationId { get; set; }

        [Display(Name = "Lab number")]
        public string ReferenceLaboratoryNumber { get; set; }

        [Display(Name = "Speciment Type")]
        public string SpecimenTypeCode { get; set; }

        [Display(Name = "Specimen Date")]
        public DateTime? SpecimenDate { get; set; }

        [Display(Name = "Isoniazid")]
        public string Isoniazid { get; set; }

        [Display(Name = "Rifampicin")]
        public string Rifampicin { get; set; }

        [Display(Name = "Pyrazinamide")]
        public string Pyrazinamide { get; set; }

        [Display(Name = "Ethambutol")]
        public string Ethambutol { get; set; }

        [Display(Name = "Aminoglycocide")]
        public string Aminoglycocide { get; set; }

        [Display(Name = "Quinolone")]
        public string Quinolone { get; set; }

        [Display(Name = "MDR")]
        public string MDR { get; set; }

        [Display(Name = "XDR")]
        public string XDR { get; set; }

        [Display(Name = "Species")]
        public string Species { get; set; }

        [Display(Name = "Nhs number")]
        public string LabNhsNumber { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime? LabBirthDate { get; set; }

        [Display(Name = "Fullname")]
        public string LabName { get; set; }

        [Display(Name = "Sex")]
        public string LabSex { get; set; }

        [Display(Name = "Address")]
        public string LabAddress { get; set; }

        [Display(Name = "Postcode")]
        public string LabPostcode { get; set; }

        public string FormattedSpecimenDate => SpecimenDate.ConvertToString();
        public string FormattedPatientDob => LabBirthDate.ConvertToString();

    }
}
