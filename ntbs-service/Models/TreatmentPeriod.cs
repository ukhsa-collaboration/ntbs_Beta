using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ntbs_service.Models.Entities;

namespace ntbs_service.Models
{
    [NotMapped]
    public class TreatmentPeriod
    {
        public int? PeriodNumber { get; }
        public bool IsTransfer { get; }
        public List<TreatmentEvent> TreatmentEvents { get; }
        public DateTime? PeriodStartDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }

        private TreatmentPeriod(int? periodNumber, bool isTransfer, IEnumerable<TreatmentEvent> treatmentEvents)
        {
            PeriodNumber = periodNumber;
            IsTransfer = isTransfer;
            TreatmentEvents = treatmentEvents.ToList();
        }

        public static TreatmentPeriod CreateTreatmentPeriod(int? periodNumber, TreatmentEvent treatmentEvent)
        {
            return new TreatmentPeriod(periodNumber, false, new List<TreatmentEvent> { treatmentEvent });
        }

        public static TreatmentPeriod CreateTransferPeriod(TreatmentEvent treatmentEvent)
        {
            return new TreatmentPeriod(null, true, new List<TreatmentEvent> { treatmentEvent });
        }

        public static TreatmentPeriod CreatePeriodFromEvents(int? periodNumber, IEnumerable<TreatmentEvent> treatmentEvents)
        {
            return new TreatmentPeriod(periodNumber, true, treatmentEvents);
        }
    }
}
