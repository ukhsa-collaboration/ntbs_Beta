using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class PartialDate
    {
        public DateTime Date { get; set; }
        public DateType Type { get; set; }

    }

    public enum DateType
    {
        DayMonthYear,
        MonthYear,
        Year
    }
}