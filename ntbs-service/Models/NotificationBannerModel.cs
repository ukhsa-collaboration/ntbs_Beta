using System;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationBannerModel
    {
        public string NotificationId;
        public DateTime? SortByDate;
        public string NotificationDate;
        public string TbService;
        public string TbServiceCode;
        public string TbServicePHECCode;
        public string LocationPHECCode;
        public string CaseManager;
        public string NhsNumber;
        public string DateOfBirth;
        public string CountryOfBirth;
        public string Postcode;
        public string Name;
        public string Sex;
        public string DrugResistance;
        public string TreatmentOutcome;
        public string Source;
        public NotificationStatus NotificationStatus;
        public string NotificationStatusString;
        public bool ShowLink;
        public bool ShowPadlock;
        public string RedirectPath;
        public int? CaseManagerId;

        // Access level is treated as a bool for either able to edit or not. This differs from the standard PermissionLevel
        // implemented across the codebase due to there being no visual difference between no permission level and readonly
        // permission on notification banner models
        public NotificationBannerModel(Notification notification, bool showPadlock = false, bool showLink = false)
        {
            NotificationId = notification.NotificationId.ToString();
            SortByDate = notification.NotificationDate ?? notification.CreationDate;
            TbService = notification.HospitalDetails.TBServiceName;
            TbServiceCode = notification.HospitalDetails.TBServiceCode;
            TbServicePHECCode = notification.HospitalDetails.TBService?.PHECCode;
            LocationPHECCode = notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode;
            CaseManager = notification.HospitalDetails.CaseManagerName;
            CaseManagerId = notification.HospitalDetails.CaseManagerId;
            NhsNumber = notification.PatientDetails.FormattedNhsNumber;
            DateOfBirth = notification.PatientDetails.FormattedDob;
            CountryOfBirth = notification.PatientDetails.CountryName;
            Postcode = notification.PatientDetails.FormattedNoAbodeOrPostcodeString;
            Name = notification.PatientDetails.FullName;
            Sex = notification.PatientDetails.SexLabel;
            NotificationStatus = notification.NotificationStatus;
            NotificationStatusString = notification.NotificationStatus.GetDisplayName();
            NotificationDate = notification.FormattedNotificationDate;
            DrugResistance = notification.DrugResistanceProfile.DrugResistanceProfileString;
            TreatmentOutcome = CalculateOutcome(notification);
            Source = "ntbs";
            ShowLink = showLink;
            ShowPadlock = showPadlock;
            RedirectPath = RouteHelper.GetNotificationPath(notification.NotificationId, NotificationSubPaths.Overview);
        }

        private static string CalculateOutcome(Notification notification)
        {
            return notification.TreatmentEvents
                       .GetMostRecentTreatmentEvent()
                       ?.TreatmentOutcome
                       ?.TreatmentOutcomeType.GetDisplayName() ?? "No outcome recorded";
        }

        public NotificationBannerModel() { }
    }
}
