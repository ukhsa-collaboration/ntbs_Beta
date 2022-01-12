using System;
using System.Collections.Generic;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Projections
{
    public class NotificationForBannerModel
    {
        public int NotificationId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? NotificationDate { get; set; }
        public int? GroupId { get; set; }
        public string TbService { get; set; }
        public string TbServiceCode { get; set; }
        public string TbServicePHECCode { get; set; }
        public string LocationPHECCode { get; set; }
        public string CaseManager { get; set; }
        public int? CaseManagerId { get; set; }
        public bool? CaseManagerIsActive { get; set; }
        public string NhsNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string Postcode { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string DrugResistance { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

        public ICollection<TreatmentEvent> TreatmentEvents { get; set; }
        public bool IsPostMortemWithCorrectEvents { get; set; }

        public IEnumerable<string> PreviousTbServiceCodes { get; set; }
        public IEnumerable<string> PreviousPhecCodes { get; set; }
        public IEnumerable<string> LinkedNotificationTbServiceCodes { get; set; }
        public IEnumerable<string> LinkedNotificationPhecCodes { get; set; }
    }
}
