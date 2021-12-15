using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Server;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration
{
    public interface ICaseManagerImportService
    {
        Task ImportOrUpdateLegacyUser(string legacyCaseManagerUsername, string legacyTbServiceCode, PerformContext context, int runId);
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

        public async Task ImportOrUpdateLegacyUser(string legacyCaseManagerUsername,
            string legacyTbServiceCode, PerformContext context, int runId)
        {
            if (legacyCaseManagerUsername == null)
            {
                return;
            }
            var legacyCaseManager = await _migrationRepository.GetLegacyUserByUsername(legacyCaseManagerUsername);
            var existingCaseManager = await _userRepository.GetUserByUsername(legacyCaseManagerUsername);
            var ntbsCaseManager = existingCaseManager ??
                                  new User {
                                      IsActive = false,
                                      IsCaseManager = false,
                                      Username = legacyCaseManager.Username,
                                      CaseManagerTbServices = new List<CaseManagerTbService>()
                                  };

            if (!ntbsCaseManager.IsActive)
            {
                ntbsCaseManager.GivenName = legacyCaseManager.GivenName;
                ntbsCaseManager.FamilyName = legacyCaseManager.FamilyName;
                ntbsCaseManager.DisplayName = $"{legacyCaseManager.GivenName} {legacyCaseManager.FamilyName}";
            }
            ntbsCaseManager.IsCaseManager = true;

            await _userRepository.AddOrUpdateUser(ntbsCaseManager, ntbsCaseManager.CaseManagerTbServices.Select(cmtb => cmtb.TbService));

            _logger.LogInformation(context, runId, "Added/Updated the case manager assigned to the notification.");
        }
    }
}
