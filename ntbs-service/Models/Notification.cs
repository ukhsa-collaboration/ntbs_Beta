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

        public string FormattedNhsNumber {
            get {
                return CreateFormattedNhsNumberString();
            }
        }

        public string FormattedPostcode {
            get {
                return CreatePostcodeString();
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
                return CreateTimePeriodsString(SocialRiskFactors.RiskFactorDrugs);
            }
        }
        public string ImprisonmentRiskFactorTimePeriods { 
            get {
                return CreateTimePeriodsString(SocialRiskFactors.RiskFactorDrugs);
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

        public string CreatePostcodeString() {
            var postcodeNoWhiteSpace = PatientDetails.Postcode.Replace(" ", string.Empty);
            string FormattedPostcode = postcodeNoWhiteSpace.Insert(postcodeNoWhiteSpace.Length - 3, " ");
            return FormattedPostcode;
        }

        public string CreateFormattedNhsNumberString() {
            return PatientDetails.NhsNumber.ToString().Substring(0, 3) + " " + PatientDetails.NhsNumber.ToString().Substring(3, 3) 
                + " " + PatientDetails.NhsNumber.ToString().Substring(6, 4);
        }

        public string CreateTimePeriodsString(RiskFactorBase riskFactor) {
            string timeString = "";
            if(riskFactor.IsCurrent) {
                timeString = "current";
            }
            if(riskFactor.InPastFiveYears) {
                timeString = String.Join(", ", new string[] {timeString, "less than 5 years ago"});
            }
            if(riskFactor.MoreThanFiveYearsAgo) {
                timeString = String.Join(", ", new string[] {timeString, "more than 5 years ago"});
            }
            return timeString;
        }
    }
}
