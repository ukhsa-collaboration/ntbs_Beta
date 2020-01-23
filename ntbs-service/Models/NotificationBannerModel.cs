using ntbs_service.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;
using System;
using ntbs_service.Models.Entities;

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
        public string Source;
        public NotificationStatus NotificationStatus;
        public string NotificationStatusString;
        public bool ShowLink = false;
        public bool ShowPadlock;
        public string RedirectPath;

        // Access level is treated as a bool for either able to edit or not. This differs from the standard PermissionLevel
        // implemented across the codebase due to there being no visual difference between no permission level and readonly
        // permission on notification banner models
        public NotificationBannerModel(Notification notification, bool showPadlock = true, bool showLink = false) {
            NotificationId = notification.NotificationId.ToString();
            SortByDate = notification.NotificationDate ?? notification.CreationDate;
            TbService = notification.TBServiceName;
            TbServiceCode = notification.Episode?.TBServiceCode;
            TbServicePHECCode = notification.Episode.TBService?.PHECCode;
            LocationPHECCode = notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode;
            CaseManager = notification.Episode.CaseManagerName;
            NhsNumber = notification.FormattedNhsNumber;
            DateOfBirth = notification.FormattedDob;
            CountryOfBirth = notification.CountryName;
            Postcode = notification.FormattedNoAbodeOrPostcodeString;
            Name = notification.FullName;
            Sex = notification.SexLabel;
            NotificationStatus = notification.NotificationStatus;
            NotificationStatusString = notification.NotificationStatusString;
            NotificationDate = notification.FormattedNotificationDate;
            Source = "ntbs";
            ShowLink = showLink;
            ShowPadlock = showPadlock;
            RedirectPath = RouteHelper.GetNotificationPath(notification.NotificationId, NotificationSubPaths.Overview);
        }
        
        public NotificationBannerModel() {}
    }
}
