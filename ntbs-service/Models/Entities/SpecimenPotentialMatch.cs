using System;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public class SpecimenPotentialMatch
    {
        [Display(Name = "Notification Id")]
        public int NotificationId { get; set; }

        [Display(Name = "Date of notification")]
        public DateTime? NotificationDate { get; set; }

        [Display(Name = "NHS Number")]
        public string NtbsNhsNumber { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime? NtbsBirthDate { get; set; }

        [Display(Name = "Name")]
        public string NtbsName { get; set; }

        [Display(Name = "Sex")]
        public string NtbsSex { get; set; }

        [Display(Name = "Address")]
        public string NtbsAddress { get; set; }

        [Display(Name = "Postcode")]
        public string NtbsPostcode { get; set; }

        public string ConfidenceLevel { get; set; }

        [Display(Name = "TB Service")]
        public string TbServiceName { get; set; }

        public string FormattedDob => NtbsBirthDate.ConvertToString();
        public string FormattedNotificationDate => NotificationDate.ConvertToString();
        public string FormattedNhsNumber => NtbsNhsNumber.FormatStringToNhsNumberFormat();
    }
}
