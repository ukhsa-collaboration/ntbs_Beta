using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Enums;
using ntbs_service.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ntbs_service.Services
{
    
    public interface ILegacySearchService
    {
        Task<(IEnumerable<NotificationBannerModel> notifications, int count)> Search(int offset, int pageSize);
    }

    public class LegacySearchService : ILegacySearchService
    {
        const string Query = @"
            SELECT *
            FROM Notifications n 
            LEFT JOIN Addresses addrs ON addrs.OldNotificationId = n.OldNotificationId
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.OldNotificationId
            ORDER BY n.NotificationDate DESC, n.OldNotificationId
            OFFSET @Offset ROWS
            FETCH NEXT @Fetch ROWS ONLY";

        const string CountQuery = @"
            SELECT COUNT(*)
            FROM Notifications n";

        private readonly IAuthorizationService _authorizationService;    

        private readonly string connectionString;

        public LegacySearchService(IConfiguration _configuration, IAuthorizationService authorizationService)
        {
            connectionString = _configuration.GetConnectionString("migration");
            _authorizationService = authorizationService;
        }

        public async Task<(IEnumerable<NotificationBannerModel> notifications, int count)> Search(int offset, int numberToFetch)
        {
            IEnumerable<dynamic> results;
            int count;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                results = await connection.QueryAsync(Query, new
                {
                    Offset = offset,
                    Fetch = numberToFetch
                });

                count = (await connection.QueryAsync<int>(CountQuery)).Single();
            }
            return (results.Select(AsNotificationBanner), count);
        }

        private static NotificationBannerModel AsNotificationBanner(dynamic result)
        {
            var notification = new NotificationBannerModel
            {
                NotificationId = result.OldNotificationId,
                NotificationStatus = NotificationStatus.Legacy,
                NotificationStatusString = "Legacy",
                NotificationDate = FormatDate(result.NotificationDate),
                Source = result.Source,
                // Sex = result.NtbsSexId, // fix
                SortByDate = result.NotificationDate,
                Name = result.FamilyName.ToUpper() + ", " + result.GivenName,
                CountryOfBirth = result.BirthCountryName,
                // TB SERVICE
                Postcode = result.Postcode,
                NhsNumber = FormatNhsNumberString(result.NhsNumber),
                DateOfBirth = FormatDate(result.DateOfBirth),
                // FullAccess authorisation

            };
            
            return notification;
        }

        private static string FormatDate(DateTime? date)
        {
            return date?.ToString("dd MMM yyyy");
        }

        private static string FormatNhsNumberString(string nhsNumber)
        {
            if (nhsNumber == null)
            {
                return "Not known";
            }
            if (string.IsNullOrEmpty(nhsNumber))
            {
                return string.Empty;
            }
            return string.Join(" ",
                nhsNumber.Substring(0, 3),
                nhsNumber.Substring(3, 3),
                nhsNumber.Substring(6, 4)
            );
        }
    }
}