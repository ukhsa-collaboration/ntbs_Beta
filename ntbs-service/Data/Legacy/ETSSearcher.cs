using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.Data.Legacy
{
    public interface IETSSearchService
    {
        Task<IEnumerable<SearchResult>> Search(SearchRequest request);
    }

    public class ETSSearcher : IETSSearchService
    {
        const string Query = @"
SELECT LegacyId AS 'NotificationId'
	,p.NhsNumber AS 'NHSNumber'
	,p.Forename AS 'GivenNames'
	,p.Surname AS 'FamilyNames'
	,p.DateOfBirth AS 'DateOfBirth'
	,p.Sex AS 'Sex'
	,n.NotificationDate AS 'NotificationDate'
FROM ets_Patient p
JOIN ets_Notification n ON p.Id = n.PatientId
WHERE 1 = 1
";

        const string NhsNumber = @"
AND p.NhsNumber LIKE @NhsNumber
";
        private readonly string connectionString;

        public ETSSearcher(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("ets");
        }

        public async Task<IEnumerable<SearchResult>> Search(SearchRequest request)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = Query;
                if (!string.IsNullOrWhiteSpace(request.NhsNumber))
                {
                    sql += NhsNumber;
                }

                var results = await connection.QueryAsync(sql, new
                {
                    NhsNumber = $"%{request.NhsNumber}%"
                });
                return results.Select(AsSearchResult);
            }
        }

        private static SearchResult AsSearchResult(dynamic result)
        {
            return new SearchResult
            {
                Source = "ETS",
                FamilyNames = result.FamilyNames.ToString(),
                GivenNames = result.GivenNames.ToString(),
                NhsNumber = result.NHSNumber.ToString(),
                DateOfBirth = result.DateOfBirth,
                DateOfNotification = result.NotificationDate
            };
        }
    }
}
