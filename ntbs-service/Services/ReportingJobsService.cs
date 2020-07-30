using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

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
                await connection.ExecuteAsync("EXEC dbo.uspGenerate");
            }
        }
    }
}
