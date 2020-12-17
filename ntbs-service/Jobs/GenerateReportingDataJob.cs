using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

        protected override bool DidExecuteSuccessfully(System.Collections.Generic.IEnumerable<dynamic> resultToTest)
        {
            bool success = false;

            string serialisedResult = JsonConvert.SerializeObject(resultToTest);
            Log.Information(serialisedResult);

            if(resultToTest.Count() == 0){
                success = true;
            } else {
                throw new ApplicationException("Stored procedure did not execute successfully as result has messages, check the logs.");
            }

            return success;
        }
    }
}
