using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire.Server;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public class NotificationImportServiceTest
    {
        private readonly INotificationImportService _notificationImportService;

        private readonly Mock<INotificationMapper> _notificationMapper = new Mock<INotificationMapper>();
        private readonly Mock<INotificationRepository> _notificationRepository = new Mock<INotificationRepository>();
        private readonly Mock<IImportLogger> _logger = new Mock<IImportLogger>();
        private readonly Mock<Sentry.IHub> _sentryHub = new Mock<Sentry.IHub>();
        private readonly Mock<ISpecimenImportService> _specimenImportService = new Mock<ISpecimenImportService>();
        private readonly Mock<IImportValidator> _importValidator = new Mock<IImportValidator>();
        private readonly Mock<IClusterImportService> _clusterImportService = new Mock<IClusterImportService>();

        private readonly Mock<INotificationImportRepository> _notificationImportRepository =
            new Mock<INotificationImportRepository>();

        private readonly Mock<IMigratedNotificationsMarker> _migratedNotificationsMarker =
            new Mock<IMigratedNotificationsMarker>();

        private readonly Mock<ICultureAndResistanceService> _cultureAndResistanceService =
            new Mock<ICultureAndResistanceService>();

        private readonly Mock<IDrugResistanceProfileService> _drugResistanceProfileService =
            new Mock<IDrugResistanceProfileService>();

        private readonly Mock<ICaseManagerImportService> _caseManagerImportService =
            new Mock<ICaseManagerImportService>();

        private readonly PerformContext _performContext = null;
        private readonly int _runId = 12345;

        public NotificationImportServiceTest()
        {
            _notificationImportService = new NotificationImportService(
                _notificationMapper.Object,
                _notificationRepository.Object,
                _notificationImportRepository.Object,
                _logger.Object,
                _sentryHub.Object,
                _migratedNotificationsMarker.Object,
                _specimenImportService.Object,
                _importValidator.Object,
                _clusterImportService.Object,
                _cultureAndResistanceService.Object,
                _drugResistanceProfileService.Object,
                _caseManagerImportService.Object);

            _importValidator
                .Setup(iv => iv.CleanAndValidateNotification(_performContext, _runId, It.IsAny<Notification>()))
                .Returns(Task.FromResult(new List<ValidationResult>()));

            _notificationImportRepository
                .Setup(nir => nir.AddLinkedNotificationsAsync(It.IsAny<List<Notification>>()))
                .Returns((List<Notification> notificationList) => Task.FromResult(notificationList));

            _notificationRepository.Setup(nr => nr.NotificationWithLegacyIdExistsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
        }

        [Fact]
        public async Task ImportByDateAsync_ImportsIdsInRange()
        {
            // Arrange
            var rangeStartDate = DateTime.Parse("2021-01-01");
            var rangeEndDate = DateTime.Parse("2021-05-01");
            var notification1 = new Notification { NotificationId = 101, ETSID = "1" };
            var notification2 = new Notification { NotificationId = 102, ETSID = "2" };
            var notificationList = new List<IList<Notification>> { new[] { notification1 }, new[] { notification2 } };

            _notificationMapper.Setup(nm =>
                    nm.GetNotificationsGroupedByPatient(_performContext, _runId, rangeStartDate, rangeEndDate))
                .Returns(Task.FromResult(notificationList.AsEnumerable()));

            // Act
            var importResults =
                await _notificationImportService.ImportByDateAsync(_performContext, _runId, rangeStartDate, rangeEndDate);

            // Assert
            VerifyNotificationImportServicesAreCalled(notification1, Times.Once());
            VerifyNotificationImportServicesAreCalled(notification2, Times.Once());

            Assert.All(importResults, result => Assert.True(result.IsValid));
            Assert.Collection(importResults,
                result => Assert.Contains(notification1.NotificationId, result.NtbsIds.Values),
                result => Assert.Contains(notification2.NotificationId, result.NtbsIds.Values));
        }

        [Fact]
        public async Task ImportByLegacyIdsAsync_ImportsIdsFromList()
        {
            // Arrange
            var legacyIds = new List<string> { "1", "2", "3" };
            var notification1 = new Notification { NotificationId = 101, ETSID = "1" };
            var notification2 = new Notification { NotificationId = 102, ETSID = "2" };
            var notificationList = new List<IList<Notification>> { new[] { notification1 }, new[] { notification2 } };

            _notificationMapper.Setup(nm =>
                    nm.GetNotificationsGroupedByPatient(_performContext, _runId, legacyIds))
                .Returns(Task.FromResult(notificationList.AsEnumerable()));

            // Act
            var importResults =
                await _notificationImportService.ImportByLegacyIdsAsync(_performContext, _runId, legacyIds);

            // Assert
            VerifyNotificationImportServicesAreCalled(notification1, Times.Once());
            VerifyNotificationImportServicesAreCalled(notification2, Times.Once());

            Assert.All(importResults, result => Assert.True(result.IsValid));
            Assert.Collection(importResults,
                result => Assert.Contains(notification1.NotificationId, result.NtbsIds.Values),
                result => Assert.Contains(notification2.NotificationId, result.NtbsIds.Values));
        }

        [Fact]
        public async Task ImportByLegacyIdsAsync_ImportsNotificationGroup()
        {
            // Arrange
            var legacyIds = new List<string> { "1", "2" };
            var notification1 = new Notification { NotificationId = 101, ETSID = "1" };
            var notification2 = new Notification { NotificationId = 102, ETSID = "2" };
            var notificationList = new List<IList<Notification>> { new[] { notification1, notification2 } };

            _notificationMapper.Setup(nm =>
                    nm.GetNotificationsGroupedByPatient(_performContext, _runId, legacyIds))
                .Returns(Task.FromResult(notificationList.AsEnumerable()));

            // Act
            var importResults =
                await _notificationImportService.ImportByLegacyIdsAsync(_performContext, _runId, legacyIds);

            // Assert
            VerifyInitialImportServicesAreCalled(notification1, Times.Once());
            VerifyInitialImportServicesAreCalled(notification2, Times.Once());
            VerifyCommittingImportServicesAreCalled(new[] { notification1, notification2 }, Times.Once());

            Assert.Collection(importResults, result =>
            {
                Assert.True(result.IsValid);
                Assert.Contains(notification1.NotificationId, result.NtbsIds.Values);
                Assert.Contains(notification2.NotificationId, result.NtbsIds.Values);
            });
        }

        [Fact]
        public async Task ImportByLegacyIdsAsync_DoesNotImportExistingNotifications()
        {
            // Arrange
            var legacyIds = new List<string> { "1", "2", "3" };
            var notification1 = new Notification { NotificationId = 101, ETSID = "1" };
            var existingNotification2 = new Notification { NotificationId = 102, ETSID = "2" };
            var notificationList = new List<IList<Notification>>
            {
                new[] { notification1 }, new[] { existingNotification2 }
            };

            const string existingId = "2";

            _notificationMapper.Setup(nm =>
                    nm.GetNotificationsGroupedByPatient(_performContext, _runId, legacyIds))
                .Returns(Task.FromResult(notificationList.AsEnumerable()));

            _notificationRepository.Setup(nr => nr.NotificationWithLegacyIdExistsAsync(existingId))
                .Returns(Task.FromResult(true));

            // Act
            var importResults =
                await _notificationImportService.ImportByLegacyIdsAsync(_performContext, _runId, legacyIds);

            // Assert
            VerifyNotificationImportServicesAreCalled(notification1, Times.Once());
            VerifyNotificationImportServicesAreCalled(existingNotification2, Times.Never());

            Assert.All(importResults, result => Assert.True(result.IsValid));
            Assert.Collection(importResults,
                result => Assert.Contains(notification1.NotificationId, result.NtbsIds.Values));
        }

        [Fact]
        public async Task ImportByLegacyIdsAsync_DoesNotImportInvalidNotifications()
        {
            // Arrange
            var legacyIds = new List<string> { "1", "2", "3" };
            var notification1 = new Notification { NotificationId = 101, ETSID = "1" };
            var invalidNotification2 = new Notification { NotificationId = 103, ETSID = "3" };
            var notificationList = new List<IList<Notification>>
            {
                new[] {notification1}, new[] { invalidNotification2 }
            };

            _notificationMapper.Setup(nm =>
                    nm.GetNotificationsGroupedByPatient(_performContext, _runId, legacyIds))
                .Returns(Task.FromResult(notificationList.AsEnumerable()));

            _importValidator
                .Setup(iv => iv.CleanAndValidateNotification(_performContext, _runId, invalidNotification2))
                .Returns(Task.FromResult(new List<ValidationResult> { new ValidationResult("Validator error") }));

            // Act
            var importResults =
                await _notificationImportService.ImportByLegacyIdsAsync(_performContext, _runId, legacyIds);

            // Assert
            VerifyNotificationImportServicesAreCalled(notification1, Times.Once());

            VerifyInitialImportServicesAreCalled(invalidNotification2, Times.Once());
            VerifyCommittingImportServicesAreCalled(new[] { invalidNotification2 }, Times.Never());

            Assert.Collection(importResults, result =>
            {
                Assert.Contains(notification1.NotificationId, result.NtbsIds.Values);
                Assert.True(result.IsValid);
            }, result =>
            {
                Assert.Empty(result.NtbsIds);
                Assert.False(result.IsValid);
            });
        }

        private void VerifyNotificationImportServicesAreCalled(Notification notification, Times times)
        {
            VerifyInitialImportServicesAreCalled(notification, times);
            VerifyCommittingImportServicesAreCalled(new List<Notification> { notification }, times);
        }

        private void VerifyInitialImportServicesAreCalled(Notification notification, Times times)
        {
            _importValidator
                .Verify(s => s.CleanAndValidateNotification(_performContext, _runId, notification), times);
        }

        private void VerifyCommittingImportServicesAreCalled(IList<Notification> notifications, Times times)
        {
            Expression<Func<List<Notification>, bool>> matchesNotificationList = l => l.SequenceEqual(notifications);

            _notificationImportRepository
                .Verify(s => s.AddLinkedNotificationsAsync(It.Is(matchesNotificationList)), times);
            _migratedNotificationsMarker.Verify(s =>
                s.MarkNotificationsAsImportedAsync(It.Is(matchesNotificationList)), times);
            _specimenImportService.Verify(s => s.ImportReferenceLabResultsAsync(
                    _performContext,
                    _runId,
                    It.Is(matchesNotificationList),
                    It.IsAny<ImportResult>()),
                times);
            _cultureAndResistanceService.Verify(s =>
                s.MigrateNotificationCultureResistanceSummary(It.Is(matchesNotificationList)), times);
            _drugResistanceProfileService.Verify(s =>
                s.UpdateDrugResistanceProfiles(It.Is(matchesNotificationList)), times);
            _clusterImportService.Verify(s =>
                s.UpdateClusterInformation(It.Is(matchesNotificationList)), times);
        }
    }
}
