using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    public class Notification
    {
        public Notification() {
            NotificationStatus = NotificationStatus.Draft;
        }
        
        public int NotificationId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

        public virtual PatientDetails PatientDetails { get; set; }
        public virtual ClinicalDetails ClinicalDetails { get; set; }
        public virtual List<NotificationSite> NotificationSites { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual PatientTBHistory PatientTBHistory { get; set; }
        public virtual ContactTracing ContactTracing { get; set; }
        public virtual SocialRiskFactors SocialRiskFactors { get; set; }

        public string FullName => String.Join(", ", new string[] {PatientDetails.FamilyName.ToUpper(), PatientDetails.GivenName});
        public string SexLabel => PatientDetails.Sex?.Label;
        public string EthnicityLabel => PatientDetails.Ethnicity?.Label;
        public string CountryName => PatientDetails.Country?.Name;
        public string TBServiceName => Episode.TBService?.Name;
        public string HospitalName => Episode.Hospital?.Name;
        public string IsPostMortemYesNo => TrueFalseToYesNo(ClinicalDetails.IsPostMortem);
        public string NoSampleTakenYesNo => TrueFalseToYesNo(!ClinicalDetails.NoSampleTaken);
        public string NotPreviouslyHadTBYesNo => TrueFalseToYesNo(!PatientTBHistory.NotPreviouslyHadTB);
        public string FormattedNhsNumber => FormatNhsNumberString();
        public string FormattedNoAbodeOrPostcodeString => CreateNoAbodeOrPostcodeString();
        public string SitesOfDiseaseList => CreateSitesOfDiseaseString();
        public string DrugRiskFactorTimePeriods => CreateTimePeriodsString(SocialRiskFactors.RiskFactorDrugs);
        public string HomelessRiskFactorTimePeriods => CreateTimePeriodsString(SocialRiskFactors.RiskFactorHomelessness);
        public string ImprisonmentRiskFactorTimePeriods => CreateTimePeriodsString(SocialRiskFactors.RiskFactorImprisonment);
        public int? DaysFromOnsetToTreatment => CalculateDaysBetweenNullableDates(ClinicalDetails.TreatmentStartDate, ClinicalDetails.SymptomStartDate);
        public int? DaysFromOnsetToPresentation => CalculateDaysBetweenNullableDates(ClinicalDetails.PresentationDate, ClinicalDetails.SymptomStartDate);
        public int? DaysFromPresentationToDiagnosis => CalculateDaysBetweenNullableDates(ClinicalDetails.DiagnosisDate, ClinicalDetails.PresentationDate);
        public int? DaysFromDiagnosisToTreatment => CalculateDaysBetweenNullableDates(ClinicalDetails.TreatmentStartDate, ClinicalDetails.DiagnosisDate);
        public string BCGVaccinationStateAndYear => FormatVaccintationStateAndYear(ClinicalDetails.BCGVaccinationState, ClinicalDetails.BCGVaccinationYear);
        public string FormattedSymptomStartDate => FormatDate(ClinicalDetails.SymptomStartDate);
        public string FormattedPresentationDate => FormatDate(ClinicalDetails.PresentationDate);
        public string FormattedDiagnosisDate => FormatDate(ClinicalDetails.DiagnosisDate);
        public string FormattedTreatmentStartDate => FormatDate(ClinicalDetails.TreatmentStartDate);
        public string FormattedDeathDate => FormatDate(ClinicalDetails.DeathDate);
        public string FormattedDob => FormatDate(PatientDetails.Dob);
        public int? TotalContactsIdentified => CalculateSum(ContactTracing.AdultsIdentified, ContactTracing.ChildrenIdentified);
        public int? TotalContactsScreened => CalculateSum(ContactTracing.AdultsScreened, ContactTracing.ChildrenScreened);
        public int? TotalContactsActiveTB => CalculateSum(ContactTracing.AdultsActiveTB, ContactTracing.ChildrenActiveTB);
        public int? TotalContactsLatentTB => CalculateSum(ContactTracing.AdultsLatentTB, ContactTracing.ChildrenLatentTB);
        public int? TotalContactsStartedTreatment => CalculateSum(ContactTracing.AdultsStartedTreatment, ContactTracing.ChildrenStartedTreatment);
        public int? TotalContactsFinishedTreatment => CalculateSum(ContactTracing.AdultsFinishedTreatment, ContactTracing.ChildrenFinishedTreatment);

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
