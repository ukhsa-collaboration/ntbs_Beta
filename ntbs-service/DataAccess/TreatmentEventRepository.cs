using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public interface ITreatmentEventRepository : IItemRepository<TreatmentEvent>
    {
        Task UpdateStartingEvent(Notification notification, ClinicalDetails clinicalDetails);
    }

    public class TreatmentEventRepository : ItemRepository<TreatmentEvent>, ITreatmentEventRepository
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

        public async Task UpdateStartingEvent(Notification notification, ClinicalDetails clinicalDetails)
        {
            var startingEvent = notification.TreatmentEvents.SingleOrDefault(t => t.IsStartingEvent);
            if (startingEvent != null)
            {
                NotificationHelper.SetStartingEventDate(startingEvent, clinicalDetails);
                await UpdateAsync(notification, startingEvent);
            }
        }
    }
}
