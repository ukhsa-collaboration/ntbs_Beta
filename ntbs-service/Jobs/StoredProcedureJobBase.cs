using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hangfire;
using Microsoft.Extensions.Configuration;
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

            var result = await ExecuteStoredProcedure(token);
            int resultChanges = result.Count();
            
            Log.Information($"Finishing stored procedure job with {resultChanges} changes made.");
        }

        protected virtual async Task<IEnumerable<dynamic>> ExecuteStoredProcedure(IJobCancellationToken token)
        {
            IEnumerable<dynamic> result = new List<dynamic>();    
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                result = await connection.QueryAsync(_sqlString, _parameters, null, null, System.Data.CommandType.StoredProcedure);
            }

            return result;
        }
    }
}
