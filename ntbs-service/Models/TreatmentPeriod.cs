using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Entities;

namespace ntbs_service.Models
{
    [NotMapped]
    public class TreatmentPeriod
    {
        public int? PeriodNumber { get; }
        public bool IsTransfer { get; }
        public List<TreatmentEvent> TreatmentEvents { get; }

        private TreatmentPeriod(int? periodNumber, bool isTransfer, TreatmentEvent treatmentEvent)
        {
            PeriodNumber = periodNumber;
            IsTransfer = isTransfer;
            TreatmentEvents = new List<TreatmentEvent> { treatmentEvent };
        }

        public static TreatmentPeriod CreateTreatmentPeriod(int? periodNumber, TreatmentEvent treatmentEvent)
        {
            return new TreatmentPeriod(periodNumber, false, treatmentEvent);
        }

        public static TreatmentPeriod CreateTransferPeriod(TreatmentEvent treatmentEvent)
        {
            return new TreatmentPeriod(null, true, treatmentEvent);
        }
    }
}
