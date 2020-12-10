using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ntbs_service.Jobs
{
    public class GenerateReportingDataJob : StoredProcedureJobBase
    {
        public GenerateReportingDataJob(IConfiguration configuration)
        : base(configuration)
        {
            this._sqlString = "[dbo].[uspGenerate]";
            this._parameters = null;
        }

        public override async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting generate reporting data job");
            
            await base.Run(token);

            Log.Information($"Finishing generate reporting data job.");
        }
    }
}
