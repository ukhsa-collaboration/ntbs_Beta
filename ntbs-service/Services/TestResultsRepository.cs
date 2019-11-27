using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface ITestResultsRepository
    {
        Task AddTestResultAsync(ManualTestResult testResultForEdit);
        Task UpdateTestResultAsync(Notification Notification, ManualTestResult testResultForEdit);
    }

    public class TestResultsRepository : ITestResultsRepository
    {
        private readonly NtbsContext _context;

        public TestResultsRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task AddTestResultAsync(ManualTestResult testResult)
        {
            _context.ManualTestResult.Add(testResult);
            await UpdateDatabaseAsync();
        }

        public async Task UpdateTestResultAsync(Notification Notification, ManualTestResult testResult)
        {
            var entity = Notification.TestData.ManualTestResults
                .First(t => t.ManualTestResultId == testResult.ManualTestResultId);
            _context.SetValues(entity, testResult);
            await UpdateDatabaseAsync();
        }

        private async Task UpdateDatabaseAsync(AuditType auditType = AuditType.Edit)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }
    }
}
