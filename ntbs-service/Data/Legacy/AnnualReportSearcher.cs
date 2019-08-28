using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.Data.Legacy
{
    public interface IAnnualReportSearchService
    {
        Task<IEnumerable<SearchResult>> Search(SearchRequest request);
    }


    public class AnnualReportSearcher : IAnnualReportSearchService
    {
        readonly string query = @"
SELECT id AS 'NotificationId'
	,nhsnumber AS 'NHSNumber'
	,forename AS 'GivenNames'
	,surname AS 'FamilyNames'
	,dob AS 'DateOfBirth'
	,sex AS 'Sex'
	,caserepdate AS 'NotificationDate'
FROM AnnualReportSample
WHERE 1 = 1
";

        readonly string nhsNumber = @"
AND nhsnumber LIKE @NhsNumber
";

        readonly string connectionString;

        public AnnualReportSearcher(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("annualReport");
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
                Source = "Annual Report",
                FamilyNames = result.FamilyNames.ToString(),
                GivenNames = result.GivenNames.ToString(),
                NhsNumber = result.NHSNumber.ToString(),
                DateOfBirth = DateTime.Parse(result.DateOfBirth),
                DateOfNotification = DateTime.Parse(result.NotificationDate)
            };
        }
    }
}
