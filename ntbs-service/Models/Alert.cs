using ntbs_service.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models
{
    public abstract class Alert
    {
        public int AlertId { get; set; }
        [Display(Name = "Notification Id")]
        public int? NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
        public DateTime CreationDate { get; set; }
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }
        public string CaseManagerEmail { get; set; }
        public virtual CaseManager CaseManager { get; set; }
        public AlertStatus AlertStatus { get; set; }
        public DateTime? ClosureDate { get; set; }
        public string ClosingUserId { get; set; }
        [Display(Name = "Alert type")]
        public AlertType AlertType { get; set; }
        public virtual string AlertReason { get; }
        public virtual string ActionLink { get; }
        [Display(Name = "Actions")]
        public virtual string Action { get; }
        [Display(Name = "Alert date")]
        public string FormattedCreationDate => FormatDate(CreationDate);
        [Display(Name = "Case manager")]
        public string CaseManagerFullName => CaseManager?.FullName;
        [Display(Name = "TB Service")]
        public string TbServiceName => TbService?.Name;
        public string CaseManagerAndTbService => string.Join(", ", CaseManagerFullName, TbServiceName);

        private string FormatDate(DateTime? date)
        {
            return date?.ToString("dd-MMM-yyyy");
        }
    }

}