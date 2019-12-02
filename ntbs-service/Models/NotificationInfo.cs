using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationInfo
    {
        public string Name { get; set; }
        public string Dob { get; set; }
        public string NhsNumber { get; set; }
        public string Sex { get; set; }
        public string Postcode { get; set; }

        public static NotificationInfo CreateFromNotification(Notification notification)
        {
            return new NotificationInfo
            {
                Name = notification.FullName,
                Dob = notification.FormattedDob,
                NhsNumber = notification.FormattedNhsNumber,
                Sex = notification.SexLabel,
                Postcode = notification.FormattedNoAbodeOrPostcodeString
            };
        }
    }
}