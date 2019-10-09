using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
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

            int parsedDay = 1;
            int parsedMonth = 1;
            int parsedYear = 1;

            bool yearOnly = false;
            bool monthAndYearOnly = false;

            if(!checkFormatIsValid(Day, Month, Year)) {
                return false;
            }

            if(string.IsNullOrEmpty(Day)) {
                if(string.IsNullOrEmpty(Month)) {
                    yearOnly = true;
                } else {
                    monthAndYearOnly = true;
                }
            }

            if(!int.TryParse(Year, out parsedYear)) {
                return false;
            }
            if(!string.IsNullOrEmpty(Month) && !int.TryParse(Month, out parsedMonth)) {
                return false;
            }
            if(!string.IsNullOrEmpty(Day) && !int.TryParse(Day, out parsedDay)) {
                return false;
            }

            try
            {
                dateTimeStart = new DateTime(parsedYear, parsedMonth, parsedDay);
                if(yearOnly) {
                    dateTimeEnd = dateTimeStart?.AddYears(1);
                }
                else if(monthAndYearOnly)
                {
                    dateTimeEnd = dateTimeStart?.AddMonths(1);
                } 
                else 
                {
                    dateTimeEnd = dateTimeStart?.AddDays(1);
                }
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public bool checkFormatIsValid(string Day, string Month, string Year) {
            if(string.IsNullOrEmpty(Year)) {
                return false;
            }
            if(string.IsNullOrEmpty(Month) && !string.IsNullOrEmpty(Day)) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}