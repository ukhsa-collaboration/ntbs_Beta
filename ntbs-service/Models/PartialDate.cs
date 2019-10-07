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
        public DateType Type { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Year) && string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Day);
        }

        public bool TryConvertToDateTime(out DateTime? dateTime)
        {
            dateTime = null;

            int parsedDay;
            int parsedMonth;
            int parsedYear;

            if(Type == DateType.Year) {
                int.TryParse(Year, out parsedYear);
                parsedMonth = 0;
                parsedDay = 1;
            } 
            if(Type == DateType.MonthYear) {

            }
            if(Type == DateType.DayMonthYear) {

            }

            if (int.TryParse(Day, out parsedDay) && int.TryParse(Month, out parsedMonth) 
                && int.TryParse(Year, out parsedYear))
            {
                try
                {
                    dateTime = new DateTime(parsedYear, parsedMonth, parsedDay);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
            return false;
        }
    }

    public enum DateType
    {
        DayMonthYear,
        MonthYear,
        Year
    }
}