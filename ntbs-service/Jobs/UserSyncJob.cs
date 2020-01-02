using System.Threading.Tasks;
using Hangfire;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class UserSyncJob
    {
        private readonly IAdImportService _adImportService;

        public UserSyncJob(IAdImportService adImportService)
        {
            _adImportService = adImportService;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting user sync job");
            await _adImportService.RunCaseManagerImportAsync();
            Log.Information($"Finishing user sync job");
        }
    }
}
