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
    public class ReportingDataProcessingJob : StoredProcedureJobBase
    {
        protected readonly string _reportingDatabaseConnectionString = "";
        protected readonly string _specimenMatchingConnectionString = "";
        protected readonly string _migrationConnectionString = "";

        public ReportingDataProcessingJob(IConfiguration configuration)
        : base(configuration)
        {
            _reportingDatabaseConnectionString = _configuration.GetConnectionString(Constants.DbConnectionStringReporting);
            _specimenMatchingConnectionString = _configuration.GetConnectionString(Constants.DbConnectionStringSpecimenMatching);
            _migrationConnectionString = _configuration.GetConnectionString(Constants.DbConnectionStringMigration);

            _sqlString = "";
            _parameters = null;
        }

        /// PerformContext context is passed in via Hangfire Server
        public override async Task Run(PerformContext context, IJobCancellationToken token)
        {
            _context = context;
            Log.Information($"Starting weekly reporting data processing job.");

            var results = await ExecutePopulateForestExtractStoredProcedure(token);

            DidExecuteSuccessfully(results);

            Log.Information($"Finishing weekly reporting data processing job.");
        }

        protected virtual async Task<IEnumerable<dynamic>> ExecutePopulateForestExtractStoredProcedure(IJobCancellationToken token)
        {
            IEnumerable<dynamic> result;

            using (var connection = new SqlConnection(_reportingDatabaseConnectionString))
            {
                connection.Open();
                result = await connection.QueryAsync("[dbo].[uspPopulateForestExtract]", _parameters, null, Constants.SqlServerDefaultCommandTimeOut, System.Data.CommandType.StoredProcedure);
            }

            return result;
        }

        protected override bool DidExecuteSuccessfully(IEnumerable<dynamic> resultToTest)
        {
            var serialisedResult = JsonConvert.SerializeObject(resultToTest);
            Log.Information(serialisedResult);
            _context.WriteLine($"Result: {serialisedResult}");

            if (resultToTest.Any())
            {
                throw new ApplicationException("Stored procedure did not execute successfully as result has messages, check the logs.");
            }
            return true;
        }


    }
}
