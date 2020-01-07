using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ISpecimenService
    {
        Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId);
    }

    public class SpecimenService : ISpecimenService
    {
        private readonly string connectionString;

        public SpecimenService(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("reporting");
        }

        public async Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId)
        {
            string query = "SELECT * FROM [dbo].[ufnGetMatchedSpecimen] (@notificationId)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var rawResult = await connection.QueryAsync<Specimen>(query, new { notificationId });
                return rawResult;
            }
        }
    }
}