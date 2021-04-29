using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Server;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration
{
    public interface ICaseManagerImportService
    {
        Task ImportOrUpdateUserFromNotification(Notification notification, PerformContext context, string requestId);

        Task ImportOrUpdateLegacyUser(string legacyCaseManagerUsername, string legacyTbServiceCode);
    }
    
    public class CaseManagerImportService : ICaseManagerImportService
    {
        private readonly IUserRepository _userRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IMigrationRepository _migrationRepository;
        private readonly IImportLogger _logger;

        public CaseManagerImportService(
            IUserRepository userRepository,
            IReferenceDataRepository referenceDataRepository,
            IMigrationRepository migrationRepository,
            IImportLogger logger)
        {
            this._userRepository = userRepository;
            this._referenceDataRepository = referenceDataRepository;
            this._migrationRepository = migrationRepository;
            _logger = logger;
        }
        
        public async Task ImportOrUpdateUserFromNotification(Notification notification,
            PerformContext context,
            string requestId)
        {
            var legacyNotificationCaseManager = await GetLegacyNotificationCaseManager(notification.LegacyId);
            if (legacyNotificationCaseManager != null)
            {
                await ImportOrUpdateLegacyUser(legacyNotificationCaseManager.Username, notification.HospitalDetails.TBServiceCode);
                _logger.LogInformation(context, requestId, "Added/Updated the case manager assigned to the notification.");
            }
        }

        public async Task ImportOrUpdateLegacyUser(string legacyCaseManagerUsername,
            string legacyTbServiceCode)
        {
            var legacyCaseManager = await _migrationRepository.GetLegacyUserByUsername(legacyCaseManagerUsername);
            var existingCaseManager = await _userRepository.GetUserByUsername(legacyCaseManagerUsername);
            var ntbsCaseManager = existingCaseManager ?? 
                                  new User {IsActive = false, Username = legacyCaseManager.Username, CaseManagerTbServices = new List<CaseManagerTbService>()};
            
            ntbsCaseManager.GivenName = legacyCaseManager.GivenName;
            ntbsCaseManager.FamilyName = legacyCaseManager.FamilyName;
            ntbsCaseManager.DisplayName = $"{ntbsCaseManager.GivenName} {ntbsCaseManager.FamilyName}";
            
            await AddTbServiceToUserBasedOnLegacyPermissions(ntbsCaseManager, legacyTbServiceCode);

            await _userRepository.AddOrUpdateUser(ntbsCaseManager, ntbsCaseManager.CaseManagerTbServices.Select(cmtb => cmtb.TbService));
        }

        private async Task<MigrationLegacyUser> GetLegacyNotificationCaseManager(string legacyId)
        {
            var caseManagerUsername = (await _migrationRepository.GetNotificationsById(new List<string> {legacyId}))
                .Select(n => n.CaseManager)
                .SingleOrDefault();
            if (caseManagerUsername == null)
            {
                return null;
            }
            return await _migrationRepository.GetLegacyUserByUsername(caseManagerUsername);
        }

        private async Task AddTbServiceToUserBasedOnLegacyPermissions(User ntbsCaseManager, string legacyTbServiceCode)
        {
            var existingCaseManagerTbServices = await _referenceDataRepository.GetCaseManagerTbServicesByUsernameAsync
                (ntbsCaseManager.Username);
            if (!ntbsCaseManager.IsActive &&
                existingCaseManagerTbServices.All(cmtb => cmtb.TbServiceCode != legacyTbServiceCode))
            {
                var legacyUserHospitals = (await _migrationRepository.GetLegacyUserHospitalsByUsername(ntbsCaseManager.Username))
                    .Where(h => h.HospitalId != null).Select(h => h.HospitalId).OfType<Guid>();
                var legacyUserTbServices =
                    await _referenceDataRepository.GetTbServicesFromHospitalIdsAsync(legacyUserHospitals);
                var legacyMatchingTbService = legacyUserTbServices.SingleOrDefault(tb => tb.Code == legacyTbServiceCode);
                if (legacyMatchingTbService != null) {
                    ntbsCaseManager.IsCaseManager = true;
                    ntbsCaseManager.CaseManagerTbServices.Add(new CaseManagerTbService{TbService = legacyMatchingTbService});
                }
            }
        }
    }
}
