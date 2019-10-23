using ntbs_service.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationBannerModel
    {
        public int NotificationId;
        public string SortByDate;
        public string NotificationDate;
        public string TBService;
        public string CaseManager;
        public string NHSNumber;
        public string DateOfBirth;
        public string CountryOfBirth;
        public string Postcode;
        public string Name;
        public string Sex;
        public string DrugResistance;
        public string TreatmentOutcome;
        public string Origin;
        public NotificationStatus NotificationStatus;
        public string NotificationStatusString;
        public bool ShowLink = false;
        public bool IsEditable;
        public string RedirectPath;

        public NotificationBannerModel(Notification notification, bool isEditable = true) {
            NotificationId = notification.NotificationId;
            if (notification.NotificationStatus == Enums.NotificationStatus.Draft) {
                SortByDate = notification.FormattedCreationDate;
            } else {
                SortByDate = notification.FormattedSubmissionDate;
            }
            TBService = notification.TBServiceName;
            CaseManager = notification.Episode.CaseManager;
            NHSNumber = notification.FormattedNhsNumber;
            DateOfBirth = notification.FormattedDob;
            CountryOfBirth = notification.CountryName;
            Postcode = notification.FormattedNoAbodeOrPostcodeString;
            Name = notification.FullName;
            Sex = notification.SexLabel;
            NotificationStatus = notification.NotificationStatus;
            NotificationStatusString = notification.NotificationStatusString;
            // TODO most likely need an enum for the different origins of notifications
            Origin = "ntbs";
            IsEditable = isEditable;
            RedirectPath = notification.OverviewPath;
        }

        static public NotificationBannerModel WithLink(Notification notification, bool isEditable = true)
        {
            return new NotificationBannerModel(notification, isEditable)
            {
                ShowLink = true
            };
        }
    }
}