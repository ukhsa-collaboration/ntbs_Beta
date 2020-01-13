using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public abstract class Alert
    {
        public int AlertId { get; set; }
        [Display(Name = "NTBS Id")]
        public int? NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
        public DateTime CreationDate { get; set; }
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        [Display(Name = "TB Service")]
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }
        [AssertThat("CaseManagerAllowedForTbService", ErrorMessage = ValidationMessages.CaseManagerMustBeAllowedForSelectedTbService)]
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
        [Display(Name = "Alert date")]
        public string FormattedCreationDate => CreationDate.ConvertToString();
        [Display(Name = "Case manager")]
        public string CaseManagerFullName => CaseManager?.FullName ?? "System";
        [Display(Name = "TB Service")]
        public string TbServiceName => TbService?.Name;

        [NotMapped]
        public bool CaseManagerAllowedForTbService
        {
            get
            {
                // If email not set, or TBService missing (ergo navigation properties not yet retrieved) pass validation
                if (string.IsNullOrEmpty(CaseManagerUsername) || TbServiceCode == null)
                {
                    return true;
                }

                if (CaseManager?.CaseManagerTbServices == null || CaseManager.CaseManagerTbServices.Count == 0)
                {
                    return false;
                }

                return CaseManager.CaseManagerTbServices.Any(c => c.TbServiceCode == TbServiceCode);
            }
        }

        [NotMapped]
        public bool Dismissable => AlertStatus == AlertStatus.Open;
    }

}
