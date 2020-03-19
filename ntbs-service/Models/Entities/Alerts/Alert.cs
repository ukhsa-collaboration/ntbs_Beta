using System;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.Entities.Alerts
{
    public abstract class Alert
    {
        public int AlertId { get; set; }
        [Display(Name = "NTBS Id")]
        public int? NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
        public DateTime CreationDate { get; set; }
        
        [Display(Name = "TB Service")]
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }
        [Display(Name = "Case Manager")]
        public string CaseManagerUsername { get; set; }
        public virtual User CaseManager { get; set; }
        public AlertStatus AlertStatus { get; set; }
        public DateTime? ClosureDate { get; set; }
        public string ClosingUserId { get; set; }
        [Display(Name = "Alert type")]
        public AlertType AlertType { get; set; }
        public virtual string ActionLink { get; }
        public virtual string Action { get; }
        public virtual bool NotDismissable  { get; }
        [Display(Name = "Case manager")] 
        public string CaseManagerFullName => CaseManager?.FullName ?? "";
        [Display(Name = "Alert date")]
        public string FormattedCreationDate => CreationDate.ConvertToString();
        [Display(Name = "TB Service")]
        public string TbServiceName => TbService?.Name;
    }

}
