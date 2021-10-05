using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.Entities
{
    public partial class Notification
    {
        public string SitesOfDiseaseList => CreateSitesOfDiseaseString();
        [Display(Name = "Date created")] public string FormattedCreationDate => CreationDate.ConvertToString();
        [Display(Name = "Date notified")] public string FormattedNotificationDate => NotificationDate.ConvertToString();
        public int? AgeAtNotification => GetAgeAtTimeOfNotification();

        public string LegacyId
        {
            get
            {
                switch (LegacySource)
                {
                    case "LTBR":
                        return LTBRID;
                    case "ETS":
                        return ETSID;
                    default:
                        return LTBRID ?? ETSID;
                }
            }
        }

        public bool TransferRequestPending =>
            Alerts?.Any(x => x.AlertType == AlertType.TransferRequest && x.AlertStatus == AlertStatus.Open) == true;

        public bool IsLastLinkedNotificationOverOneYearOld => GetIsLastLinkedNotificationOverOneYearOld();

        public bool IsMdr => DrugResistanceHelper.IsMdr(
            DrugResistanceProfile,
            ClinicalDetails.TreatmentRegimen,
            MDRDetails.ExposureToKnownCaseStatus);

        public bool MdrDetailsEntered => DrugResistanceHelper.MdrDetailsEntered(MDRDetails.ExposureToKnownCaseStatus);

        public bool IsMBovis => DrugResistanceHelper.IsMbovis(DrugResistanceProfile, MBovisDetails);

        public bool IsMBovisQuestionnaireComplete =>
            DrugResistanceHelper.IsMBovisQuestionnaireComplete(MBovisDetails);

        public ICollection<TreatmentEvent> GetTreatmentEventsWithStartingEvent()
        {
            var treatmentEvents = TreatmentEvents.ToList();
            if (NotificationStatus == NotificationStatus.Draft && ClinicalDetails.StartingDate != null)
            {
                treatmentEvents.Add(NotificationHelper.CreateTreatmentStartEvent(this));
            }

            return treatmentEvents;
        }

        public override bool? IsLegacy => LTBRID != null || ETSID != null;

        private string CreateSitesOfDiseaseString()
        {
            if (NotificationSites == null)
            {
                return string.Empty;
            }

            var siteNames = NotificationSites.Select(ns => ns.Site)
                .Where(ns => ns != null)
                .OrderBy(SiteOrdering)
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

        private int SiteOrdering(Site site)
        {
            switch (site.SiteId)
            {
                case 1: return 1;
                case 12: return 2;
                case 13: return 3;
                case 2: return 4;
                case 3: return 5;
                case 4: return 6;
                case 5: return 7;
                case 10: return 8;
                case 11: return 9;
                case 7: return 10;
                case 8: return 11;
                case 9: return 12;
                case 6: return 13;
                case 14: return 14;
                case 15: return 15;
                case 16: return 16;
                case 17: return 17;
                default: return 18;
            }
        }
    }
}
