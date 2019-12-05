using ntbs_service.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationBannerModel
    {
        public int NotificationId;
        public string SortByDate;
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
        public string Origin;
        public NotificationStatus NotificationStatus;
        public string NotificationStatusString;
        public bool ShowLink = false;
        public bool FullAccess;
        public string RedirectPath;

        public NotificationBannerModel(Notification notification, bool fullAccess = true, bool showLink = false) {
            NotificationId = notification.NotificationId;
            if (notification.NotificationStatus == NotificationStatus.Draft) {
                SortByDate = notification.FormattedCreationDate;
            } else {
                SortByDate = notification.FormattedSubmissionDate;
            }
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
            Origin = "ntbs";
            ShowLink = showLink;
            FullAccess = fullAccess;
            RedirectPath = RouteHelper.GetNotificationPath(NotificationId, NotificationSubPaths.Overview);
        }
    }
}
