using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace ntbs_service.Jobs
{
    public class ReportingDataRefreshJob : StoredProcedureJobBase
    {
        protected readonly string _reportingDatabaseConnectionString = "";
        protected readonly string _specimenMatchingConnectionString = "";
        protected readonly string _migrationConnectionString = "";

        public ReportingDataRefreshJob(IConfiguration configuration)
        : base(configuration)
        {
            _reportingDatabaseConnectionString = _configuration.GetConnectionString(Constants.DbConnectionStringReporting);
            _specimenMatchingConnectionString = _configuration.GetConnectionString(Constants.DbConnectionStringSpecimenMatching);
            _migrationConnectionString = _configuration.GetConnectionString(Constants.DbConnectionStringMigration);

            this._sqlString = "";
            this._parameters = null;
        }

        /// PerformContext context is passed in via Hangfire Server
        public override async Task Run(PerformContext context, IJobCancellationToken token)
        {
            this._context = context;
            Log.Information($"Starting reporting data refresh job.");

            try
            {
                var stepOneResults = await ExecuteReportingLabSpecimanStoredProcedure(token);
                var stepTwoResults = await ExecuteSpecimenMatchingGenerateStoredProcedure(token);
                var stepThreeResults = await ExecuteReportingGenerateStoredProcedure(token);
                var stepFourResults = await ExecuteMigrationGenerateStoredProcedure(token);

                var allResults = new List<dynamic>();
                allResults.AddRange(stepOneResults);
                allResults.AddRange(stepTwoResults);
                allResults.AddRange(stepThreeResults);
                allResults.AddRange(stepFourResults);

                var success = this.DidExecuteSuccessfully(allResults);
            }
            catch (Exception ex)
            {
                this._context.WriteLine(ex.Message);
                Log.Error(ex, "Error occured during reporting data refresh job.");
                throw;
            }

            Log.Information($"Finishing reporting data refresh job.");
        }

        protected virtual async Task<IEnumerable<dynamic>> ExecuteReportingLabSpecimanStoredProcedure(IJobCancellationToken token)
        {
            IEnumerable<dynamic> result = new List<dynamic>();

            using (var connection = new SqlConnection(_reportingDatabaseConnectionString))
            {
                connection.Open();
                result = await connection.QueryAsync("[dbo].[uspLabSpecimen]", _parameters, null, Constants.SqlServerDefaultCommandTimeOut, System.Data.CommandType.StoredProcedure);
            }

            return result;
        }

        protected virtual async Task<IEnumerable<dynamic>> ExecuteSpecimenMatchingGenerateStoredProcedure(IJobCancellationToken token)
        {
            IEnumerable<dynamic> result = new List<dynamic>();

            using (var connection = new SqlConnection(_specimenMatchingConnectionString))
            {
                connection.Open();
                result = await connection.QueryAsync("[dbo].[uspGenerate]", _parameters, null, Constants.SqlServerDefaultCommandTimeOut, System.Data.CommandType.StoredProcedure);
            }

            return result;
        }

        protected virtual async Task<IEnumerable<dynamic>> ExecuteReportingGenerateStoredProcedure(IJobCancellationToken token)
        {
            IEnumerable<dynamic> result = new List<dynamic>();

            using (var connection = new SqlConnection(_reportingDatabaseConnectionString))
            {
                connection.Open();
                result = await connection.QueryAsync("[dbo].[uspGenerate]", _parameters, null, Constants.SqlServerDefaultCommandTimeOut, System.Data.CommandType.StoredProcedure);
            }

            return result;
        }

        protected virtual async Task<IEnumerable<dynamic>> ExecuteMigrationGenerateStoredProcedure(IJobCancellationToken token)
        {
            IEnumerable<dynamic> result = new List<dynamic>();

            using (var connection = new SqlConnection(_migrationConnectionString))
            {
                connection.Open();
                result = await connection.QueryAsync("[dbo].[uspGenerate]", _parameters, null, Constants.SqlServerDefaultCommandTimeOut, System.Data.CommandType.StoredProcedure);
            }

            return result;
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
