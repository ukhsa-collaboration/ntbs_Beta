using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.DataAccess
{
    public interface IExternalStoredProcedureRepository
    {
        Task<IEnumerable<dynamic>> ExecuteSpecimenMatchingGenerateStoredProcedure();
        Task<IEnumerable<dynamic>> ExecuteReportingGenerateStoredProcedure();
        Task<IEnumerable<dynamic>> ExecuteMigrationGenerateStoredProcedure();
        Task<IEnumerable<dynamic>> ExecutePopulateForestExtractStoredProcedure();
    }

    public class ExternalStoredProcedureRepository : IExternalStoredProcedureRepository
    {
        private readonly string _reportingDatabaseConnectionString;
        private readonly string _specimenMatchingConnectionString;
        private readonly string _migrationConnectionString;

        public ExternalStoredProcedureRepository(IConfiguration configuration)
        {
            _reportingDatabaseConnectionString =
                configuration.GetConnectionString(Constants.DbConnectionStringReporting);
            _specimenMatchingConnectionString =
                configuration.GetConnectionString(Constants.DbConnectionStringSpecimenMatching);
            _migrationConnectionString = configuration.GetConnectionString(Constants.DbConnectionStringMigration);
        }

        public Task<IEnumerable<dynamic>> ExecuteSpecimenMatchingGenerateStoredProcedure() =>
            ExecuteStoredProcedure(_specimenMatchingConnectionString, "[dbo].[uspGenerate]");

        public Task<IEnumerable<dynamic>> ExecuteReportingGenerateStoredProcedure() =>
            ExecuteStoredProcedure(_reportingDatabaseConnectionString, "[dbo].[uspGenerate]");

        public Task<IEnumerable<dynamic>> ExecuteMigrationGenerateStoredProcedure() =>
            ExecuteStoredProcedure(_migrationConnectionString, "[dbo].[uspGenerate]");

        public Task<IEnumerable<dynamic>> ExecutePopulateForestExtractStoredProcedure() =>
            ExecuteStoredProcedure(_reportingDatabaseConnectionString, "[dbo].[uspPopulateForestExtract]");

        private static async Task<IEnumerable<dynamic>> ExecuteStoredProcedure(string connectionString,
            string sqlString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync(
                    sqlString,
                    commandTimeout: Constants.SqlServerDefaultCommandTimeOut,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
