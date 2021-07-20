using System;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.DataAccess
{
    public class AlertRepositoryTests : IClassFixture<RepositoryFixture<AlertRepository>>, IDisposable
    {
        private readonly NtbsContext _context;
        private readonly AlertRepository _alertRepo;

        public AlertRepositoryTests(RepositoryFixture<AlertRepository> alertRepositoryFixture)
        {
            _context = alertRepositoryFixture.Context;
            _alertRepo = new AlertRepository(_context);
        }

        private readonly User _caseManager = new User {DisplayName = "Pharoah Sanders", Username = "ps@fake-nhs.co"};
        private readonly TBService _tbService = new TBService {Code = "TB-TEST-001", Name = "Test Service 001"};

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public async void CorrectlyMapsTransferAlertToDisplayAlert(bool hasCaseManager, bool hasTbService)
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
            Assert.Equal(hasCaseManager ? _caseManager.DisplayName : null, result.CaseManagerName);
            Assert.Equal(hasTbService ? _tbService.Code : null, result.TbServiceCode);
            Assert.Equal(hasTbService ? _tbService.Name : null, result.TbServiceName);
            Assert.Equal(new DateTime(2004,2,2), result.CreationDate);
            Assert.Equal(notification.NotificationId, result.NotificationId);
            Assert.Equal(AlertType.TransferRequest, result.AlertType);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async void CorrectlyMapsUserOnNonTransferAlertToDisplayAlert(bool hasCaseManager, bool caseManagerIsActive)
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
            Assert.Equal(caseManagerIsActive && hasCaseManager ? caseManager.DisplayName : null, result.CaseManagerName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void CorrectlyMapsTBServiceOnNonTransferAlertToDisplayAlert(bool hasTbService)
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
            Assert.Equal(hasTbService ? _tbService.Code : null, result.TbServiceCode);
            Assert.Equal(hasTbService ? _tbService.Name : null, result.TbServiceName);
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
