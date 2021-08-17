using System;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using ntbs_service.DataAccess;
using Serilog;
using static ntbs_service.Jobs.StoredProcedureJobHelper;

namespace ntbs_service.Jobs
{
    public class GenerateReportingDataJob
    {
        private readonly IExternalStoredProcedureRepository _externalStoredProcedureRepository;

        public GenerateReportingDataJob(IExternalStoredProcedureRepository externalStoredProcedureRepository)
        {
            _externalStoredProcedureRepository = externalStoredProcedureRepository;
        }

        // PerformContext context is passed in via Hangfire Server
        public async Task Run(PerformContext context)
        {
            LogInfo(context, "Starting generate reporting data job");

            try
            {
                LogInfo(context, "Starting reporting uspGenerate");
                var results = await _externalStoredProcedureRepository.ExecuteReportingGenerateStoredProcedure();
                AssertSuccessfulExecution(context, results);
            }
            catch (Exception ex)
            {
                context.WriteLine(ex.Message);
                Log.Error(ex, "Error occurred during generate reporting data job");
                throw;
            }

            LogInfo(context, "Finishing generate reporting data job.");
        }
    }
}
