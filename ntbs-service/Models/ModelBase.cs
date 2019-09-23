using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    public class ModelBase
    {
        [NotMapped]
        public bool ShouldValidateFull { get; set; }

        public void SetFullValidation(NotificationStatus notificationStatus, bool isBeingSubmitted) 
        {
            ShouldValidateFull = isBeingSubmitted || notificationStatus == NotificationStatus.Notified;
        }
    }
}
