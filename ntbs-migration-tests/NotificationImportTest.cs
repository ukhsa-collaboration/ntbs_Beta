using System;
using System.Collections.Generic;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Enums;
using Microsoft.Extensions.Configuration;
using Xunit;
using Moq;
using ntbs_service.DataMigration;
using ntbs_service.DataAccess;
using System.Threading.Tasks;
using ntbs_service.Services;

namespace ntbs_migration_tests
{
    public class NotificationImportTest
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly NotificationMapper _notificationMapper;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly Mock<INotificationImportRepository> _mockNotificationImportRepository;
        private readonly Mock<IImportLogger> _mockLogger;
        private readonly Mock<IPostcodeService> _mockPostcodeService;
        private readonly NotificationImportService _notifcationImportService;

        private readonly string _connectionString = Environment.GetEnvironmentVariable("migrationDbUnitTest");
        private const string RequestId = "Test Request Id";

        public NotificationImportTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())[It.IsAny<string>()]).Returns(_connectionString);

            _notificationMapper = new NotificationMapper(_mockConfiguration.Object);

            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockNotificationImportRepository = new Mock<INotificationImportRepository>();
            _mockLogger = new Mock<IImportLogger>();
            _mockPostcodeService = new Mock<IPostcodeService>();
            _notifcationImportService = new NotificationImportService(_notificationMapper, 
                                                                    _mockNotificationRepository.Object,
                                                                    _mockNotificationImportRepository.Object,
                                                                    _mockPostcodeService.Object,
                                                                    _mockLogger.Object);
        }

        [Fact]
        public async Task NotificationExists()
        {
            //Given
            _mockNotificationRepository.Setup(x => x.NotificationWithLegacyIdExists("1")).Returns(true);

            //When
            var importedNotifications = await _notifcationImportService.ImportNotificationsAsync(RequestId, "1");

            //Then
            _mockLogger.Verify(x => x.LogInformation(RequestId, "Notification with Id=1 already exists in database"));
            Assert.Null(importedNotifications);
        }

        [Fact]
        public async Task NotificationNotFound()
        {
            //Given
            _mockNotificationRepository.Setup(x => x.NotificationWithLegacyIdExists(It.IsAny<string>())).Returns(false);

            //When
            var importedNotifications = await _notifcationImportService.ImportNotificationsAsync(RequestId, "0");

            //Then
            _mockLogger.Verify(x => x.LogInformation(RequestId, "No notifications found with Id=0"));
            Assert.Null(importedNotifications);
        }

        [Fact]
        public async Task NotificationWithoutValidationErrors()
        {
            //Given
            _mockNotificationRepository.Setup(x => x.NotificationWithLegacyIdExists(It.IsAny<string>()))
                                        .Returns(false);

            var dummyPostcodeList = new List<PostcodeLookup> {
                new PostcodeLookup {
                    Postcode = "SW154JY",
                    CountryCode = "UK"
                }
            };
            _mockPostcodeService.Setup(x => x.FindPostcodes(It.IsAny<List<string>>()))
                                .Returns(dummyPostcodeList);

            _mockNotificationImportRepository.Setup(x => x.AddLinkedNotificationsAsync(It.IsAny<List<Notification>>()))
                                            .Returns<List<Notification>>(x => Task.FromResult(x));
            var patientName = "TYLER, Kamala";

            //When
            var importedNotifications = await _notifcationImportService.ImportNotificationsAsync(RequestId, "66-1");

            //Then
            _mockLogger.Verify(x => x.LogInformation(RequestId, $"2 notifications found to import for {patientName}"));
            _mockLogger.Verify(x => x.LogInformation(RequestId, "No validation errors found"));
            _mockNotificationImportRepository.Verify(x => x.AddLinkedNotificationsAsync(It.Is<List<Notification>>(n => n.Count == 2)), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(RequestId, $"Finished importing notification for {patientName}"));
            Assert.Equal(2, importedNotifications.Count);
        }

        [Fact]
        public async Task NotificationValidationErrors()
        {
            //Given
            _mockNotificationRepository.Setup(x => x.NotificationWithLegacyIdExists(It.IsAny<string>()))
                                        .Returns(false);

            var dummyPostcodeList = new List<PostcodeLookup> {
                new PostcodeLookup {
                    Postcode = "EC1N7SS",
                    CountryCode = "UK"
                }
            };
            _mockPostcodeService.Setup(x => x.FindPostcodes(It.IsAny<List<string>>()))
                                .Returns(dummyPostcodeList);
            var patientName = "STETLER, Michael";

            //When
            var importedNotifications = await _notifcationImportService.ImportNotificationsAsync(RequestId, "41-1");

            //Then
            _mockLogger.Verify(x => x.LogInformation(RequestId, $"2 notifications found to import for {patientName}"));
            _mockLogger.Verify(x => x.LogWarning(RequestId, "Diagnosis date is a mandatory field"));
            _mockLogger.Verify(x => x.LogInformation(RequestId, "Terminating importing notifications for a patient due to validation errors"));
            _mockNotificationImportRepository.Verify(x => x.AddLinkedNotificationsAsync(It.IsAny<List<Notification>>()), Times.Never);
            Assert.Null(importedNotifications);
        }
    }
}
