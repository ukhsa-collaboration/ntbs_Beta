using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Projections
{
    public class NotificationForHomePage
    {
        [Display(Name = "NTBS Id")]
        public int NotificationId { get; set; }
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Notification date")]
        public DateTime? NotificationDate { get; set; }
        [Display(Name = "Given name")]
        public string GivenName { get; set; }
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }
        [Display(Name = "TB Service")]
        public string TbServiceName { get; set; }
        [Display(Name = "Case Manager")]
        public string CaseManagerName { get; set; }

        [Display(Name = "Name")]
        public string FullName =>
            string.Join(", ", new[] { FamilyName?.ToUpper(), GivenName }.Where(s => !string.IsNullOrEmpty(s)));

        [Display(Name = "Date created")]
        public string FormattedCreationDate => CreationDate.ConvertToString();
        [Display(Name = "Date notified")]
        public string FormattedNotificationDate => NotificationDate.ConvertToString();
    }
}
