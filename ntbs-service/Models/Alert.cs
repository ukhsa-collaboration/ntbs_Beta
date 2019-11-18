using ntbs_service.Models.Enums;
using System;

namespace ntbs_service.Models
{
    public abstract class Alert
    {
        public int AlertId { get; set; }
        public int? NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
        public DateTime CreationDate { get; set; }
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }
        public string CaseManagerEmail { get; set; }
        public virtual CaseManager CaseManager { get; set; }
        public Guid? HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }
        public AlertStatus AlertStatus { get; set; }
        public DateTime? ClosureDate { get; set; }
        public string ClosingUserId { get; set; }
        public AlertType AlertType { get; set; }
        public virtual string AlertReason { get; }
        public virtual string AlertLink { get; }
    }

}