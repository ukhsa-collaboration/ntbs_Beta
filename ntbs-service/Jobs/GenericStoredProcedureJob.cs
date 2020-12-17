using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using ntbs_service.Properties;
using Serilog;

namespace ntbs_service.Jobs
{
    public class GenericStoredProcedureJob : StoredProcedureJobBase
    {
        public GenericStoredProcedureJob(IConfiguration configuration)
        : base(configuration)
        {
            var scheduledJobConfig = new ScheduledJobsConfig();
            configuration.GetSection(Constants.ScheduledJobsConfig).Bind(scheduledJobConfig);
            
            this._sqlString = scheduledJobConfig.GenericStoredProcedureNameToRun;
            this._parameters = null;
        }

        public override async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting example stored procedure job");
            
            await base.Run(token);

            Log.Information($"Finishing example stored procedure job.");
        }
    }
}
