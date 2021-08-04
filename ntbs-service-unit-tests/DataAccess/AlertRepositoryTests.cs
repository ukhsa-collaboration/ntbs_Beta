using System;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service_unit_tests.TestHelpers;
using Xunit;

namespace ntbs_service_unit_tests.DataAccess
{
    public class AlertRepositoryTests : IClassFixture<RepositoryFixture<AlertRepository>>, IDisposable
    {
        private readonly NtbsContext _context;
        private readonly AlertRepository _alertRepo;

        public AlertRepositoryTests(RepositoryFixture<AlertRepository> alertRepositoryFixture)
        {
            ContextHelper.DisableAudits();
            _context = alertRepositoryFixture.Context;
            _alertRepo = new AlertRepository(_context);
        }

        private readonly User _caseManager = new User {DisplayName = "Pharoah Sanders", Username = "ps@fake-nhs.co"};
        private readonly TBService _tbService = new TBService {Code = "TB-TEST-001", Name = "Test Service 001"};

        [Theory]
        [InlineData(true, true, "Pharoah Sanders", "Test Service 001", "TB-TEST-001")]
        [InlineData(false, true, null, "Test Service 001", "TB-TEST-001")]
        [InlineData(true, false, "Pharoah Sanders", null, null)]
        [InlineData(false, false, null, null, null)]
        public async void CorrectlyMapsTransferAlertToDisplayAlert(
            bool hasCaseManager,
            bool hasTbService,
            string expectedCaseManagerName,
            string expectedTbServiceName,
            string expectedTbServiceCode)
        {
            // Arrange
            var notification = await GivenNotificationWithCaseManagerAndTbService
                (hasCaseManager ? _caseManager : null, hasTbService ? _tbService : null);
            await _context.Alert.AddAsync(new TransferAlert()
            {
                CaseManagerId = notification.HospitalDetails.CaseManagerId,
                TbServiceCode = notification.HospitalDetails.TBServiceCode,
                CreationDate = new DateTime(2004,2,2),
                NotificationId = notification.NotificationId,
                AlertType = AlertType.TransferRequest
            });
            _context.SaveChanges();

            // Act
            var result = (await _alertRepo.GetOpenAlertsForNotificationAsync(notification.NotificationId)).Single();

            // Assert
            Assert.Equal(expectedCaseManagerName, result.CaseManagerName);
            Assert.Equal(expectedTbServiceCode, result.TbServiceCode);
            Assert.Equal(expectedTbServiceName, result.TbServiceName);
            Assert.Equal(new DateTime(2004,2,2), result.CreationDate);
            Assert.Equal(notification.NotificationId, result.NotificationId);
            Assert.Equal(AlertType.TransferRequest, result.AlertType);
        }

        [Theory]
        [InlineData(true, true, "Pharoah Sanders")]
        [InlineData(true, false, null)]
        [InlineData(false, false, null)]
        public async void CorrectlyMapsUserOnNonTransferAlertToDisplayAlert
            (bool hasCaseManager, bool caseManagerIsActive, string expectedCaseManagerName)
        {
            // Arrange
            var caseManager = hasCaseManager
                ? new User {DisplayName = "Pharoah Sanders", Username = "ps@fake-nhs.co", IsActive = caseManagerIsActive}
                : null;
            var notification = await GivenNotificationWithCaseManagerAndTbService(caseManager, _tbService);

            await _context.Alert.AddAsync(new DataQualityTreatmentOutcome12()
            {
                CreationDate = new DateTime(2004,2,2),
                NotificationId = notification.NotificationId,
                AlertType = AlertType.DataQualityTreatmentOutcome12
            });
            _context.SaveChanges();

            // Act
            var result = (await _alertRepo.GetOpenAlertsForNotificationAsync(notification.NotificationId)).Single();

            // Assert
            Assert.Equal(expectedCaseManagerName, result.CaseManagerName);
        }

        [Theory]
        [InlineData(true, "Test Service 001", "TB-TEST-001")]
        [InlineData(false, null, null)]
        public async void CorrectlyMapsTBServiceOnNonTransferAlertToDisplayAlert(
            bool hasTbService,
            string expectedTbServiceName,
            string expectedTbServiceCode)
        {
            // Arrange
            var notification = await GivenNotificationWithCaseManagerAndTbService(_caseManager, hasTbService ? _tbService : null);

            await _context.Alert.AddAsync(new DataQualityTreatmentOutcome12()
            {
                CreationDate = new DateTime(2004,2,2),
                NotificationId = notification.NotificationId,
                AlertType = AlertType.DataQualityTreatmentOutcome12
            });
            _context.SaveChanges();

            // Act
            var result = (await _alertRepo.GetOpenAlertsForNotificationAsync(notification.NotificationId)).Single();

            // Assert
            Assert.Equal(expectedTbServiceCode, result.TbServiceCode);
            Assert.Equal(expectedTbServiceName, result.TbServiceName);
        }

        private async Task<Notification> GivenNotificationWithCaseManagerAndTbService(User caseManager, TBService tbService)
        {
            var notification = await _context.Notification.AddAsync(new Notification {NotificationId = 6655,
                HospitalDetails = new HospitalDetails
                {
                    CaseManager = caseManager,
                    TBService = tbService
                }
            });
            await _context.SaveChangesAsync();
            return notification.Entity;
        }

        public void Dispose()
        {
            _context.Alert.RemoveRange(_context.Alert);
            _context.Notification.RemoveRange(_context.Notification);
            _context.TbService.RemoveRange(_context.TbService);
            _context.User.RemoveRange(_context.User);
            _context.SaveChanges();
        }
    }
}
