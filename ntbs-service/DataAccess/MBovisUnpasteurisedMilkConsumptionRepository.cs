using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public class MBovisUnpasteurisedMilkConsumptionRepository : ItemRepository<MBovisUnpasteurisedMilkConsumption>
    {
        public MBovisUnpasteurisedMilkConsumptionRepository(NtbsContext context) : base(context) { }

        protected override DbSet<MBovisUnpasteurisedMilkConsumption> GetDbSet()
        {
            return _context.MBovisUnpasteurisedMilkConsumption;
        }

        protected override MBovisUnpasteurisedMilkConsumption GetEntityToUpdate(Notification notification, MBovisUnpasteurisedMilkConsumption mBovisUnpasteurisedMilkConsumption)
        {
            return notification.MBovisDetails.MBovisUnpasteurisedMilkConsumptions
                .Single(m => m.MBovisUnpasteurisedMilkConsumptionId == mBovisUnpasteurisedMilkConsumption.MBovisUnpasteurisedMilkConsumptionId);
        }
    }
}
