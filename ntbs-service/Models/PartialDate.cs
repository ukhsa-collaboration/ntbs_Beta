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

            bool missingMonth = false;
            bool missingDay = false;

            if(!int.TryParse(Year, out parsedYear)) {
                return false;
            } 
            if(!int.TryParse(Month, out parsedMonth)) {
                parsedMonth = 1;
                missingMonth = true;
            }
            if(!int.TryParse(Day, out parsedDay)) {
                parsedDay = 1;
                missingDay = true;
            }
            try
            {
                dateTimeStart = new DateTime(parsedYear, parsedMonth, parsedDay);
                if(!missingDay && !missingMonth) {
                    dateTimeEnd = dateTimeStart?.AddYears(1);
                }
                if(missingDay)
                {
                    dateTimeEnd = dateTimeStart?.AddDays(1);
                } 
                else if(missingMonth) 
                {
                    dateTimeEnd = dateTimeStart?.AddMonths(1);
                }
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            
            return false;
        }

         public bool tryParseDate(int year, int month, int day, out DateTime? dateRangeStart, out DateTime? dateRangeEnd) {
            try
            {
                dateRangeStart = new DateTime(year, month, day);
                dateRangeEnd = dateRangeStart?.AddDays(1);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                dateRangeStart = null;
                dateRangeEnd = null;
                return false;
            }
        }
    }
}