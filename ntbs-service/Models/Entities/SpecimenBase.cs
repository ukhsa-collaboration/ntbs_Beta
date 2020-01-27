using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    [NotMapped]
    public class SpecimenBase
    {
        [Display(Name = "Lab number")]
        public string ReferenceLaboratoryNumber { get; set; }
        
        [Display(Name = "Requesting Lab")] 
        public string LaboratoryName { get; set; }    

        [Display(Name = "Specimen type")]
        public string SpecimenTypeCode { get; set; }

        [Display(Name = "Specimen date")]
        public DateTime? SpecimenDate { get; set; }

        [Display(Name = "Species")]
        public string Species { get; set; }

        [Display(Name = "NHS number")]
        public string LabNhsNumber { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime? LabBirthDate { get; set; }

        [Display(Name = "Name")]
        public string LabName { get; set; }

        [Display(Name = "Sex")]
        public string LabSex { get; set; }

        [Display(Name = "Address")]
        public string LabAddress { get; set; }

        [Display(Name = "Postcode")]
        public string LabPostcode { get; set; }

        [Display(Name = "TB Service Name")]
        public string TbServiceName { get; set; }

        public string FormattedLabDob => LabBirthDate.ConvertToString();
        public string FormattedSpecimenDate => SpecimenDate.ConvertToString();
        public string FormattedNhsNumber => NotificationFieldFormattingHelper.FormatNHSNumber(LabNhsNumber);
    }
}
