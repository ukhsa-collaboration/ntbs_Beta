using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public partial class Notification
    {
        public string SitesOfDiseaseList => CreateSitesOfDiseaseString();
        [Display(Name = "Date created")]
        public string FormattedCreationDate => CreationDate.ConvertToString();
        [Display(Name = "Date notified")]
        public string FormattedNotificationDate => NotificationDate.ConvertToString();
        public int? AgeAtNotification => GetAgeAtTimeOfNotification();
        public string LegacyId => !string.IsNullOrWhiteSpace(LTBRID) ? LTBRID : ETSID;
        public bool TransferRequestPending => Alerts?.Any(x => x.AlertType == AlertType.TransferRequest && x.AlertStatus == AlertStatus.Open) == true;
        public bool IsLastLinkedNotificationOverOneYearOld => GetIsLastLinkedNotificationOverOneYearOld();

        public bool IsMdr =>
            // If user-set treatment ... 
            ClinicalDetails.IsMDRTreatment 
            // ... or lab results indicate MDR, ...
            || DrugResistanceProfile.DrugResistanceProfileString == "RR/MDR/XDR"
            // ... or if there is any data entered in the MDR pages - otherwise we could be hiding record data
            || MDRDetails.MDRDetailsEntered;

        public bool IsMBovis =>
            // If the lab results point to M. bovis species ...
            string.Equals("M. bovis", DrugResistanceProfile.Species, StringComparison.InvariantCultureIgnoreCase)
            // ... or if there is any data in the M. bovis pages - otherwise we could be hiding record data
            || MBovisDetails.DataEntered;
        
        public override bool? IsLegacy => LTBRID != null || ETSID != null;

        private string CreateSitesOfDiseaseString()
        {
            if (NotificationSites == null)
            {
                return string.Empty;
            }

            var siteNames = NotificationSites.Select(ns => ns.Site)
                .Where(ns => ns != null)
                .Select(s => s.Description);
            return string.Join(", ", siteNames);
        }
        
        private bool GetIsLastLinkedNotificationOverOneYearOld()
        {
            var latestNotification = Group?.Notifications?.Last() ?? this; 
            var latestNotificationDate = latestNotification.NotificationDate ?? latestNotification.CreationDate;
            
            return DateTime.Now > latestNotificationDate.AddYears(1);
        }

        private int? GetAgeAtTimeOfNotification()
        {
            if (NotificationDate == null || PatientDetails?.Dob == null)
            {
                return null;
            }

            var notificationDate = (DateTime)NotificationDate;
            var birthDate = (DateTime)PatientDetails.Dob;

            var yearDiff = notificationDate.Year - birthDate.Year;
            if ((birthDate.Month * 100 + birthDate.Day) > (notificationDate.Month * 100 + notificationDate.Day))
            {
                yearDiff -= 1;
            }
            return yearDiff;
        }
    }
}
