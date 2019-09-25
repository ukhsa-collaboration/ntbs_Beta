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

        /* 
        * Full Validation is done if the form is being submitted or already been submitted.
        * Since the ModelBase does not have direct access to NotificaitonStatus, 
        * this methods is used to set Validation State from ViewModel
        */ 
        public void SetFullValidation(NotificationStatus notificationStatus, bool isBeingSubmitted) 
        {
            ShouldValidateFull = isBeingSubmitted || notificationStatus == NotificationStatus.Notified;
        }
    }
}
