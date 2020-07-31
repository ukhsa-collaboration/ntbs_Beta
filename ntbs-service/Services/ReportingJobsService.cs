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
            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<string>("EXECUTE [dbo].[uspGenerate]");
                Log.Information($"Result came back: {result.AsList()[0]}");
            }
            Log.Information("dbo.uspGenerate should have finished now");
        }
    }
}
