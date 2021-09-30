using System;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using ntbs_service.DataAccess;
using Serilog;
using static ntbs_service.Jobs.StoredProcedureJobHelper;

namespace ntbs_service.Jobs
{
    public class ReportingDataRefreshJob
    {
        private readonly IExternalStoredProcedureRepository _externalStoredProcedureRepository;

        public ReportingDataRefreshJob(IExternalStoredProcedureRepository externalStoredProcedureRepository)
        {
            _externalStoredProcedureRepository = externalStoredProcedureRepository;
        }

        // PerformContext context is passed in via Hangfire Server
        public async Task Run(PerformContext context)
        {
            LogInfo(context, "Starting reporting data refresh job");

            try
            {
                LogInfo(context, "Starting migration uspGenerate");
                var stepOneResults =
                    await _externalStoredProcedureRepository.ExecuteMigrationGenerateStoredProcedure();
                AssertSuccessfulExecution(context, stepOneResults);

                LogInfo(context, "Starting specimen-matching uspGenerate");
                var stepTwoResults =
                    await _externalStoredProcedureRepository.ExecuteSpecimenMatchingGenerateStoredProcedure();
                AssertSuccessfulExecution(context, stepTwoResults);

                LogInfo(context, "Starting reporting uspGenerate");
                var stepThreeResults =
                    await _externalStoredProcedureRepository.ExecuteReportingGenerateStoredProcedure();
                AssertSuccessfulExecution(context, stepThreeResults);
            }
            catch (Exception ex)
            {
                context.WriteLine(ex.Message);
                Log.Error(ex, "Error occured during reporting data refresh job");
                throw;
            }

            LogInfo(context, "Finishing reporting data refresh job");
        }
    }
}
