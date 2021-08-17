using System;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using ntbs_service.DataAccess;
using Serilog;
using static ntbs_service.Jobs.StoredProcedureJobHelper;

namespace ntbs_service.Jobs
{
    public class ReportingDataProcessingJob
    {
        private readonly IExternalStoredProcedureRepository _externalStoredProcedureRepository;

        public ReportingDataProcessingJob(IExternalStoredProcedureRepository externalStoredProcedureRepository)
        {
            _externalStoredProcedureRepository = externalStoredProcedureRepository;
        }

        // PerformContext context is passed in via Hangfire Server
        public async Task Run(PerformContext context)
        {
            LogInfo(context, "Starting reporting data processing job");

            try
            {
                LogInfo(context, "Starting reporting uspPopulateForestExtract");
                var results = await _externalStoredProcedureRepository.ExecutePopulateForestExtractStoredProcedure();
                AssertSuccessfulExecution(context, results);
            }
            catch (Exception ex)
            {
                context.WriteLine(ex.Message);
                Log.Error(ex, "Error occured during reporting data processing job");
                throw;
            }

            LogInfo(context, "Finishing reporting data processing job");
        }
    }
}
