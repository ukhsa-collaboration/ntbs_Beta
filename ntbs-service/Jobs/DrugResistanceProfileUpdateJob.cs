using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class DrugResistanceProfileUpdateJob : HangfireJobContext
    {
        private const int MaxNumberOfUpdatesPerRun = 2000;

        private readonly IDrugResistanceProfileService _service;

        public DrugResistanceProfileUpdateJob(IDrugResistanceProfileService service, NtbsContext ntbsContext) : base(ntbsContext)
        {
            _service = service;
        }

        // We limit the number of Notifications that this job will update to 2,000 for performance reasons.
        // For larger numbers, the EF context was becoming very large, and the job was slowing down considerably.
        // We would only expect a large number of updates to be needed here following a large migration. In this
        // scenario, the job should be run manually to import all the data (and if this is forgotten then we
        // trigger a warning in Sentry).
        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting Drug resistance profile update job");
            var numberOfNotificationsStillNeedingUpdate = await _service.UpdateDrugResistanceProfiles(MaxNumberOfUpdatesPerRun);
            var totalNumberOfNotificationsRequiredAtStartOfRun = numberOfNotificationsStillNeedingUpdate + MaxNumberOfUpdatesPerRun;
            if (numberOfNotificationsStillNeedingUpdate > 0)
            {
                Log.Warning($"Discovered {totalNumberOfNotificationsRequiredAtStartOfRun} Notifications which require updates to their " +
                            $"Drug Resistance Profile. The Drug Resistance Profile update job will only handle {MaxNumberOfUpdatesPerRun} " +
                            $"Notifications per run, so there are still {numberOfNotificationsStillNeedingUpdate} Notifications which " +
                            "need to be updated. Trigger the job manually in order to update these Notifications.");
            }
            Log.Information($"Finishing Drug resistance profile update job");
        }
    }
}
