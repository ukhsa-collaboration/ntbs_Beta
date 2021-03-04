using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{

    public interface ILegacySearchService
    {
        Task<(IEnumerable<NotificationBannerModel> notifications, int count)> SearchAsync(ILegacySearchBuilder builder, int offset, int pageSize, ClaimsPrincipal user);
        Task<NotificationBannerModel> GetByIdAsync(string notificationId);
    }

    public class LegacySearchService : ILegacySearchService
    {
        private string SelectQueryStart => @"SELECT * " + FromString;
        private string CountQuery => @"SELECT COUNT(*) " + FromString;

        private string FromString => $@"
            FROM MergedNotifications n 
            LEFT JOIN Addresses addrs ON addrs.OldNotificationId = n.PrimaryNotificationId
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.PrimaryNotificationId
            WHERE NOT EXISTS ({_notificationImportHelper.SelectImportedNotificationWhereIdEquals("n.PrimaryNotificationId")})
            ";

        const string SelectQueryEnd = @"
            ORDER BY CASE
                    WHEN n.NtbsHospitalId IN @editPermissionHospitals THEN 1
                    WHEN n.NtbsHospitalId NOT IN @editPermissionHospitals THEN 0
                    END DESC,
                n.NotificationDate DESC,
                n.PrimaryNotificationId DESC
            OFFSET @Offset ROWS
            FETCH NEXT @Fetch ROWS ONLY";

        private const string SelectByIdCondition = @"AND n.PrimaryNotificationId = @id";

        private readonly string connectionString;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly INotificationImportHelper _notificationImportHelper;
        private readonly IUserService _userService;
        private readonly bool LegacySearchEnabled;
        public IList<Sex> Sexes;

        public LegacySearchService(IConfiguration configuration,
                                    IReferenceDataRepository referenceDataRepository,
                                    INotificationImportHelper notificationImportHelper,
                                    IUserService userService)
        {
            LegacySearchEnabled = configuration.GetValue<bool>(Constants.LegacySearchEnabledConfigValue);
            connectionString = configuration.GetConnectionString("migration");
            _referenceDataRepository = referenceDataRepository;
            _notificationImportHelper = notificationImportHelper;
            if (LegacySearchEnabled)
            {
                Sexes = _referenceDataRepository.GetAllSexesAsync().Result;
            }
            _userService = userService;
        }

        public async Task<(IEnumerable<NotificationBannerModel> notifications, int count)> SearchAsync(
            ILegacySearchBuilder builder, int offset, int numberToFetch,
            ClaimsPrincipal user)
        {
            if (!LegacySearchEnabled)
            {
                return (new List<NotificationBannerModel>(), 0);
            }

            IEnumerable<dynamic> results;
            int count;
            var (queryConditions, parameters) = builder.GetResult();
            parameters.Offset = offset;
            parameters.Fetch = numberToFetch;
            var permittedTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code);
            parameters.editPermissionHospitals = (await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(permittedTbServiceCodes))
                .Select(h => h.HospitalId);

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

        public async Task<NotificationBannerModel> GetByIdAsync(string notificationId)
        {
            string fullQuery = SelectQueryStart + SelectByIdCondition;
            dynamic result;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                result = (await connection.QueryAsync(fullQuery, new { id = notificationId })).FirstOrDefault();
            }

            return result == null ? null : await AsNotificationBannerAsync(result);
        }

        private async Task<NotificationBannerModel> AsNotificationBannerAsync(dynamic result)
        {
            TBService tbService = null;
            string locationPhecCode = null;
            if (result.NtbsHospitalId is Guid guid)
            {
                tbService = await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid);
            }

            if (result.Postcode != null)
            {
                locationPhecCode = await _referenceDataRepository.GetLocationPhecCodeForPostcodeAsync(result.Postcode);
            }
            var notificationBannerModel = new NotificationBannerModel
            {
                NotificationId = result.PrimaryNotificationId,
                NotificationStatus = NotificationStatus.Legacy,
                NotificationStatusString = "Legacy",
                NotificationDate = (result.NotificationDate as DateTime?).ConvertToString(),
                Source = result.Source,
                Sex = Sexes.Single(s => s.SexId == result.NtbsSexId).Label,
                SortByDate = result.NotificationDate,
                Name = result.FamilyName.ToUpper() + ", " + result.GivenName,
                CountryOfBirth = result.BirthCountryName,
                TbService = tbService?.Name,
                TbServiceCode = tbService?.Code,
                TbServicePHECCode = tbService?.PHECCode,
                LocationPHECCode = locationPhecCode,
                Postcode = (result.Postcode as string).FormatStringToPostcodeFormat(),
                NhsNumber = (result.NhsNumber as string).FormatStringToNhsNumberFormat(),
                DateOfBirth = (result.DateOfBirth as DateTime?).ConvertToString(),
                ShowPadlock = true,
                ShowLink = true,
                RedirectPath = RouteHelper.GetLegacyNotificationPath(result.PrimaryNotificationId)
            };

            return notificationBannerModel;
        }
    }
}
