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
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.DataMigration;

namespace ntbs_service.Services
{

    public interface ILegacySearchService
    {
        Task<(IEnumerable<NotificationBannerModel> notifications, int count)> SearchAsync(ILegacySearchBuilder builder, int offset, int pageSize);
    }

    public class LegacySearchService : ILegacySearchService
    {
        const string SelectQueryStartTemplate = @"
            SELECT * 
            FROM Notifications n 
            LEFT JOIN Addresses addrs ON addrs.OldNotificationId = n.OldNotificationId
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.OldNotificationId
            WHERE NOT EXISTS ({0})
            ";
        private string SelectQueryStart => string.Format(SelectQueryStartTemplate, _notificationImportHelper.SelectImportedNotificationByIdQuery);

        const string CountQueryTemplate = @"
            SELECT COUNT(*)
            FROM Notifications n
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.OldNotificationId
            WHERE NOT EXISTS ({0})
            ";
        private string CountQuery => string.Format(CountQueryTemplate, _notificationImportHelper.SelectImportedNotificationByIdQuery);

        const string SelectQueryEnd = @"
            ORDER BY n.NotificationDate DESC, n.OldNotificationId
            OFFSET @Offset ROWS
            FETCH NEXT @Fetch ROWS ONLY";

        private readonly string connectionString;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly INotificationImportHelper _notificationImportHelper;
        private readonly bool LegacySearchEnabled;
        public IList<Sex> Sexes;

        public LegacySearchService(IConfiguration configuration,
                                    IReferenceDataRepository referenceDataRepository,
                                    INotificationImportHelper notificationImportHelper)
        {
            LegacySearchEnabled = configuration.GetValue<bool>(Constants.LEGACY_SEARCH_ENABLED_CONFIG_VALUE);
            connectionString = configuration.GetConnectionString("migration");
            _referenceDataRepository = referenceDataRepository;
            _notificationImportHelper = notificationImportHelper;
            Sexes = _referenceDataRepository.GetAllSexesAsync().Result;
        }

        public async Task<(IEnumerable<NotificationBannerModel> notifications, int count)> SearchAsync(ILegacySearchBuilder builder, int offset, int numberToFetch)
        {
            if (!LegacySearchEnabled)
            {
                return (new List<NotificationBannerModel> { }, 0);
            }

            IEnumerable<dynamic> results;
            int count;
            var (queryConditions, parameters) = builder.GetResult();
            parameters.Offset = offset;
            parameters.Fetch = numberToFetch;

            string fullSelectQuery = SelectQueryStart + queryConditions + SelectQueryEnd;
            string fullCountQuery = CountQuery + queryConditions;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                results = await connection.QueryAsync(fullSelectQuery, (object)parameters);
                count = (await connection.QueryAsync<int>(fullCountQuery, (object)parameters)).Single();
            }

            var notificationBannerModels = results.Select(r => (NotificationBannerModel)AsNotificationBannerAsync(r).Result);
            return (notificationBannerModels, count);
        }

        private async Task<NotificationBannerModel> AsNotificationBannerAsync(dynamic result)
        {
            TBService tbService = null;
            if (result.NtbsHospitalId is Guid guid)
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
                Postcode = (result.Postcode as string).FormatStringToPostcodeFormat(),
                NhsNumber = (result.NhsNumber as string).FormatStringToNhsNumberFormat(),
                DateOfBirth = (result.DateOfBirth as DateTime?).ConvertToString(),
                ShowPadlock = true
            };

            return notificationBannerModel;
        }
    }
}
