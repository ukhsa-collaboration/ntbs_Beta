using System;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using ntbs_service.DataAccess;
using Serilog;

namespace ntbs_service.Jobs
{
    public class ReportingDataRefreshJob : StoredProcedureJobBase
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
                LogInfo(context, "Starting specimen-matching uspGenerate");
                var stepOneResults =
                    await _externalStoredProcedureRepository.ExecuteSpecimenMatchingGenerateStoredProcedure();
                CheckExecutedSuccessfully(context, stepOneResults);

                LogInfo(context, "Starting reporting uspGenerate");
                var stepTwoResults =
                    await _externalStoredProcedureRepository.ExecuteReportingGenerateStoredProcedure();
                CheckExecutedSuccessfully(context, stepTwoResults);

                LogInfo(context, "Starting migration uspGenerate");
                var stepThreeResults =
                    await _externalStoredProcedureRepository.ExecuteMigrationGenerateStoredProcedure();
                CheckExecutedSuccessfully(context, stepThreeResults);
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
