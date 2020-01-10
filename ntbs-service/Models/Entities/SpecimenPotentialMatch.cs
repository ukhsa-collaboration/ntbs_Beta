using System;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public class SpecimenPotentialMatch
    {
        public int NotificationId { get; set; }

        public DateTime? NotificationDate { get; set; }

        [Display(Name = "Nhs number")]
        public string NtbsNhsNumber { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime? NtbsBirthDate { get; set; }

        [Display(Name = "Fullname")]
        public string NtbsName { get; set; }

        [Display(Name = "Sex")]
        public string NtbsSex { get; set; }

        [Display(Name = "Address")]
        public string NtbsAddress { get; set; }

        [Display(Name = "Postcode")]
        public string NtbsPostcode { get; set; }

        public string FormattedDob => NtbsBirthDate.ConvertToString();
    }
}
