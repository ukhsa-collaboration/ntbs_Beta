using System.Threading.Tasks;
using Hangfire;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class DrugResistanceProfileUpdateJob
    {
        private readonly IDrugResistanceProfilesService _service;

        public DrugResistanceProfileUpdateJob(IDrugResistanceProfilesService service)
        {
            _service = service;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting Drug resistance profile update job");
            await _service.UpdateDrugResistanceProfiles();
            Log.Information($"Finishing Drug resistance profile update job");
        }
    }
}
