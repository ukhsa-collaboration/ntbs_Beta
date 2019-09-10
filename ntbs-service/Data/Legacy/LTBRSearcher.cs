using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.Data.Legacy
{
    public interface ILTBRSearchService
    {
        Task<IEnumerable<SearchResult>> Search(SearchRequest request);
    }

    public class LTBRSearcher : ILTBRSearchService
    {
        const string query = @"
SELECT DISTINCT 'tbc' AS 'NotificationId'
	,p.pt_NHSNo AS 'NHSNumber'
	,pe.pe_Forename AS 'GivenNames'
	,pe.pe_Surname AS 'FamilyNames'
	,p.pt_DOB AS 'DateOfBirth'
	,p.pt_Sex AS 'Sex'
	,dp.dp_NotifiedDate AS 'NotificationDate'
FROM dbt_Patient p
JOIN dbt_PatientEpisode pe
	ON p.pt_PatientId = pe.pe_PatientId
JOIN dbt_DiseasePeriod dp
	ON p.pt_PatientId = dp.dp_PatientId
WHERE 1 = 1
";

        const string nhsNumber = @"
AND p.pt_NHSNo LIKE @NhsNumber
";
        private readonly string connectionString;

        public LTBRSearcher(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("ltbr");
        }

        public async Task<IEnumerable<SearchResult>> Search(SearchRequest request)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = query;
                if (!string.IsNullOrWhiteSpace(request.NhsNumber))
                {
                    sql += nhsNumber;
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
                Source = "LTBR",
                FamilyNames = result.FamilyNames.ToString(),
                GivenNames = result.GivenNames.ToString(),
                NhsNumber = result.NHSNumber.ToString(),
                DateOfBirth = result.DateOfBirth ?? new DateTime(),
                DateOfNotification = result.NotificationDate
            };
        }
    }
}
