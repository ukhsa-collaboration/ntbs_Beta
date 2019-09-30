using System;
using System.Collections.Generic;
using ntbs_service.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationBannerModel
    {
        public int NotificationId;
        public string DateOfNotification;
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
        public string NotificationOrigination;
        public NotificationStatus NotificationStatus;
        public string NotificationStatusString;
        public bool FullPermissions;
        public bool SearchPage;

        public NotificationBannerModel(Notification notification, bool isSearchPage = false) {
            NotificationId = notification.NotificationId;
            if (notification.NotificationStatus == Enums.NotificationStatus.Draft) {
                DateOfNotification = notification.FormattedCreationDate;
            } else {
                DateOfNotification = notification.FormattedSubmissionDate;
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
            FullPermissions = true;
            NotificationOrigination = "ntbs";
            SearchPage = isSearchPage;
        }
    }
}