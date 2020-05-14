namespace ntbs_service.Models.Validations
{
    public static class ValidationMessages
    {
        public static string ValueInvalid(string name) => $"Value is not valid for {name}";
        public static string InvalidDate(string name) => $"{name} does not have a valid date selection";
        public static string InvalidYear(string name) => $"{name} has an invalid year";
        public static string TodayOrEarlier(string name) => $"{name} must be today or earlier";
        public static string DateValidityRangeStart(string name, string startDate) => 
            $"{name} must not be before {startDate}";
        public static string ValidYearLaterThanBirthYear(string name, int birthYear) =>
            $"{name} should be later than birth year ({birthYear})";

        #region Shared
        
        public const string InvalidYearForAttribute = "{0} has an invalid year";
        public const string StandardStringFormat = "{0} can only contain letters and the symbols ' - . ,";
        public const string StandardStringWithNumbersFormat = "{0} can only contain letters, numbers and the symbols ' - . ,";
        public const string StringWithNumbersAndForwardSlashFormat = "{0} can only contain letters, numbers and the symbols ' - . , /";
        public const string MinTwoCharacters = "Enter at least 2 characters";
        public const string InvalidCharacter = "Invalid character found in {0}";
        public const string NumberFormat = "{0} can only contain digits 0-9";
        public const string NumberHyphenAndSpaceFormat = "{0} can only contain digits 0-9, the symbol - and spaces";
        public const string PositiveNumbersOnly = "Please enter a positive value";
        public const string YearIfMonthRequired = "Year and month must be provided if a day has been provided";
        public const string YearRequired = "A year must be provided";
        public const string SupplyAParameter = "Please supply at least one of these fields";
        public const string SupplyAModelParameter = "Please supply at least one of {0} fields";
        public const string Mandatory = "{0} is a mandatory field";
        public const string RequiredEnter = "Please enter {0}";
        public const string RequiredSelect = "Please select {0}";
        public const string NotValid = "{0} is not valid";
        public const string DateShouldBeLaterThanDob = "{0} must be later than date of birth";
        public const string DateShouldBeLaterThanDobYear = "{0} must be later than date of birth year";
        public const string DateShouldBeLaterThanNotification = "{0} must be after the date of notification";
        public const string BeforeCurrentYear = "{0} must be the current year or earlier";
        public const string IdDoesNotMatchNtbsRecord = "The NTBS ID does not match an existing ID in the system";
        public const string IdNotAvailableInNtbs = "Details not available in NTBS";

        #endregion

        #region Patient Details
        public const string NhsNumberLength = "{0} needs to be 10 digits long";
        public const string InvalidNhsNumber = "This {0} is not valid. Confirm you have entered it correctly";
        public const string FieldRequired = "{0} is a mandatory field";
        public const string PostcodeNotFound = "{0} is not found";
        #endregion

        #region Clinical Details

        public const string DiseaseSiteIsRequired = "Please choose at least one site of disease";
        public const string ValidTreatmentOptions = "Short course and MDR treatment cannot both be true";
        public const string MDRCantChange = "You cannot change the value of this field because an MDR Enhanced Surveillance Questionnaire exists. Please contact NTBS@phe.gov.uk";
        #endregion

        #region Test Results
        public const string InvalidTestAndSampleTypeCombination = "{0} does not match test type selected";
        public const string NoTestResult = "Please add a test result or confirm no sample was taken";

        #endregion

        #region Contact Tracing
        public const string ContactTracingAdultsScreened = "Adults screened must not be greater than the number of adults identified";
        public const string ContactTracingChildrenScreened = "Children Screened must not be greater than the number of children identified";
        public const string ContactTracingAdultsLatentTB = "Adults with latent TB must not be greater than number of adults screened minus those with active TB";
        public const string ContactTracingChildrenLatentTB = "Children with latent TB must not be greater than number of children screened minus those with active TB";
        public const string ContactTracingAdultsActiveTB = "Adults with active TB  must not be greater than number of adults screened minus those with latent TB";
        public const string ContactTracingChildrenActiveTB = "Children with active TB must not be greater than equal to than number of children screened minus those with latent TB";
        public const string ContactTracingAdultStartedTreatment = "Adults that have started treatment must not be greater than number of adults with latent TB";
        public const string ContactTracingChildrenStartedTreatment = "Children that have started treatment must not be greater than number of children with latent TB";
        public const string ContactTracingAdultsFinishedTreatment = "Adults that have finished treatment must not be greater than number of adults the started treatment";
        public const string ContactTracingChildrenFinishedTreatment = "Children that have finished treatment must not be greater than number of children the started treatment";
        #endregion

        #region Hospital Details
        public const string TBServiceCantChange = "{0} cannot be changed for non-draft notifications";
        public const string HospitalMustBelongToSelectedTbSerice = "{0} must belong to selected TB Service";
        public const string CaseManagerMustBeAllowedForSelectedTbService = "{0} must be allowed for selected TB Service";

        #endregion

        #region Travel History
        public const string TravelOrVisitTotalNumberOfCountriesRequired = "Please supply total number of countries";
        public const string TotalNumberOfCountriesVisitedFromGreaterThanInputNumber = "Number of countries entered exceeds total number of countries visited from";
        public const string TotalNumberOfCountriesTravelledToGreaterThanInputNumber = "Number of countries entered exceeds total number of countries travelled to";
        public const string TravelTotalDurationWithinLimit = "Total duration of travel must not exceed 24 months";
        public const string VisitTotalDurationWithinLimit = "Total duration of visits must not exceed 24 months";
        public const string TravelUniqueCountry = "Multiple visits to same country - record as single period of travel";
        public const string VisitUniqueCountry = "Multiple visits from same country - record as single visit";
        public const string TravelIsChronological = "Travel must be recorded in chronological order";
        public const string VisitIsChronological = "Visits must be recorded in chronological order";
        public const string TravelOrVisitDurationHasCountry = "Duration cannot be added without a corresponding country";
        #endregion

        #region Immunosuppression
        public const string ImmunosuppressionDetailRequired = "Please supply other immunosuppression details";
        public const string ImmunosuppressionTypeRequired = "At least one immunosuppression type must be selected";
        #endregion

        #region Social Context
        public const string VenueDateToShouldBeLaterThanDateFrom = "{0} must be later than date from";
        #endregion

        #region Denotify

        public const string DenotificationDateLatestToday = "Date of denotification cannot be later than today";
        public const string DenotificationReasonRequired = "Please supply a reason for denotification";
        public const string DenotificationReasonOtherRequired = "Please supply additional details for the denotification reason";
        #endregion

        #region Previous History
        public const string ValidYear = "Please enter a valid year";
        #endregion

        #region MDR Details
        public const string RelationshipToCaseIsRequired = "Please supply details of the relationship to case";
        public const string NotifiedToPheStatusIsRequired = "Please specify whether case was notified to PHE";
        public const string RelatedNotificationIdCannotBeSameAsNotificationId = "Contact's Notification ID cannot be the same as the current notification";
        public const string RelatedNotificationIdMustBeInteger = "Notification ID should consist of numbers only";
        #endregion

        #region TreatmentEvent

        public const string SubTypeDoesNotCorrespondToOutcome = "Please supply additional information for outcome value";
        // Below are not currently surfaced in the application - but adding messages if down the line import uses these.
        public const string TreatmentOutcomeRequiredForOutcome = "Please supply treatment outcome type for a treatment outcome";
        public const string TreatmentOutcomeInvalidForRestart = "Treatment outcome type is not allowed for a treatment restart";
        public const string DeathEventRequiredForPostMortemCase = "A death event is required for a post mortem case";

        #endregion

        #region TransferAlert
        public const string TransferDestinationCannotBeCurrentTbService = "{0} can not be transferred to the notification's current TB service";
        #endregion

        #region LabResults

        public const string LabResultNotificationDoesNotExist =
            "The notification ID does not exist, verify you have entered the correct ID before moving forward";
        public const string LabResultNotificationMatchNoPermission =
            "The notification does not belong to one of your services. If you believe this is the correct notification, please contact ntbs@phe.gov.uk";
        
        #endregion

        #region MBovisDetails

        public const string HasNoExposureRecords =
            "Please add a record of exposure to known TB cases or confirm no exposure has occurred";
        public const string HasNoUnpasteurisedMilkConsumptionRecords =
            "Please add a record of unpasteurised milk consumption or confirm no consumption has occurred";
        public const string HasNoOccupationExposureRecords =
            "Please add a record of occupation exposure or confirm no exposure has occurred";
        public const string HasNoAnimalExposureRecords =
            "Please add a record of occupation exposure or confirm no exposure has occurred";

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
        public const string CharacterValidationWithNumbersForwardSlashAndNewLine = @"[0-9a-zA-Z \/\-,.'\n\r]+";
        public const string CharacterValidationWithNumbersForwardSlashExtended = @"[0-9a-zA-Z \/\-,.'`#&+;:$_()\\\[\]=\*\?]+";
        public const string CharacterValidationWithNumbersForwardSlashExtendedWithNewLine = @"[0-9a-zA-Z \/\-,.'`#&+;:$_()<>\\\[\]=\*\?\n\r]+";
        public const string NumbersValidation = @"[0-9]+";
        public const string NumbersHyphenAndSpaceValidation = @"[0-9\- ]+";
        // Taken from https://stackoverflow.com/a/164994/2363767
        public const string PostcodeValidation = @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})";
    }
}
