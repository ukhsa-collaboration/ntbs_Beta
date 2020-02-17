using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public class TreatmentEventRepository : ItemRepository<TreatmentEvent>
    {
        public TreatmentEventRepository(NtbsContext context) : base(context) { }

        protected override DbSet<TreatmentEvent> GetDbSet()
        {
            return _context.TreatmentEvent;
        }

        protected override TreatmentEvent GetEntityToUpdate(Notification notification, TreatmentEvent treatmentEvent)
        {
            return notification.TreatmentEvents
                .Single(s => s.TreatmentEventId == treatmentEvent.TreatmentEventId);
        }
    }
}
