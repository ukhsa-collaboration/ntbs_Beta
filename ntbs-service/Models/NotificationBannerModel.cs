using ntbs_service.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;
using System;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationBannerModel
    {
        public string NotificationId;
        public DateTime SortByDate;
        public string NotificationDate;
        public string TbService;
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
        public bool FullAccess;
        public string RedirectPath;

        public NotificationBannerModel(Notification notification, bool fullAccess = true, bool showLink = false) {
            NotificationId = notification.NotificationId.ToString();
            SortByDate = notification.NotificationDate ?? notification.CreationDate;
            TbService = notification.TBServiceName;
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
            // TODO most likely need an enum for the different origins of notifications
            Source = "ntbs";
            ShowLink = showLink;
            FullAccess = fullAccess;
            RedirectPath = RouteHelper.GetNotificationPath(notification.NotificationId, NotificationSubPaths.Overview);
        }
        
        public NotificationBannerModel() {}
    }
}
