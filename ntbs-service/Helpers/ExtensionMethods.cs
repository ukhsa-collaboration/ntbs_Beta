using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ntbs_service.Models;

namespace ntbs_service.Helpers
{
    public static class DateTimeExtension
    {
        public static FormattedDate ConvertToFormattedDate(this DateTime? dateTime) {

            return new FormattedDate() { Day = dateTime?.Day.ToString(), Month = dateTime?.Month.ToString(), Year = dateTime?.Year.ToString() };
        }
    }

    
}