using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public class TestResultRepository : ItemRepository<ManualTestResult>
    {
        public TestResultRepository(NtbsContext context) : base(context) {}

        protected override int? ItemRootId(ManualTestResult item) => item.NotificationId;
        protected override string ItemRootEntity => "Notification";

        protected override DbSet<ManualTestResult> GetDbSet()
        {
            return _context.ManualTestResult;
        }

        protected override ManualTestResult GetEntityToUpdate(Notification notification, ManualTestResult testResult)
        {
            return notification.TestData.ManualTestResults
                .First(t => t.ManualTestResultId == testResult.ManualTestResultId);
        }
    }
}
