using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Projections;

namespace ntbs_service.Models
{
    [NotMapped]
    public class NotificationBannerModel
    {
        public static string NTBS_SOURCE = "ntbs";

        public string NotificationId;
        public DateTime? SortByDate;
        public string NotificationDate;
        public string TbService;
        public string TbServiceCode;
        public string TbServicePHECCode;
        public string LocationPHECCode;
        public string CaseManager;
        public bool? CaseManagerIsActive;
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
        public IEnumerable<string> PreviousTbServiceCodes { get; set; }
        public IEnumerable<string> PreviousPhecCodes { get; set; }
        public IEnumerable<string> LinkedNotificationTbServiceCodes { get; set; }
        public IEnumerable<string> LinkedNotificationPhecCodes { get; set; }

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
            CaseManagerIsActive = notification.HospitalDetails.CaseManager?.IsActive;
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
            TreatmentOutcome = CalculateOutcome(notification.TreatmentEvents);
            PreviousTbServiceCodes = notification.PreviousTbServices.Select(service => service.TbServiceCode);
            PreviousPhecCodes = notification.PreviousTbServices.Select(service => service.PhecCode);
            LinkedNotificationPhecCodes =
                notification.Group?.Notifications.Select(no => no.HospitalDetails.TBService.PHECCode);
            LinkedNotificationTbServiceCodes =
                notification.Group?.Notifications.Select(no => no.HospitalDetails.TBServiceCode);
            Source = NTBS_SOURCE;
            ShowLink = showLink;
            ShowPadlock = showPadlock;
            RedirectPath = RouteHelper.GetNotificationPath(notification.NotificationId, NotificationSubPaths.Overview);
        }

        public NotificationBannerModel(NotificationForBannerModel notification, bool showPadlock = false,
            bool showLink = false)
        {
            NotificationId = notification.NotificationId.ToString();
            SortByDate = notification.NotificationDate ?? notification.CreationDate;
            TbService = notification.TbService;
            TbServiceCode = notification.TbServiceCode;
            TbServicePHECCode = notification.TbServicePHECCode;
            LocationPHECCode = notification.LocationPHECCode;
            CaseManager = notification.CaseManager;
            CaseManagerIsActive = notification.CaseManagerIsActive;
            CaseManagerId = notification.CaseManagerId;
            NhsNumber = notification.NhsNumber;
            DateOfBirth = notification.DateOfBirth;
            CountryOfBirth = notification.CountryOfBirth;
            Postcode = notification.Postcode;
            Name = notification.Name;
            Sex = notification.Sex;
            NotificationStatus = notification.NotificationStatus;
            NotificationStatusString = notification.NotificationStatus.GetDisplayName();
            NotificationDate = notification.NotificationDate.ConvertToString();
            DrugResistance = notification.DrugResistance;
            TreatmentOutcome = CalculateOutcome(notification.TreatmentEvents);
            PreviousTbServiceCodes = notification.PreviousTbServiceCodes;
            PreviousPhecCodes = notification.PreviousPhecCodes;
            LinkedNotificationTbServiceCodes = notification.LinkedNotificationTbServiceCodes;
            LinkedNotificationPhecCodes = notification.LinkedNotificationPhecCodes;
            Source = NTBS_SOURCE;
            ShowLink = showLink;
            ShowPadlock = showPadlock;
            RedirectPath = RouteHelper.GetNotificationPath(notification.NotificationId, NotificationSubPaths.Overview);
        }

        private static string CalculateOutcome(ICollection<TreatmentEvent> treatmentEvents)
        {
            return treatmentEvents.GetMostRecentTreatmentEvent()
                       ?.TreatmentOutcome
                       ?.TreatmentOutcomeType.GetDisplayName() ?? "No outcome recorded";
        }

        public NotificationBannerModel() { }
    }
}
