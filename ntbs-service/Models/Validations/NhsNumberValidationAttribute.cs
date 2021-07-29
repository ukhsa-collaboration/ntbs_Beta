using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Validations
{
    public class ValidNhsNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            var nhsNumber = NotificationHelper.FormatNhsNumber(value.ToString());
            var match = Regex.Match(nhsNumber, "^[0-9]+$");
            if (!match.Success)
            {
                return new ValidationResult(string.Format(ValidationMessages.NumberFormat, validationContext.DisplayName));
            }
            else if (nhsNumber.Length != 10)
            {
                return new ValidationResult(ValidationMessages.NhsNumberLength);
            }

            var monodigitNumbers = new List<string> { "0000000000", "1111111111", "9999999999" };
            if (monodigitNumbers.Contains(nhsNumber))
            {
                return new ValidationResult(ValidationMessages.NhsNumberMonodigit);
            }

            var firstDigit = nhsNumber.Substring(0, 1);
            // Scotland uses different validation and has NHS numbers starting with 0, 1 and 2. 9 is a generally used test digit for NHS numbers.
            var scottishAndTestDigits = new List<string> { "0", "1", "2", "9" };
            if (scottishAndTestDigits.Contains(firstDigit))
            {
                return null;
            }

            if (!ValidateNhsNumber(nhsNumber))
            {
                return new ValidationResult(ValidationMessages.InvalidNhsNumber);
            }

            return null;
        }

        // Adapted from https://github.com/pfwd/NHSNumber-Validation/blob/master/C%23/NHSNumberValidation.cs
        // Validation logic can be found at https://www.datadictionary.nhs.uk/data_dictionary/attributes/n/nhs/nhs_number_de.asp?shownav=1
        public bool ValidateNhsNumber(string nhsNumber)
        {
            var multiplicationTotal = 0;
            string currentString;
            int currentNumber;

            for (var i = 0; i <= 8; i++)
            {
                currentString = nhsNumber.Substring(i, 1);
                currentNumber = Convert.ToInt16(currentString);
                multiplicationTotal += currentNumber * (10 - i);
            }

            var remainder = multiplicationTotal % 11;
            var checkNumberCalculated = 11 - remainder;
            var checkDigit = Convert.ToInt16(nhsNumber.Substring(nhsNumber.Length - 1, 1));

            if (checkNumberCalculated == 11)
            {
                checkNumberCalculated = 0;
            }
            if (checkNumberCalculated == 10)
            {
                return false;
            }
            return checkNumberCalculated == checkDigit;
        }
    }
}
