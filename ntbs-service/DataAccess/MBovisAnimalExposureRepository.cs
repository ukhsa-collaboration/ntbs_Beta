using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public class MBovisAnimalExposureRepository : ItemRepository<MBovisAnimalExposure>
    {
        public MBovisAnimalExposureRepository(NtbsContext context) : base(context) { }

        protected override DbSet<MBovisAnimalExposure> GetDbSet()
        {
            return _context.MBovisAnimalExposure;
        }

        protected override MBovisAnimalExposure GetEntityToUpdate(Notification notification, MBovisAnimalExposure mBovisAnimalExposure)
        {
            return notification.MBovisDetails.MBovisAnimalExposures
                .Single(m => m.MBovisAnimalExposureId == mBovisAnimalExposure.MBovisAnimalExposureId);
        }
    }
}
