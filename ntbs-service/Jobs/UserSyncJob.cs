using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    // Attempts here talks about retry attempts, so we retry up to four times after the initial failure, so there are
    // five attempts total. The delay here means the first retry is after two minutes, and all subsequent retries after
    // ten minutes.
    // I have not found good docs for this, but the source code is easy enough to read:
    // https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.Core/AutomaticRetryAttribute.cs
    [AutomaticRetry(Attempts = 4, DelaysInSeconds = new []{ 120, 600 })]
    public class UserSyncJob : HangfireJobContext
    {
        private readonly IAdImportService _adImportService;

        public UserSyncJob(IAdImportService adImportService, NtbsContext ntbsContext) : base(ntbsContext)
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
