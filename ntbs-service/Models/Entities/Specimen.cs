using System;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public class Specimen
    {
        public int NotificationId { get; set; }
        public string ReferenceLaboratoryNumber { get; set; }
        public string SpecimenTypeCode { get; set; }
        public DateTime? SpecimenDate { get; set; }
        public string LaboratoryName { get; set; }
        public string Isoniazid { get; set; }
        public string Rifampicin { get; set; }
        public string Pyrazinamide { get; set; }
        public string Ethambutol { get; set; }
        public string Aminoglycocide { get; set; }
        public string Quinolone { get; set; }
        public string MDR { get; set; }
        public string XDR { get; set; }
        public string ReferenceLaboratory { get; set; }
        public string Species { get; set; }
        public string PatientNhsNumber { get; set; }
        public DateTime? PatientBirthDate { get; set; }
        public string PatientName { get; set; }
        public string PatientSex { get; set; }
        public string PatientAddress { get; set; }
        public string PatientPostcode { get; set; }
        public string FormattedSpecimenDate => SpecimenDate.ConvertToString();
        public string FormattedPatientDob => PatientBirthDate.ConvertToString();

    }
}
