using System.Threading.Tasks;
using Hangfire;
using ntbs_service.Services;

namespace ntbs_service.Jobs
{
    public class DbScheduledJob
    {
        private readonly IReportingJobsService _dbJobsService;

        public DbScheduledJob(IReportingJobsService dbJobsService)
        {
            _dbJobsService = dbJobsService;
        }

        public async Task Run(IJobCancellationToken token)
        {
            await _dbJobsService.RunJobX();
        }
    }
}
