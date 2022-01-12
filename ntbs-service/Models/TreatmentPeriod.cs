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
        public DateTime? PeriodStartDate { get; }
        public DateTime? PeriodEndDate { get; private set; }

        private TreatmentPeriod(int? periodNumber, bool isTransfer, List<TreatmentEvent> treatmentEvents, DateTime? endDate = null)
        {
            PeriodNumber = periodNumber;
            IsTransfer = isTransfer;
            TreatmentEvents = treatmentEvents;
            PeriodStartDate = treatmentEvents.First().EventDate;
            PeriodEndDate = endDate ?? treatmentEvents.Last().EventDate;
        }

        public static TreatmentPeriod CreateTreatmentPeriod(int? periodNumber, TreatmentEvent treatmentEvent)
        {
            return new TreatmentPeriod(periodNumber, false, new List<TreatmentEvent> { treatmentEvent });
        }

        public static TreatmentPeriod CreateTransferPeriod(TreatmentEvent treatmentEvent)
        {
            return new TreatmentPeriod(null, true, new List<TreatmentEvent> { treatmentEvent });
        }

        public static TreatmentPeriod CreatePeriodFromEvents(int? periodNumber, IEnumerable<TreatmentEvent> treatmentEvents, DateTime? endDate = null)
        {
            return new TreatmentPeriod(periodNumber, false, treatmentEvents.ToList(), endDate);
        }

        public void AddTreatmentEvent(TreatmentEvent treatmentEvent)
        {
            TreatmentEvents.Add(treatmentEvent);
            PeriodEndDate = treatmentEvent.EventDate;
        }
    }
}
