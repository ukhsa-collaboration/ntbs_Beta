using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public class MBovisExposureToKnownCaseRepository : ItemRepository<MBovisExposureToKnownCase>
    {
        public MBovisExposureToKnownCaseRepository(NtbsContext context) : base(context) { }

        protected override DbSet<MBovisExposureToKnownCase> GetDbSet()
        {
            return _context.MBovisExposureToKnownCase;
        }

        protected override MBovisExposureToKnownCase GetEntityToUpdate(Notification notification, MBovisExposureToKnownCase mBovisExposureToKnownCase)
        {
            return notification.MBovisDetails.MBovisExposureToKnownCases
                .Single(m => m.MBovisExposureToKnownCaseId == mBovisExposureToKnownCase.MBovisExposureToKnownCaseId);
        }
    }
}
