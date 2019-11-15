using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ntbs_service.Models
{
    public class Alert : ModelBase
    {
        public int AlertId { get; set; }
        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
        public DateTime CreationDate { get; set; }
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }
        public string CaseManagerEmail { get; set; }
        public virtual CaseManager CaseManager { get; set; }
        public int HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }
        public AlertType AlertType { get; set; }
        public AlertStatus AlertStatus { get; set; }
        public virtual ClosureDetails ClosureDetails { get; set; }
    }
}