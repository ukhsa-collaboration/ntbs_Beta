using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public class TreatmentEventMapperTest : IDisposable
    {
        private readonly ITreatmentEventMapper _treatmentEventMapper;
        private readonly ICaseManagerImportService _caseManagerImportService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly NtbsContext _context;
        private readonly Mock<IOptionsMonitor<AdOptions>> _adOptionMock = new Mock<IOptionsMonitor<AdOptions>>();
        private Dictionary<string, MigrationLegacyUser> _usernameToLegacyUserDict = new Dictionary<string, MigrationLegacyUser>();

        public TreatmentEventMapperTest()
        {
            _context = SetupTestContext();
            _referenceDataRepository = new ReferenceDataRepository(_context);
            _adOptionMock.Setup(s => s.CurrentValue).Returns(new AdOptions{ReadOnlyUserGroup = "TestReadOnly"});
            var userRepo = new UserRepository(_context, _adOptionMock.Object);
            var migrationRepo = new Mock<IMigrationRepository>();
            migrationRepo.Setup(mr => mr.GetLegacyUserByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToLegacyUserDict[username]));
            migrationRepo.Setup(repo => repo.GetLegacyUserHospitalsByUsername(It.IsAny<string>()))
                .ReturnsAsync((string username) => new List<MigrationLegacyUserHospital>());
            var importLogger = new Mock<IImportLogger>();
            _caseManagerImportService =
                new CaseManagerImportService(userRepo, _referenceDataRepository, migrationRepo.Object, importLogger.Object);
            _treatmentEventMapper = new TreatmentEventMapper(_caseManagerImportService, _referenceDataRepository);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task MigrationTreatmentEventMappedCorrectlyWithTbServiceAndCaseManagerAdded()
        {
            // Arrange
            GivenLegacyUserWithName("miles.davis@columbia.nhs.uk", "Miles", "Davis");
            GivenHospitalIdHasTbServiceCode(new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B"), "TBS00JAZZ");
            var migrationTransferEvent = new MigrationDbTransferEvent
            {
                EventDate = DateTime.Parse("12/12/2012"),
                CaseManager = "miles.davis@columbia.nhs.uk",
                HospitalId = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B"),
                TreatmentEventType = "TransferIn"
            };

            // Act
            var mappedEvent = await _treatmentEventMapper.AsTransferEvent(migrationTransferEvent, null, 1);

            // Assert
            Assert.Equal(DateTime.Parse("12/12/2012"), mappedEvent.EventDate);
            Assert.Equal(TreatmentEventType.TransferIn, mappedEvent.TreatmentEventType);
            Assert.Equal("TBS00JAZZ", mappedEvent.TbServiceCode);
            Assert.NotNull(_context.User.SingleOrDefault(u => u.Username == "miles.davis@columbia.nhs.uk"));
            Assert.Equal(_context.User.Single().Id, mappedEvent.CaseManagerId);
        }

        [Fact]
        public async Task MigrationTreatmentEventWithBoilerplateNoteMappedToNull()
        {
            // Arrange
            GivenLegacyUserWithName("miles.davis@columbia.nhs.uk", "Miles", "Davis");
            var migrationTransferEvent = new MigrationDbTransferEvent
            {
                EventDate = DateTime.Parse("12/12/2012"),
                CaseManager = "miles.davis@columbia.nhs.uk",
                HospitalId = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B"),
                TreatmentEventType = "TransferIn",
                Notes = "Dear Mrs Lucy Carmen-Minoe, \n You have been identified as the new case manager for the case below.\n"
                    + "Id: 134222 \n\n Patient: Harry Swingset  Case report date: 11/11/2009"
            };

            // Act
            var mappedEvent = await _treatmentEventMapper.AsTransferEvent(migrationTransferEvent, null, 1);

            // Assert
            Assert.Equal(DateTime.Parse("12/12/2012"), mappedEvent.EventDate);
            Assert.Equal(TreatmentEventType.TransferIn, mappedEvent.TreatmentEventType);
            Assert.Null(mappedEvent.Note);
        }

        [Fact]
        public async Task MigrationTreatmentEventWithCustomNoteMappedToWithoutBoilerplateParts()
        {
            // Arrange
            GivenLegacyUserWithName("miles.davis@columbia.nhs.uk", "Miles", "Davis");
            var migrationTransferEvent = new MigrationDbTransferEvent
            {
                EventDate = DateTime.Parse("12/12/2012"),
                CaseManager = "miles.davis@columbia.nhs.uk",
                HospitalId = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B"),
                TreatmentEventType = "TransferIn",
                Notes = "Dear Mrs Lucy Carmen-Minoe, \n You have been identified as the new case manager for the case below. This patient was moved from XYX hospital in London. This is very important information.\n"
                        + "Id: 134222 \n\n Patient: Harry Swingset  Case report date: 11/11/2009 \n Also they are allergic to mung beans"
            };

            // Act
            var mappedEvent = await _treatmentEventMapper.AsTransferEvent(migrationTransferEvent, null, 1);

            // Assert
            Assert.Equal(DateTime.Parse("12/12/2012"), mappedEvent.EventDate);
            Assert.Equal(TreatmentEventType.TransferIn, mappedEvent.TreatmentEventType);
            Assert.Contains("This patient was moved from XYX hospital in London. This is very important information.", mappedEvent.Note);
            Assert.Contains("Also they are allergic to mung beans", mappedEvent.Note);
            Assert.DoesNotContain("Id: 134222", mappedEvent.Note);
            Assert.DoesNotContain("Dear Mrs Lucy Carmen-Minoe", mappedEvent.Note);
            Assert.DoesNotContain("You have been identified as the new case manager for the case below.", mappedEvent.Note);
        }

        [Fact]
        public async Task MigrationOutcomeEventMappedCorrectlyWithTbServiceAndCaseManagerAdded()
        {
            // Arrange
            GivenLegacyUserWithName("milicent.davis@columbia.nhs.uk", "Milicent", "Davis");
            GivenHospitalIdHasTbServiceCode(new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B"), "TBS00JAZZ");
            var migrationTransferEvent = new MigrationDbOutcomeEvent
            {
                EventDate = DateTime.Parse("12/12/2012"),
                CaseManager = "milicent.davis@columbia.nhs.uk",
                NtbsHospitalId = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B"),
                TreatmentEventType = "TransferIn",
                TreatmentOutcomeId = 2,
                Note = "The patient had a specific outcome"
            };

            // Act
            var mappedEvent = await _treatmentEventMapper.AsOutcomeEvent(migrationTransferEvent, null, 1);

            // Assert
            Assert.Equal(DateTime.Parse("12/12/2012"), mappedEvent.EventDate);
            Assert.Equal(TreatmentEventType.TransferIn, mappedEvent.TreatmentEventType);
            Assert.Equal("The patient had a specific outcome", mappedEvent.Note);
            Assert.Equal(2, mappedEvent.TreatmentOutcomeId);
            Assert.Equal("TBS00JAZZ", mappedEvent.TbServiceCode);
            Assert.NotNull(_context.User.SingleOrDefault(u => u.Username == "milicent.davis@columbia.nhs.uk"));
            Assert.Equal(_context.User.Single().Id, mappedEvent.CaseManagerId);
        }

        [Fact]
        public async Task MigrationOutcomeEventMappedCorrectlyWithNoTbServiceOrCaseManagerAdded()
        {
            // Arrange
            GivenLegacyUserWithName("milicent.davis@columbia.nhs.uk", "Milicent", "Davis");
            var migrationTransferEvent = new MigrationDbOutcomeEvent
            {
                EventDate = DateTime.Parse("12/12/2012"),
                NtbsHospitalId = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B"),
                TreatmentEventType = "TransferIn",
                TreatmentOutcomeId = 2,
                Note = "The patient had a specific outcome"
            };

            // Act
            var mappedEvent = await _treatmentEventMapper.AsOutcomeEvent(migrationTransferEvent, null, 1);

            // Assert
            Assert.Equal(DateTime.Parse("12/12/2012"), mappedEvent.EventDate);
            Assert.Equal(TreatmentEventType.TransferIn, mappedEvent.TreatmentEventType);
            Assert.Equal("The patient had a specific outcome", mappedEvent.Note);
            Assert.Equal(2, mappedEvent.TreatmentOutcomeId);
            Assert.Null(mappedEvent.TbServiceCode);
            Assert.Null(mappedEvent.CaseManagerId);
        }

        private void GivenLegacyUserWithName(string username, string givenName, string familyName)
        {
            _usernameToLegacyUserDict.Add(
                username,
                new MigrationLegacyUser
                {
                    Username = username, GivenName = givenName, FamilyName = familyName
                }
            );
        }

        private void GivenHospitalIdHasTbServiceCode(Guid hospitalId, string tbServiceCode)
        {
            _context.Hospital.Add(new Hospital {TBServiceCode = tbServiceCode, HospitalId = hospitalId});
            _context.TbService.Add(new TBService {Code = tbServiceCode});
            _context.SaveChanges();
        }

        private NtbsContext SetupTestContext()
        {
            // Generating a unique database name makes sure the database is not shared between tests.
            string dbName = Guid.NewGuid().ToString();
            return new NtbsContext(new DbContextOptionsBuilder<NtbsContext>()
                .UseInMemoryDatabase(dbName)
                .Options
            );
        }
    }
}
