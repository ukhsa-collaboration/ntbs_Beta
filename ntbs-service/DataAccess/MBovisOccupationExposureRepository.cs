using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public class MBovisOccupationExposureRepository : ItemRepository<MBovisOccupationExposure>
    {
        public MBovisOccupationExposureRepository(NtbsContext context) : base(context) { }

        protected override DbSet<MBovisOccupationExposure> GetDbSet()
        {
            return _context.MBovisOccupationExposures;
        }

        protected override MBovisOccupationExposure GetEntityToUpdate(Notification notification, MBovisOccupationExposure mBovisUnpasteurisedMilkConsumption)
        {
            return notification.MBovisDetails.MBovisOccupationExposures
                .Single(m => m.MBovisOccupationExposureId == mBovisUnpasteurisedMilkConsumption.MBovisOccupationExposureId);
        }
    }
}
