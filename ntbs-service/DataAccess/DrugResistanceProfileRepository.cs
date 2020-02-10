using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public interface IDrugResistanceProfileRepository
    {
        Task<Dictionary<int, DrugResistanceProfile>> GetDrugResistanceProfilesAsync();
    }
    
    public class DrugResistanceProfileRepository : IDrugResistanceProfileRepository
    {
        private readonly string _reportingDbConnectionString;

        public DrugResistanceProfileRepository(IConfiguration configuration)
        {
            _reportingDbConnectionString = configuration.GetConnectionString("reporting");
        }
        
        public async Task<Dictionary<int, DrugResistanceProfile>> GetDrugResistanceProfilesAsync()
        {
            var query = $@"
                SELECT NotificationId,
                    [New DrugResistanceProfile] AS [DrugResistanceProfile],
                    [New Species] AS [Species]
                FROM [dbo].[vwChangesToDRPSpecies]";

            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                return (await connection.QueryAsync(query)).ToDictionary(
                    t => (int) t.NotificationId,
                    t => new DrugResistanceProfile
                    {
                        DrugResistanceProfileString = t.DrugResistanceProfile,
                        Species = t.Species
                    });
            }
        }
    }
}
