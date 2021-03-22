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
        Task ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(Notification notification, PerformContext context, string requestId);
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
        
        public async Task ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(Notification notification,
            PerformContext context,
            string requestId)
        {
            var legacyNotificationCaseManager = await GetLegacyNotificationCaseManager(notification.LegacyId);
            if (legacyNotificationCaseManager == null)
            {
                return;
            }

            await ImportOrUpdateLegacyUser(legacyNotificationCaseManager, notification.HospitalDetails.TBServiceCode, context, requestId);
            _logger.LogInformation(context, requestId, "Added/Updated the case manager assigned to the notification.");
            
            foreach (var treatmentEvent in notification.TreatmentEvents)
            {
                if (treatmentEvent.CaseManagerUsername == null)
                {
                    continue;
                }
                
                var legacyTreatmentCaseManager = await _migrationRepository.GetLegacyUserByUsername(treatmentEvent.CaseManagerUsername);
                await ImportOrUpdateLegacyUser(legacyTreatmentCaseManager, treatmentEvent.TbServiceCode, context, requestId);
            }
            _logger.LogInformation(context, requestId, "Added/Updated the case managers assigned to the notification's transfer events.");
        }

        private async Task ImportOrUpdateLegacyUser(MigrationLegacyUser legacyCaseManager,
            string legacyTbServiceCode,
            PerformContext context,
            string requestId)
        {
            var existingCaseManager = await _userRepository.GetUserByUsername(legacyCaseManager.Username);
            var ntbsCaseManager = existingCaseManager ?? 
                                  new User {IsActive = false, Username = legacyCaseManager.Username, CaseManagerTbServices = new List<CaseManagerTbService>()};
            
            ntbsCaseManager.GivenName = legacyCaseManager.GivenName;
            ntbsCaseManager.FamilyName = legacyCaseManager.FamilyName;
            ntbsCaseManager.DisplayName = $"{ntbsCaseManager.GivenName} {ntbsCaseManager.FamilyName}";
            
            await AddTbServiceToUserBasedOnLegacyPermissions(ntbsCaseManager, legacyTbServiceCode, legacyCaseManager.Username);

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

        private async Task AddTbServiceToUserBasedOnLegacyPermissions(User ntbsCaseManager, string legacyTbServiceCode, string legacyCaseManagerUsername)
        {
            var existingCaseManagerTbServices = await _referenceDataRepository.GetCaseManagerTbServicesByUsernameAsync
                (ntbsCaseManager.Username);
            if (!ntbsCaseManager.IsActive &&
                existingCaseManagerTbServices.All(cmtb => cmtb.TbServiceCode != legacyTbServiceCode))
            {
                var legacyUserHospitals = (await _migrationRepository.GetLegacyUserHospitalsByUsername(legacyCaseManagerUsername))
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
