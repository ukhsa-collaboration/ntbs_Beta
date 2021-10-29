using System;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public partial class ClinicalDetails
    {
        public string FormattedTreatmentRegimen => GetFormattedTreatmentRegimen();
        public string FormattedDotOfferedState => GetFormattedDotOfferedState();
        public string FormattedHealthcareSettingState => GetFormattedHealthcareSettingState();
        public string FormattedHomeVisitState => GetFormattedHomeVisitState();

        public string IsPostMortemYesNo => IsPostMortem.FormatYesNo();
        public string IsSymptomaticYesNo => IsSymptomatic.FormatYesNo();
        public string DaysFromOnsetToTreatment => FormatNullableDateDifference(TreatmentStartDate, SymptomStartDate);
        public string DaysFromOnsetToFirstPresentation => FormatNullableDateDifference(FirstPresentationDate, SymptomStartDate);
        public string DaysFromFirstPresentationToTBServicePresentation => FormatNullableDateDifference(TBServicePresentationDate, FirstPresentationDate);
        public string DaysFromTBServicePresentationToDiagnosis => FormatNullableDateDifference(DiagnosisDate, TBServicePresentationDate);
        public string DaysFromDiagnosisToTreatment => FormatNullableDateDifference(TreatmentStartDate, DiagnosisDate);
        public string BCGVaccinationStateAndYear => FormatStateAndYear(BCGVaccinationState, BCGVaccinationYear);

        [NotMapped]
        public bool DatesHaveBeenSet { get; set; }

        private string GetFormattedTreatmentRegimen()
        {
            if (TreatmentRegimen == null)
            {
                return "";
            }

            switch (TreatmentRegimen)
            {
                case Enums.TreatmentRegimen.MdrTreatment:
                    var displayString = TreatmentRegimen.GetDisplayName();
                    if (MDRTreatmentStartDate != null)
                    {
                        displayString += $" - from {MDRTreatmentStartDate.ConvertToString()}";
                    }
                    if (MDRExpectedTreatmentDurationInMonths != null)
                    {
                        displayString += $" for {MDRExpectedTreatmentDurationInMonths} months (expected)";
                    }
                    return displayString;
                case Enums.TreatmentRegimen.Other:
                    return TreatmentRegimenOtherDescription == null
                        ? TreatmentRegimen.GetDisplayName()
                        : $"{TreatmentRegimen.GetDisplayName()} - {TreatmentRegimenOtherDescription}";
                default:
                    return TreatmentRegimen.GetDisplayName();
            }
        }

        public string FormattedCaseManagementStatus
        {
            get
            {
                switch (EnhancedCaseManagementStatus)
                {
                    case Status.Yes:
                        return EnhancedCaseManagementLevel == 0 ? "Yes"
                            : $"Yes - Level {EnhancedCaseManagementLevel}";
                    case Status.No:
                        return "No - Level 0";
                    case Status.Unknown:
                        return "Unknown";
                    case null:
                        return "";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string FormattedSymptomStartDate => SymptomStartDate.ConvertToString();
        public string FormattedPresentationToAnyHealthServiceDate => FirstPresentationDate.ConvertToString();
        public string FormattedPresentationToTbServiceDate => TBServicePresentationDate.ConvertToString();
        public string FormattedDiagnosisDate => DiagnosisDate.ConvertToString();
        public string FormattedTreatmentStartDate => TreatmentStartDate.ConvertToString();
        public string GetHIVTestState => HIVTestState?.GetDisplayName() ?? string.Empty;

        private string GetFormattedDotOfferedState()
        {
            if (IsDotOffered == null)
            {
                return null;
            }

            return DotStatus == null
                ? IsDotOffered.GetDisplayName()
                : $"{IsDotOffered.GetDisplayName()} - {DotStatus.GetDisplayName()}";
        }

        private string GetFormattedHomeVisitState()
        {
            if (HomeVisitCarriedOut == null)
            {
                return null;
            }

            var prefix = HomeVisitCarriedOut.GetDisplayName();
            return FirstHomeVisitDate != null
                ? $"{prefix} - {FirstHomeVisitDate.ConvertToString()}"
                : prefix;
        }

        private string GetFormattedHealthcareSettingState()
        {
            var healthcareState = HealthcareSetting?.GetDisplayName();
            return HealthcareDescription != null
                ? $"{healthcareState} - {HealthcareDescription}"
                : healthcareState;
        }

        private static string FormatStateAndYear(Status? state, int? year)
        {
            return state?.ToString() + (year != null ? " - " + year : string.Empty);
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
    }
}
