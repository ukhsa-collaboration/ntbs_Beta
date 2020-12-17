using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using Serilog;

namespace ntbs_service.Jobs
{
    public class StoredProcedureJobBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly string _connectionString;
        protected string _sqlString = "";
        protected object[] _parameters;

        public StoredProcedureJobBase(IConfiguration configuration, string sqlString, object[] sqlParameters = null)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(Constants.DbConnectionStringReporting);
            _sqlString = sqlString;
            _parameters = sqlParameters;
        }

        public StoredProcedureJobBase(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(Constants.DbConnectionStringReporting);
        }

        public virtual async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting stored procedure job");

            if (String.IsNullOrEmpty(_sqlString)) 
            {
                throw new ArgumentNullException(nameof(_sqlString));
            }
            
            var successfulExecution = false;
            int resultChanges = 0;

            try {
                var result = await ExecuteStoredProcedure(token);
                resultChanges = result.Count();
                successfulExecution = DidExecuteSuccessfully(result);
            }
            finally {
                Log.Information($"Finishing stored procedure job with {resultChanges} changes made, successful? {successfulExecution}.");
            }
        }

        protected virtual async Task<IEnumerable<dynamic>> ExecuteStoredProcedure(IJobCancellationToken token)
        {
            IEnumerable<dynamic> result = new List<dynamic>();    
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                result = await connection.QueryAsync(_sqlString, _parameters, null, 600, System.Data.CommandType.StoredProcedure);
            }

            return result;
        }

        protected virtual bool DidExecuteSuccessfully(IEnumerable<dynamic> resultToTest) {
            bool success = false;

            string serialisedResult = JsonConvert.SerializeObject(resultToTest);
            Log.Information(serialisedResult);
            
            // always successful as we cannot know what the expected output should be.
            success = true;

            return success;
        }
    }
}
