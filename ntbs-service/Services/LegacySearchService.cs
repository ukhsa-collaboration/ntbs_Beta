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
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    
    public interface ILegacySearchService
    {
        Task<(IEnumerable<NotificationBannerModel> notifications, int count)> SearchAsync(ILegacySearchBuilder builder, int offset, int pageSize);
    }

    public class LegacySearchService : ILegacySearchService
    {
        const string Query = @"
            SELECT * 
            FROM Notifications n 
            LEFT JOIN Addresses addrs ON addrs.OldNotificationId = n.OldNotificationId
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.OldNotificationId
            ";
            
        const string QueryEnd = @"
            ORDER BY n.NotificationDate DESC, n.OldNotificationId
            OFFSET @Offset ROWS
            FETCH NEXT @Fetch ROWS ONLY";

        const string CountQuery = @"
            SELECT COUNT(*)
            FROM Notifications n
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.OldNotificationId
            WHERE n.OldNotificationId = '100-1' OR n.GroupId = '100-1' AND n.Source = 'LTBR' OR dmg.NhsNumber = '100-1'";

        private readonly string connectionString;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IConfiguration _configuration;
        public IList<Sex> Sexes;

        public LegacySearchService(IConfiguration configuration, IReferenceDataRepository referenceDataRepository)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("migration");
            _referenceDataRepository = referenceDataRepository;
            Sexes = _referenceDataRepository.GetAllSexesAsync().Result;
        }

        public async Task<(IEnumerable<NotificationBannerModel> notifications, int count)> SearchAsync(ILegacySearchBuilder builder, int offset, int numberToFetch)
        {
            if (!_configuration.GetValue<bool>(Constants.LEGACY_SEARCH_ENABLED_CONFIG_VALUE))
            {
                return (new List<NotificationBannerModel> {}, 0);
            }

            IEnumerable<dynamic> results;
            int count;
            var (sqlQuery, parameters) = builder.GetResult();
            parameters.Offset = offset;
            parameters.Fetch = numberToFetch;
            
            object parametersObject = parameters;
            string fullQuery = Query + sqlQuery + QueryEnd;
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                results = await connection.QueryAsync(fullQuery, parametersObject);

                count = (await connection.QueryAsync<int>(CountQuery)).Single();
            }
            
            var notificationBannerModels = results.Select(r => (NotificationBannerModel)AsNotificationBannerAsync(r).Result);
            return (notificationBannerModels, count);
        }

        private async Task<NotificationBannerModel> AsNotificationBannerAsync(dynamic result)
        {
            TBService tbService = null;
            if(result.NtbsHospitalId is Guid guid)
            {
                tbService = await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid); 
            }
            var notificationBannerModel = new NotificationBannerModel
            {
                NotificationId = result.OldNotificationId,
                NotificationStatus = NotificationStatus.Legacy,
                NotificationStatusString = "Legacy",
                NotificationDate = (result.NotificationDate as DateTime?).ConvertToString(),
                Source = result.Source,
                Sex = Sexes.Where(s => s.SexId == result.NtbsSexId)?.Single().Label,
                SortByDate = result.NotificationDate,
                Name = result.FamilyName.ToUpper() + ", " + result.GivenName,
                CountryOfBirth = result.BirthCountryName,
                TbService = tbService?.Name,
                TbServiceCode = tbService?.Code,
                Postcode = result.Postcode,
                NhsNumber = (result.NhsNumber as string).FormatStringToNhsNumberFormat(),
                DateOfBirth = (result.DateOfBirth as DateTime?).ConvertToString(),
                FullAccess = true
            };
            
            return notificationBannerModel;
        }
    }
}