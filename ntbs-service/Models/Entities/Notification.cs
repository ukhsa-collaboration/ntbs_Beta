using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class Notification : ModelBase, IHasRootEntity
    {
        public Notification()
        {
            NotificationStatus = NotificationStatus.Draft;
            PatientDetails = new PatientDetails();
            HospitalDetails = new HospitalDetails();
            SocialRiskFactors = new SocialRiskFactors();
            ClinicalDetails = new ClinicalDetails();
            PatientTBHistory = new PatientTBHistory();
            ContactTracing = new ContactTracing();
            ImmunosuppressionDetails = new ImmunosuppressionDetails();
            TravelDetails = new TravelDetails();
            VisitorDetails = new VisitorDetails();
            ComorbidityDetails = new ComorbidityDetails();
            MDRDetails = new MDRDetails();
            TestData = new TestData();
            DrugResistanceProfile = new DrugResistanceProfile();
        }

        #region DB Mapped Fields

        [Display(Name = "NTBS Id")]
        public int NotificationId { get; set; }
        [MaxLength(50)]
        public string ETSID { get; set; }
        [MaxLength(50)]
        public string LTBRPatientId { get; set; }
        [MaxLength(50)]
        public string LTBRID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        [Display(Name = "Deletion reason")]
        [MaxLength(150)]
        public string DeletionReason { get; set; }
        public int? GroupId { get; set; }
        [Display(Name = "Cluster Id")]
        public string ClusterId { get; set; }

        [Display(Name = "Notification date")]
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(@"PatientDetails.Dob == null || NotificationDate > PatientDetails.Dob", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [ValidClinicalDate]
        public DateTime? NotificationDate { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

        [NotMapped]
        public override bool? IsLegacy => LTBRID != null || ETSID != null;

        #endregion

        #region Navigation Properties

        [AssertThat("NotificationSites.Count > 0 || !ShouldValidateFull", ErrorMessage = ValidationMessages.DiseaseSiteIsRequired)]
        public virtual List<NotificationSite> NotificationSites { get; set; }
        public virtual PatientDetails PatientDetails { get; set; }
        public virtual ClinicalDetails ClinicalDetails { get; set; }
        public virtual HospitalDetails HospitalDetails { get; set; }
        public virtual PatientTBHistory PatientTBHistory { get; set; }
        public virtual ContactTracing ContactTracing { get; set; }
        public virtual SocialRiskFactors SocialRiskFactors { get; set; }
        public virtual ImmunosuppressionDetails ImmunosuppressionDetails { get; set; }
        public virtual TravelDetails TravelDetails { get; set; }
        public virtual VisitorDetails VisitorDetails { get; set; }
        public virtual DenotificationDetails DenotificationDetails { get; set; }
        public virtual ComorbidityDetails ComorbidityDetails { get; set; }
        public virtual MDRDetails MDRDetails { get; set; }
        public virtual NotificationGroup Group { get; set; }
        public virtual TestData TestData { get; set; }
        public virtual ICollection<Alert> Alerts { get; set; }
        public virtual ICollection<SocialContextVenue> SocialContextVenues { get; set; }
        public virtual ICollection<SocialContextAddress> SocialContextAddresses { get; set; }
        public virtual ICollection<TreatmentEvent> TreatmentEvents { get; set; }
        public virtual DrugResistanceProfile DrugResistanceProfile { get; set; }

        #endregion

        #region Display and Formatting methods/fields

        public string NotificationStatusString => GetNotificationStatusString();
        public string FullName => string.Join(", ", new[] { PatientDetails.FamilyName?.ToUpper(), PatientDetails.GivenName }.Where(s => !String.IsNullOrEmpty(s)));
        public string SexLabel => PatientDetails.Sex?.Label;
        public string EthnicityLabel => PatientDetails.Ethnicity?.Label;
        public string CountryName => PatientDetails.Country?.Name;
        [Display(Name = "TB Service")]
        public string TBServiceName => HospitalDetails.TBService?.Name;
        public string HospitalName => HospitalDetails.Hospital?.Name;
        public string IsPostMortemYesNo => ClinicalDetails.IsPostMortem.FormatYesNo();
        public string IsSymptomatic => ClinicalDetails.IsSymptomatic.FormatYesNo();
        public string PreviouslyHadTBYesNo => (PatientTBHistory.PreviouslyHadTB).FormatYesNo();
        public string UkBornYesNo => PatientDetails.UkBorn.FormatYesNo();
        public string IsShortCourseYesNo => ClinicalDetails.IsShortCourseTreatment.FormatYesNo();
        public string HasRecentVisitor => VisitorDetails.HasVisitor.FormatYesNo();
        public string HasRecentTravel => TravelDetails.HasTravel.FormatYesNo();
        public string FormattedNhsNumber => FormatNhsNumberString();
        public IList<string> FormattedAddress => (PatientDetails.Address ?? string.Empty).Split(Environment.NewLine);
        public string FormattedNoAbodeOrPostcodeString => PatientDetails.NoFixedAbode ? "No fixed abode" : PatientDetails.Postcode?.Trim();
        public string FormattedOccupationString => PatientDetails?.FormatOccupationString();
        public string SitesOfDiseaseList => CreateSitesOfDiseaseString();
        public string DrugRiskFactorTimePeriods => CreateTimePeriodsString(SocialRiskFactors.RiskFactorDrugs);
        public string HomelessRiskFactorTimePeriods => CreateTimePeriodsString(SocialRiskFactors.RiskFactorHomelessness);
        public string ImprisonmentRiskFactorTimePeriods => CreateTimePeriodsString(SocialRiskFactors.RiskFactorImprisonment);
        public string DaysFromOnsetToTreatment => FormatNullableDateDifference(ClinicalDetails.TreatmentStartDate, ClinicalDetails.SymptomStartDate);
        public string DaysFromOnsetToFirstPresentation => FormatNullableDateDifference(ClinicalDetails.FirstPresentationDate, ClinicalDetails.SymptomStartDate);
        public string DaysFromFirstPresentationToTBServicePresentation => FormatNullableDateDifference(ClinicalDetails.TBServicePresentationDate, ClinicalDetails.FirstPresentationDate);
        public string DaysFromTBServicePresentationToDiagnosis => FormatNullableDateDifference(ClinicalDetails.DiagnosisDate, ClinicalDetails.TBServicePresentationDate);
        public string DaysFromDiagnosisToTreatment => FormatNullableDateDifference(ClinicalDetails.TreatmentStartDate, ClinicalDetails.DiagnosisDate);
        public string BCGVaccinationStateAndYear => FormatStateAndYear(ClinicalDetails.BCGVaccinationState, ClinicalDetails.BCGVaccinationYear);
        public string MDRTreatmentStateAndDate => FormatBooleanStateAndDate(ClinicalDetails.IsMDRTreatment, ClinicalDetails.MDRTreatmentStartDate);
        public string FormattedSymptomStartDate => ClinicalDetails.SymptomStartDate.ConvertToString();
        public string FormattedPresentationToAnyHealthServiceDate => ClinicalDetails.FirstPresentationDate.ConvertToString();
        public string FormattedPresentationToTBServiceDate => ClinicalDetails.TBServicePresentationDate.ConvertToString();
        public string FormattedDiagnosisDate => ClinicalDetails.DiagnosisDate.ConvertToString();
        public string FormattedHomeVisitDate => ClinicalDetails.FirstHomeVisitDate.ConvertToString();
        public string FormattedDotOfferedState => GetFormattedDotOfferedState();
        public string FormattedHealthcareSettingState => GetFormattedHealthcareSettingState();
        public string FormattedHomeVisitState => GetFormattedHomeVisitState();
        public string FormattedTreatmentStartDate => ClinicalDetails.TreatmentStartDate.ConvertToString();
        public string FormattedDeathDate => ClinicalDetails.DeathDate.ConvertToString();
        public string FormattedDob => PatientDetails.Dob.ConvertToString();
        [Display(Name = "Date created")]
        public string FormattedCreationDate => CreationDate.ConvertToString();
        [Display(Name = "Date notified")]
        public string FormattedNotificationDate => NotificationDate.ConvertToString();
        public string HIVTestState => ClinicalDetails.HIVTestState?.GetDisplayName() ?? string.Empty;
        public string LocalAuthorityName => PatientDetails?.PostcodeLookup?.LocalAuthority?.Name;
        public string ResidencePHECName => PatientDetails?.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHEC?.Name;
        public string TreatmentPHECName => HospitalDetails.TBService?.PHEC?.Name;
        public int? AgeAtNotification => GetAgeAtTimeOfNotification();
        public string MDRCaseCountryName => MDRDetails.Country?.Name;
        public bool HasBeenNotified => NotificationStatus == NotificationStatus.Notified || NotificationStatus == NotificationStatus.Legacy;
        public string LegacyId => LTBRID ?? ETSID;
        public bool TransferRequestPending => Alerts?.Any(x => x.AlertType == AlertType.TransferRequest && x.AlertStatus == AlertStatus.Open) == true;
        public bool OverOneYearOld => NotificationDate != null && DateTime.Now > NotificationDate.Value.AddYears(1);
        public string FormattedDenotificationDate => DenotificationDetails?.DateOfDenotification.ConvertToString();
        public string DenotificationReasonString => DenotificationDetails?.Reason.GetDisplayName() + 
                                                    (DenotificationDetails?.Reason == DenotificationReason.Other ? $" - {DenotificationDetails?.OtherDescription}" : "");
        public bool IsMdr => ClinicalDetails.IsMDRTreatment == true || DrugResistanceProfile.DrugResistanceProfileString == "RR/MDR/XDR";

        private string GetNotificationStatusString()
        {
            if (NotificationStatus == NotificationStatus.Draft)
            {
                return "Draft";
            }
            else if (NotificationStatus == NotificationStatus.Notified)
            {
                return "Notification";
            }
            else if (NotificationStatus == NotificationStatus.Denotified)
            {
                return "Denotified";
            }
            else if (NotificationStatus == NotificationStatus.Legacy)
            {
                return "Legacy";
            }

            throw new InvalidOperationException("Notification status is not currently set");
        }

        private static string FormatStateAndYear(Status? state, int? year)
        {
            return state?.ToString() + (year != null ? " - " + year : string.Empty);
        }

        private static string FormatBooleanStateAndDate(bool? booleanState, DateTime? date)
        {
            return booleanState.FormatYesNo() + (date != null ? " - " + date.ConvertToString() : string.Empty);
        }

        private string FormatNullableDateDifference(DateTime? date1, DateTime? date2)
        {
            var days = CalculateDaysBetweenNullableDates(date1, date2);
            switch (days)
            {
                case null:
                    return null;
                case 1:
                    return days + " day";
                default:
                    return days + " days";
            }
        }

        private static int? CalculateDaysBetweenNullableDates(DateTime? date1, DateTime? date2)
        {
            return (date1?.Date - date2?.Date)?.Days;
        }

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

        private string FormatNhsNumberString()
        {
            if (PatientDetails.NhsNumberNotKnown)
            {
                return "Not known";
            }

            return NotificationFieldFormattingHelper.FormatNHSNumber(PatientDetails.NhsNumber);
        }

        private static string CreateTimePeriodsString(RiskFactorDetails riskFactor)
        {
            var timeStrings = new List<string>();
            if (riskFactor.IsCurrent)
            {
                timeStrings.Add("current");
            }
            if (riskFactor.InPastFiveYears)
            {
                timeStrings.Add("less than 5 years ago");
            }
            if (riskFactor.MoreThanFiveYearsAgo)
            {
                timeStrings.Add("more than 5 years ago");
            }
            return string.Join(", ", timeStrings);
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

        private string GetFormattedDotOfferedState()
        {
            if (ClinicalDetails.IsDotOffered == null)
            {
                return null;
            }

            var prefix = (bool)ClinicalDetails.IsDotOffered ? "Yes" : "No";
            return ClinicalDetails.DotStatus != null
                ? $"{prefix} - {ClinicalDetails.DotStatus.GetDisplayName()}"
                : prefix;
        }
        
        private string GetFormattedHomeVisitState()
        {
            if (ClinicalDetails.HomeVisitCarriedOut == null)
            {
                return null;
            }

            var prefix = ClinicalDetails.HomeVisitCarriedOut.GetDisplayName();
            return ClinicalDetails.FirstHomeVisitDate != null
                ? $"{prefix} - {FormattedHomeVisitDate}"
                : prefix;
        }

        private string GetFormattedHealthcareSettingState()
        {
            var healthcareState = ClinicalDetails.HealthcareSetting?.GetDisplayName();
            return ClinicalDetails.HealthcareDescription != null
                ? $"{healthcareState} - {ClinicalDetails.HealthcareDescription}"
                : healthcareState;
        }
        

        #endregion

        string IHasRootEntity.RootEntityType => RootEntities.Notification;
        string IHasRootEntity.RootId => NotificationId == default ? null : NotificationId.ToString();
    }
}
