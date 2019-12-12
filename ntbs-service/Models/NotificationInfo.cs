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
                Name = notification.FullName,
                Dob = notification.FormattedDob,
                NhsNumber = notification.FormattedNhsNumber,
                DrugResistance = null, //  TODO NTBS-384: Add reference to drug resistance profile
                Sex = notification.SexLabel,
                Postcode = notification.FormattedNoAbodeOrPostcodeString
            };
        }
    }
}
