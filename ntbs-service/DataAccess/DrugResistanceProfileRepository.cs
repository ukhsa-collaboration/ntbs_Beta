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
        private readonly string _specimenMatchingDbConnectionString;

        public DrugResistanceProfileRepository(IConfiguration configuration)
        {
            _specimenMatchingDbConnectionString =
                configuration.GetConnectionString(Constants.DbConnectionStringSpecimenMatching);
        }

        public async Task<Dictionary<int, DrugResistanceProfile>> GetDrugResistanceProfilesAsync()
        {
            var query = $@"
                SELECT NotificationId,
                    [New DrugResistanceProfile] AS [DrugResistanceProfile],
                    [New Species] AS [Species]
                FROM [dbo].[vwChangesToDRPSpecies]";

            using (var connection = new SqlConnection(_specimenMatchingDbConnectionString))
            {
                connection.Open();
                return (await connection.QueryAsync(query)).ToDictionary(
                    t => (int)t.NotificationId,
                    t => new DrugResistanceProfile
                    {
                        NotificationId = t.NotificationId,
                        DrugResistanceProfileString = t.DrugResistanceProfile,
                        Species = t.Species
                    });
            }
        }
    }
}
