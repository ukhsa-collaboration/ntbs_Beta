using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Entities;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationInfo
    {
        public string Name { get; set; }
        public string Dob { get; set; }
        public string NhsNumber { get; set; }
        public string DrugResistance { get; set; }
        public string Sex { get; set; }
        public string Postcode { get; set; }

        public static NotificationInfo CreateFromNotification(Notification notification)
        {
            return new NotificationInfo
            {
                Name = notification.PatientDetails.FullName,
                Dob = notification.PatientDetails.FormattedDob,
                NhsNumber = notification.PatientDetails.FormattedNhsNumber,
                DrugResistance = notification.DrugResistanceProfile.DrugResistanceProfileString,
                Sex = notification.PatientDetails.SexLabel,
                Postcode = notification.PatientDetails.FormattedNoAbodeOrPostcodeString
            };
        }
    }
}
