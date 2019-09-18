using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class NotificationBannerDetails
    {
        public int NotificationId { get; set; }
        public virtual PatientDetails Patient { get; set; }
        public virtual Episode Episode { get; set; }
    }
}