using System;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [NotMapped]
    [ValidPartialDateCanConvertToDatetime(ErrorMessage = ValidationMessages.InvalidDate)]
    public class PartialDate
    {
        public string Day {get; set; }
        public string Month {get; set; }
        public string Year  {get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Year) && string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Day);
        }

        public bool TryConvertToDateTimeRange(out DateTime? dateTimeStart, out DateTime? dateTimeEnd)
        {
            dateTimeStart = null;
            dateTimeEnd = null;

            if(!ParseDateStrings(out int parsedYear, out int parsedMonth, out int parsedDay)) 
            {
                return false;
            }

            try
            {
                dateTimeStart = new DateTime(parsedYear, parsedMonth, parsedDay);

                if (!string.IsNullOrEmpty(Day)) 
                {
                    dateTimeEnd = dateTimeStart?.AddDays(1);
                }
                else if(!string.IsNullOrEmpty(Month)) 
                {
                    dateTimeEnd = dateTimeStart?.AddMonths(1);
                }
                else 
                {
                    dateTimeEnd = dateTimeStart?.AddYears(1);
                }
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public bool ParseDateStrings(out int year, out int month, out int day) {
            var canParseYear = true;
            if (!int.TryParse(Year, out int parsedYear)) {
                canParseYear = false;
            }
            if (!int.TryParse(Month, out int parsedMonth)) {
                parsedMonth = 1;
            };
            if (!int.TryParse(Day, out int parsedDay)) {
                parsedDay = 1;
            };
            year = parsedYear;
            month = parsedMonth;
            day = parsedDay;
            return canParseYear;
        }
    }
}
