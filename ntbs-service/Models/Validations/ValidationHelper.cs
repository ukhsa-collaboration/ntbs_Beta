﻿using System;

namespace ntbs_service.Models.Validations
{
    public static class ValidationMessages
    {
        public const string ValidDate = "Please enter a valid date";
        public const string ValidYear = "Please enter a valid year";
        public static string TodayOrEarlier(string name) => $"{name} must be today or earlier";
        public static string DateValidityRangeStart(string name, string startDate) => $"{name} must not be before {startDate}";

        public static string ValidYearLaterThanBirthYear(int birthYear)
        {
            return $"Should be later than birth year ({birthYear})";
        }

        #region Shared
        public const string StandardStringFormat = "Can only contain letters and the symbols ' - . ,";
        public const string StandardStringWithNumbersFormat = "Can only contain letters, numbers and the symbols ' - . ,";
        public const string StringWithNumbersAndForwardSlashFormat = "Can only contain letters, numbers and the symbols ' - . , /";
        public const string MinTwoCharacters = "Enter at least 2 characters";
        public const string InvalidCharacter = "Invalid character found";
        public const string NumberFormat = "Can only contain digits 0-9";
        public const string PositiveNumbersOnly = "Please enter a positive value";
        public const string InvalidDate = "Invalid date selection";
        public const string YearIfMonthRequired = "Year and month must be provided if a day has been provided";
        public const string YearRequired = "A year must be provided";
        public const string SupplyAParameter = "Please supply at least one of these fields";
        public const string ValidYearRange = "Year must be provided between {1} and {2}";

        #endregion

        #region Patient Details
        public const string NhsNumberFormat = "NHS Number can only contain digits 0-9";
        public const string NhsNumberLength = "NHS Number needs to be 10 digits long";
        public const string FamilyNameIsRequired = "Family Name is a mandatory field";
        public const string GivenNameIsRequired = "Given Name is a mandatory field";
        public const string BirthDateIsRequired = "Date of birth is a mandatory field";
        public const string SexIsRequired = "Sex is a mandatory field";
        public const string EthnicGroupIsRequired = "Ethnic Group is a mandatory field";
        public const string NHSNumberIsRequired = "NHS number is a mandatory field";
        public const string BirthCountryIsRequired = "Country of Birth is a mandatory field";
        public const string PostcodeIsRequired = "Postcode is a mandatory field";
        public const string PostcodeIsNotValid = "Postcode is not valid";
        public const string YearOfUkEntryMustBeAfterDob = "Year of entry to the UK must be after patient's date of birth.";
        public const string YearOfUkEntryMustNotBeInFuture = "Year of entry to the UK must be no later than this year.";
        #endregion

        #region Clinical Details
        public const string RiskFactorSelection = "At least one field should be selected";
        public const string DiseaseSiteIsRequired = "Please choose at least one site of disease";
        public const string DiseaseSiteOtherIsRequired = "Other Field is a mandatory field";
        public const string SampleTakenIsRequired = "Sample taken is a mandatory field";
        public const string DeathDateIsRequired = "Date of death is a mandatory field";
        public const string DiagnosisDateIsRequired = "Diagnosis date is a mandatory field";
        public const string ValidTreatmentOptions = "Short course and MDR treatment cannot both be true";
        public const string ShortTreatmentIsRequired = "Short course treatment cannot be Yes if MDR treatment is Yes";
        public const string MDRIsRequired = "MDR treatment cannot be Yes if Short course treatment is Yes";
        public const string MDRDateIsRequired = "RR/MDR/XDR treatment date is a mandatory field";
        public const string BCGYearIsRequired = "BCG Year of vaccination is a mandatory field";
        #endregion

        #region Contact Tracing
        public const string ContactTracingAdultsScreened = "Must not be greater than the number of adults identified";
        public const string ContactTracingChildrenScreened = "Must not be greater than the number of children identified";
        public const string ContactTracingAdultsLatentTB = "Must not be greater than number of adults screened minus those with active TB";
        public const string ContactTracingChildrenLatentTB = "Must not be greater than number of children screened minus those with active TB";
        public const string ContactTracingAdultsActiveTB = "Must not be greater than number of adults screened minus those with latent TB";
        public const string ContactTracingChildrenActiveTB = "Must not be greater than equal to than number of children screened minus those with latent TB";
        public const string ContactTracingAdultStartedTreatment = "Must not be greater than number of adults with latent TB";
        public const string ContactTracingChildrenStartedTreatment = "Must not be greater than number of children with latent TB";
        public const string ContactTracingAdultsFinishedTreatment = "Must not be greater than number of adults the started treatment";
        public const string ContactTracingChildrenFinishedTreatment = "Must not be greater than number of children the started treatment";
        #endregion

        #region Hospital Details
        public const string TBServiceIsRequired = "TB Service is a mandatory field";
        public const string TBServiceCantChange = "TB Service cannot be changed for non-draft notifications";
        public const string HospitalIsRequired = "Hospital is a mandatory field";
        public const string HospitalMustBelongToSelectedTbSerice = "Hospital must belong to selected TB Service";
        public const string NotificationDateIsRequired = "Notification Date is a mandatory field";
        public const string NotificationDateShouldBeLaterThanDob = "Notification date must be later than date of birth";
        public const string CaseManagerMustBeAllowedForSelectedTbService = "Case Manager must be allowed for selected TB Service";

        #endregion

        #region Travel History
        public const string TravelOrVisitTotalNumberOfCountriesRequired = "Please supply total number of countries";
        public const string TotalNumberOfCountriesVisitedFromGreaterThanInputNumber = "Number of countries entered exceeds total number of countries visited from";
        public const string TotalNumberOfCountriesTravelledToGreaterThanInputNumber = "Number of countries entered exceeds total number of countries travelled to";
        public const string TravelMostRecentCountryRequired = "Please supply most recent country visited";
        public const string VisitMostRecentCountryRequired = "Please supply most recent country visited from";
        public const string TravelTotalDurationWithinLimit = "Total duration of travel must not exceed 24 months";
        public const string VisitTotalDurationWithinLimit = "Total duration of visits must not exceed 24 months";
        public const string TravelUniqueCountry = "Multiple visits to same country - record as single period of travel";
        public const string VisitUniqueCountry = "Multiple visits from same country - record as single visit";
        public const string TravelIsChronological = "Travel must be recorded in chronological order";
        public const string VisitIsChronological = "Visits must be recorded in chronological order";
        public const string TravelOrVisitDurationHasCountry = "Duration cannot be added without a corresponding country";
        public const string VisitCountryRequiresDuration = "Please supply a duration for visit";
        public const string TravelCountryRequiresDuration = "Please supply a duration for travel";
        #endregion

        #region Immunosuppression
        public const string ImmunosuppressionDetailRequired = "Please supply immunosuppression other details";
        public const string ImmunosuppressionTypeRequired = "At least one field must be selected";
        #endregion

        #region Denotify
        public const string DenotificationDateAfterNotification = "Date of denotification must be after the date of notification";
        public const string DenotificationDateLatestToday = "Date of denotification cannot be later than today";
        public const string DenotificationReasonRequired = "Please supply a reason for denotification";
        public const string DenotificationReasonOtherRequired = "Please supply additional details for the denotification reason";
        #endregion

        #region Previous History
        public const string PreviousTBDiagnosisYear = "Please enter a valid year. Year of diagnosis cannot be after 2000";
        public const string TBHistoryIsRequired = "Year of previous TB diagnosis is a mandatory field";
        #endregion
    }

    public static class ValidDates
    {
        public const string EarliestBirthDate = "01/01/1900";
        public const string EarliestClinicalDate = "01/01/2010";
        public const int EarliestYear = 1900;
    }

    public static class ValidationRegexes
    {
        public const string CharacterValidation = @"[a-zA-Z \-,.']+";
        public const string CharacterValidationWithNumbers = @"[0-9a-zA-z \-,.']+";
        public const string CharacterValidationWithNumbersForwardSlash = @"[0-9a-zA-Z \/\-,.']+";
        public const string CharacterValidationWithNumbersForwardSlashAndNewLine = @"[0-9a-zA-Z \/\-,.'\n\r]+";
        public const string CharacterValidationWithNumbersForwardSlashExtended = @"[0-9a-zA-Z \/\-,.'`#&+;:$_()\\\[\]=\*\?]+";
    }
}
