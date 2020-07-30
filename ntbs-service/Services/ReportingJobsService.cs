using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ntbs_service.Services
{
    public interface IReportingJobsService
    {
        Task RunJobX();
    }

    public class ReportingJobsService: IReportingJobsService
    {
        private readonly string _reportingDbConnectionString;

        public ReportingJobsService(IConfiguration configuration)
        {
            _reportingDbConnectionString = configuration.GetConnectionString(Constants.DbConnectionStringReporting);
        }

        public async Task RunJobX()
        {
            Log.Information("Kicking off dbo.uspGenerate");
            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                Log.Information("connection should be open now");
                await connection.ExecuteAsync("uspGenerate", commandType: CommandType.StoredProcedure);
            }
            Log.Information("dbo.uspGenerate should have finished now");
        }
    }
}
