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
        Task<IEnumerable<NotificationBannerModel>> Search(string notificationId);
    }

    public class LegacySearchService : ILegacySearchService
    {
        const string Query = @"
            SELECT *
            FROM Notifications n 
            LEFT JOIN Addresses addrs ON addrs.OldNotificationId = n.OldNotificationId
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.OldNotificationId";

        private readonly IAuthorizationService _authorizationService;    

        private readonly string connectionString;

        private readonly ClaimsPrincipal User;
        public LegacySearchService(IConfiguration _configuration, IAuthorizationService authorizationService, ClaimsPrincipal user)
        {
            connectionString = _configuration.GetConnectionString("migration");
            _authorizationService = authorizationService;
            User = user;
        }

        public async Task<IEnumerable<NotificationBannerModel>> Search(string notificationId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var results = await connection.QueryAsync(Query, new
                {
                    NotificationId = $"{notificationId}"
                });

                return results.Select(AsNotification).CreateNotificationBanners(User, _authorizationService);
            }
        }

        private static Notification AsNotification(dynamic result)
        {
            var notification = new Notification
            {
                NotificationDate = result.NotificationDate,
                CreationDate = result.CreationDate,
                PatientDetails = ExtractPatientDetails(result)
            };
            
            return notification;
        }

        private static NotificationBannerModel AsNotificationBannerModel(dynamic result)
        {
            var notification = new Notification
            {
                NotificationDate = result.NotificationDate,
                CreationDate = result.CreationDate,
                PatientDetails = ExtractPatientDetails(result)
            };
            
            var notificationBannerModel = new NotificationBannerModel(notification);
            return notificationBannerModel;
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
                    OccupationId = notification.NtbsOccupationId,
                    OccupationOther = notification.NtbsOccupationFreeText
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