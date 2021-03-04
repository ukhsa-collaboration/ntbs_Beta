using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public class CaseManagerImportServiceTests
    {
        private readonly CaseManagerImportService _caseManagerImportService;
        private readonly Mock<IReferenceDataRepository> _referenceDataRepositoryMock =
            new Mock<IReferenceDataRepository>();
        private readonly Mock<IMigrationRepository> _migrationRepositoryMock = new Mock<IMigrationRepository>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        
        private Dictionary<string, User> _usernameToUserDict;
        
        private Dictionary<List<string>, IEnumerable<MigrationDbNotification>> _idToNotificationDict;
        private Dictionary<string, MigrationLegacyUser> _usernameToLegacyUserDict;
        private Dictionary<string, IEnumerable<MigrationLegacyUserHospital>> _usernameToLegacyUserHospitalDict;
        
        private Dictionary<List<Guid>, IList<TBService>> _legacyUserHospitalsToTbServicesDict;
        private Dictionary<string, IList<CaseManagerTbService>> _usernameToCaseManagerTbServicesDict;

        public CaseManagerImportServiceTests()
        {
            SetupMockUserRepo();
            SetupMockMigrationRepo();
            SetupMockReferenceRepo();
            var importLogger = new ImportLogger();
            _caseManagerImportService = new CaseManagerImportService(_userRepositoryMock.Object, _referenceDataRepositoryMock.Object,
                _migrationRepositoryMock.Object, importLogger);
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyNotificationDoesNotExist_AddsCaseManager()
        {
            var legacyIds = new List<string> {"130331"};
            SetupNotificationsInGroups(("130331", "1"));
            const string royalBerkshireCode = "TBS001";
            const string bristolRoyalCode = "TBS002";
            const string westonGeneralCode = "TBS003";
            _hospitalToTbServiceCodeDict = new Dictionary<Guid, TBService>
            {
                {new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A"), new TBService {Code = royalBerkshireCode}},
                {new Guid("F026FDCD-7BAF-4C96-994C-20E436CC8C59"), new TBService {Code = bristolRoyalCode}},
                {new Guid("0AC033AB-9A11-4FA6-AA1A-1FCA71180C2F"), new TBService {Code = westonGeneralCode}}
            };
        }

        private void SetupMockUserRepo()
        {
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToUserDict[username]));
        }

        private void SetupMockMigrationRepo()
        {
            _migrationRepositoryMock.Setup(repo => repo.GetNotificationsById(It.IsAny<List<string>>()))
                .Returns((List<string> ids) => Task.FromResult(_idToNotificationDict[ids]));
            _migrationRepositoryMock.Setup(repo => repo.GetLegacyUserByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToLegacyUserDict[username]));
            _migrationRepositoryMock.Setup(repo => repo.GetLegacyUserHospitalsByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToLegacyUserHospitalDict[username]));
            _migrationRepositoryMock.Setup(repo => repo.GetNotificationsById(It.IsAny<List<string>>()))
                .Returns((List<string> ids) => Task.FromResult(_idToNotificationDict[ids]));
        }

        private void SetupMockReferenceRepo()
        {
            _referenceDataRepositoryMock.Setup(repo => repo.GetCaseManagerTbServicesByUsernameAsync(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToCaseManagerTbServicesDict[username]));
            _referenceDataRepositoryMock.Setup(repo => repo.GetTbServicesFromHospitalIdsAsync(It.IsAny<List<Guid>>()))
                .Returns((List<Guid> ids) => Task.FromResult(_legacyUserHospitalsToTbServicesDict[ids]));
        }
    }
}
