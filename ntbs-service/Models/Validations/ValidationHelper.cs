using System;

namespace ntbs_service.Models.Validations
{
    public static class ValidationMessages
    {
        public const string ValidDate = "Please enter a valid date";
        public static string DateValidityRange(string startDate)
        {
            return $"Must be between {startDate} and {DateTime.Now.ToShortDateString()}";
        }

        public const string NameFormat = "Names can only contain letters and ' - . ,";

        public const string NhsNumberFormat = "NHS Number can only contain digits 0-9";
        public const string NhsNumberLength = "NHS Number needs to be 10 digits long";
    }

    public static class ValidDates
    {
        public const string EarliestBirthDate = "01/01/1900";

        public const string EarliestClinicalDate = "01/01/2010";
    }

    public static class ValidationRegexes
    {
        public const string ValidCharactersForName = @"[a-zA-Z \-,.']+";
    }
}