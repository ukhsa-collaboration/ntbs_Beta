using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public virtual PatientDetails PatientDetails { get; set; }
        public virtual ClinicalDetails ClinicalDetails { get; set; }
        public virtual List<NotificationSite> NotificationSites { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual PatientTBHistory PatientTBHistory { get; set; }
        public virtual ContactTracing ContactTracing { get; set; }
        public virtual SocialRiskFactors SocialRiskFactors { get; set; }

        public string FullName {
            get {
                return String.Join(", ", new string[] {PatientDetails.FamilyName.ToUpper(), PatientDetails.GivenName});
            }
        }

        public string SexLabel {
            get {
                return PatientDetails.Sex?.Label;
            }
        }

        public string EthnicityLabel {
            get {
                return PatientDetails.Ethnicity?.Label;
            }
        }

        public string CountryName {
            get {
                return PatientDetails.Country?.Name;
            }
        }

        public string TBServiceName {
            get {
                return Episode.TBService?.Name;
            }
        }

        public string HospitalName {
            get {
                return Episode.Hospital?.Name;
            }
        }

        public string IsPostMortemYesNo {
            get {
                return TrueFalseToYesNo(ClinicalDetails.IsPostMortem);
            }
        }

        public string NoSampleTakenYesNo {
            get {
                return TrueFalseToYesNo(!ClinicalDetails.NoSampleTaken);
            }
        }

        public string NotPreviouslyHadTBYesNo {
            get {
                return TrueFalseToYesNo(!PatientTBHistory.NotPreviouslyHadTB);
            }
        }

        public string FormattedNhsNumber {
            get {
                return FormatNhsNumberString();
            }
        }

        public string FormattedNoAbodeOrPostcodeString {
            get {
                return CreateNoAbodeOrPostcodeString();
            }
        }

        public string SitesOfDiseaseList { 
            get {
                return CreateSitesOfDiseaseString();
            } 
        }

        public string DrugRiskFactorTimePeriods { 
            get {
                return CreateTimePeriodsString(SocialRiskFactors.RiskFactorDrugs);
            }
        }
        public string HomelessRiskFactorTimePeriods { 
            get {
                return CreateTimePeriodsString(SocialRiskFactors.RiskFactorHomelessness);
            }
        }
        public string ImprisonmentRiskFactorTimePeriods { 
            get {
                return CreateTimePeriodsString(SocialRiskFactors.RiskFactorImprisonment);
            }
        }

        public int? DaysFromOnsetToTreatment {
            get {
                return CalculateDaysBetweenNullableDates(ClinicalDetails.TreatmentStartDate, ClinicalDetails.SymptomStartDate);
            }
        }

        public int? DaysFromOnsetToPresentation {
            get {
                return CalculateDaysBetweenNullableDates(ClinicalDetails.PresentationDate, ClinicalDetails.SymptomStartDate);
            }
        }

        public int? DaysFromPresentationToDiagnosis {
            get {
                return CalculateDaysBetweenNullableDates(ClinicalDetails.DiagnosisDate, ClinicalDetails.PresentationDate);
            }
        }

        public int? DaysFromDiagnosisToTreatment {
            get {
                return CalculateDaysBetweenNullableDates(ClinicalDetails.TreatmentStartDate, ClinicalDetails.DiagnosisDate);
            }
        }

        public string BCGVaccinationStateAndYear {
            get {
                return FormatVaccintationStateAndYear(ClinicalDetails.BCGVaccinationState, ClinicalDetails.BCGVaccinationYear);
            }
        }

        public string FormattedSymptomStartDate {
            get {
                return FormatDate(ClinicalDetails.SymptomStartDate);
            }
        }

        public string FormattedPresentationDate {
            get {
                return FormatDate(ClinicalDetails.PresentationDate);
            }
        }

        public string FormattedDiagnosisDate {
            get {
                return FormatDate(ClinicalDetails.DiagnosisDate);
            }
        }

        public string FormattedTreatmentStartDate {
            get {
                return FormatDate(ClinicalDetails.TreatmentStartDate);
            }
        }

        public string FormattedDeathDate {
            get {
                return FormatDate(ClinicalDetails.DeathDate);
            }
        }

        public string FormattedDob {
            get {
                return FormatDate(PatientDetails.Dob);
            }
        }

        public int? TotalContactsIdentified {
            get {
                return CalculateSum(ContactTracing.AdultsIdentified, ContactTracing.ChildrenIdentified);
            }
        }

        public int? TotalContactsScreened {
            get {
                return CalculateSum(ContactTracing.AdultsScreened, ContactTracing.ChildrenScreened);
            }
        }

        public int? TotalContactsActiveTB {
            get {
                return CalculateSum(ContactTracing.AdultsActiveTB, ContactTracing.ChildrenActiveTB);
            }
        }

        public int? TotalContactsLatentTB {
            get {
                return CalculateSum(ContactTracing.AdultsLatentTB, ContactTracing.ChildrenLatentTB);
            }
        }

        public int? TotalContactsStartedTreatment {
            get {
                return CalculateSum(ContactTracing.AdultsStartedTreatment, ContactTracing.ChildrenStartedTreatment);
            }
        }

        public int? TotalContactsFinishedTreatment {
            get {
                return CalculateSum(ContactTracing.AdultsFinishedTreatment, ContactTracing.ChildrenFinishedTreatment);
            }
        }

        public int? CalculateSum(int? x, int? y) {
            return x + y;
        }

        public string FormatDate(DateTime? date) {
            return date?.ToString("dd-MMM-yyyy");
        }

        public string TrueFalseToYesNo(bool x) {
            return x ? "Yes" : "No";
        }

        public string FormatVaccintationStateAndYear(Status vaccinationState, int? vaccinationYear) {
            return vaccinationState.ToString() + (vaccinationYear != null ? " - " + vaccinationYear : "");
        }

        public int? CalculateDaysBetweenNullableDates(DateTime? date1, DateTime? date2) {
            return (date1?.Date - date2?.Date)?.Days;
        }

        public string CreateSitesOfDiseaseString() {
            var siteNames = NotificationSites.Select(ns => ns.Site).Select(s => s.Description);
            return String.Join(", ", siteNames); 
        }

        public string CreateNoAbodeOrPostcodeString() {
            if(PatientDetails.NoFixedAbode) {
                return "No fixed abode";
            } else {
                var postcodeNoWhiteSpace = PatientDetails.Postcode.Replace(" ", string.Empty);
                string FormattedPostcode = postcodeNoWhiteSpace.Insert(postcodeNoWhiteSpace.Length - 3, " ");
                return FormattedPostcode;
            }
        }

        public string FormatNhsNumberString() {
            return PatientDetails.NhsNumber.ToString().Substring(0, 3) + " " + PatientDetails.NhsNumber.ToString().Substring(3, 3) 
                + " " + PatientDetails.NhsNumber.ToString().Substring(6, 4);
        }

        public string CreateTimePeriodsString(RiskFactorBase riskFactor) {
            var timeStrings = new List<string>();
            if(riskFactor.IsCurrent) {
                timeStrings.Add("current");
            }
            if(riskFactor.InPastFiveYears) {
                timeStrings.Add("less than 5 years ago");
            }
            if(riskFactor.MoreThanFiveYearsAgo) {
                timeStrings.Add("more than 5 years ago");
            }
            return String.Join(", ", timeStrings);
        }
    }
}
