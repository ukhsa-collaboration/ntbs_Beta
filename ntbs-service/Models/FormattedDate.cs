using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class FormattedDate
    {
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Year) && string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Day);
        }

        public bool HasAnyEmpty()
        {
            return string.IsNullOrEmpty(Year) || string.IsNullOrEmpty(Month) || string.IsNullOrEmpty(Day);
        }

        public bool TryConvertToDateTime(out DateTime? dateTime)
        {
            dateTime = null;

            if (IsEmpty()) 
            {
                return true;
            }
                        
            if (int.TryParse(Day, out var parsedDay) 
                && int.TryParse(Month, out var parsedMonth) 
                && int.TryParse(Year, out var parsedYear))
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
}