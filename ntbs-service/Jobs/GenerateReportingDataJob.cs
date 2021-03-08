using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
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

        /// PerformContext context is passed in via Hangfire Server
        public override async Task Run(PerformContext context, IJobCancellationToken token)
        {
            Log.Information($"Starting generate reporting data job");

            await base.Run(context, token);

            Log.Information($"Finishing generate reporting data job.");
        }

        protected override bool DidExecuteSuccessfully(System.Collections.Generic.IEnumerable<dynamic> resultToTest)
        {
            var success = false;

            var serialisedResult = JsonConvert.SerializeObject(resultToTest);
            Log.Information(serialisedResult);
            _context.WriteLine($"Result: {serialisedResult}");

            if (resultToTest.Count() == 0)
            {
                success = true;
            }
            else
            {
                throw new ApplicationException("Stored procedure did not execute successfully as result has messages, check the logs.");
            }

            return success;
        }
    }
}
