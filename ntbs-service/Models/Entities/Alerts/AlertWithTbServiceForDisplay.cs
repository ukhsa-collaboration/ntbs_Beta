using System;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts
{
    public class AlertWithTbServiceForDisplay
    {
        public int AlertId { get; set; }
        [Display(Name = "NTBS Id")]
        public int? NotificationId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool NotDismissable  { get; set; }
        [Display(Name = "Alert date")]
        public string FormattedCreationDate => CreationDate.ConvertToString();
        [Display(Name = "Alert type")]
        public AlertType AlertType { get; set; }
        public string ActionLink { get; set; }
        public string Action { get; set; }
        public string TbServiceCode { get; set; }
        [Display(Name =  "TB Service")]
        public string TbServiceName { get; set; }
        [Display(Name =  "Case Manager")]
        public string CaseManagerName { get; set; }
    }

}
