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
        Task<(IEnumerable<Notification> notifications, int count)> Search(int offset, int pageSize);
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

        public async Task<(IEnumerable<Notification> notifications, int count)> Search(int offset, int numberToFetch)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var results = await connection.QueryAsync(Query, new
                {
                    Offset = offset,
                    Fetch = numberToFetch
                });

                var count = (await connection.QueryAsync<int>(CountQuery)).Single();

                return (results.Select(AsNotification), count);
            }
        }

        private static Notification AsNotification(dynamic result)
        {
            var notification = new Notification
            {
                ETSID = result.ETSID?.ToString(),
                LTBRID = result.LTBRID?.ToString(),
                NotificationStatus = NotificationStatus.Legacy,
                NotificationDate = result.NotificationDate,
                CreationDate = result.CreationDate,
                PatientDetails = ExtractPatientDetails(result)
            };
            
            return notification;
        }

        private static PatientDetails ExtractPatientDetails(dynamic notification)
        {
            return new PatientDetails
            {
                FamilyName = notification.FamilyName,
                GivenName = notification.GivenName,
                NhsNumber = notification.NhsNumber,
                Dob = notification.DateOfBirth,
                UkBorn = GetBoolValue(notification.CountryName),
                LocalPatientId = notification.LocalPatientId,
                Postcode = notification.Postcode,
                Address = notification.Line1 + " " + notification.Line2,
                EthnicityId = notification.NtbsEthnicGroupId,
                SexId = notification.NtbsSexId,
                NhsNumberNotKnown = notification.NhsNumberNotKnown == 1,
                NoFixedAbode = notification.NoFixedAbode == 1,
                LegacyCountryName = notification.BirthCountryName
            };
        }

        private static Episode ExtractEpisode(dynamic notification)
        {
            return new Episode
            {
            };
        }

        private static bool? GetBoolValue(int? value)
        {
            if (value == null)
            {
                return null;
            }
            return value == 1 ? true : false;
        }
    }
}