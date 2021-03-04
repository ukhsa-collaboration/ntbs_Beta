using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Projections;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class DrugResistanceProfileServiceTest
    {
        private const int MaxNumberOfUpdates = 3;

        private readonly DrugResistanceProfileService drpService;
        private readonly Mock<INotificationRepository> mockNotificationRepository;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly Mock<IEnhancedSurveillanceAlertsService> mockMdrService;
        private readonly Mock<IDrugResistanceProfileRepository> mockDrugResistanceProfileService;

        private static readonly DrugResistanceProfile drpWithMdr = new DrugResistanceProfile
        {
            Species = "TestSpecies",
            DrugResistanceProfileString = "RR/MDR/XDR"
        };

        private static readonly DrugResistanceProfile drpWithoutMdr = new DrugResistanceProfile
        {
            Species = "TestSpecies",
            DrugResistanceProfileString = "No Mdr"
        };

        private static readonly DrugResistanceProfile drpWithMbovis = new DrugResistanceProfile
        {
            Species = "M. bovis",
            DrugResistanceProfileString = "No Mdr"
        };

        private static readonly DrugResistanceProfile drpWithoutMbovis = new DrugResistanceProfile
        {
            Species = "No M. bovis",
            DrugResistanceProfileString = "No Mdr"
        };

        private readonly NotificationForDrugResistanceImport mockNotificationWithMdr =
            new NotificationForDrugResistanceImport { NotificationId = 1, DrugResistanceProfile = drpWithMdr };

        private readonly NotificationForDrugResistanceImport mockNotificationWithoutMdr =
            new NotificationForDrugResistanceImport { NotificationId = 2, DrugResistanceProfile = drpWithoutMdr };

        private readonly NotificationForDrugResistanceImport mockNotificationWithMbovis =
            new NotificationForDrugResistanceImport { NotificationId = 3, DrugResistanceProfile = drpWithMbovis };

        private readonly NotificationForDrugResistanceImport mockNotificationWithoutMbovis =
            new NotificationForDrugResistanceImport { NotificationId = 4, DrugResistanceProfile = drpWithoutMbovis };

        public DrugResistanceProfileServiceTest()
        {
            mockNotificationRepository = new Mock<INotificationRepository>();
            mockNotificationService = new Mock<INotificationService>();
            mockMdrService = new Mock<IEnhancedSurveillanceAlertsService>();
            mockDrugResistanceProfileService = new Mock<IDrugResistanceProfileRepository>();

            drpService = new DrugResistanceProfileService(
                mockNotificationService.Object,
                mockNotificationRepository.Object,
                mockDrugResistanceProfileService.Object,
                mockMdrService.Object);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_DoesNotUpdateRecordIfNotMatching()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {3, drpWithMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(3))
                .Returns(Task.FromResult<NotificationForDrugResistanceImport>(null));

            // Act
            Func<Task> act = () => drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            await Assert.ThrowsAsync<DataException>(act);
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfileAsync(It.IsAny<DrugResistanceProfile>(), It.IsAny<DrugResistanceProfile>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_DoesUpdateRecordIfMatching()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(It.IsAny<int>()))
                .Returns<int>(GetNotificationWithEmptyDrugResistanceProfile);

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfileAsync(It.IsAny<DrugResistanceProfile>(), It.IsAny<DrugResistanceProfile>()), Times.Exactly(2));
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Exactly(2));
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_WhenNoUpdatedDrp_NoAlertCreated()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(mockNotificationWithMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMdr));
            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(mockNotificationWithoutMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMdr));

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfileAsync(It.IsAny<DrugResistanceProfile>(), It.IsAny<DrugResistanceProfile>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_UpdatedOnlyChangedDrpString()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(mockNotificationWithMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMdr));
            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(mockNotificationWithoutMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMdr));

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfileAsync(It.IsAny<DrugResistanceProfile>(), It.IsAny<DrugResistanceProfile>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_UpdatedOnlyChangedDrpSpecies()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {3, drpWithMbovis},
                    {4, drpWithMbovis}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(mockNotificationWithMbovis.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMbovis));
            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(mockNotificationWithoutMbovis.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMbovis));

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfileAsync(It.IsAny<DrugResistanceProfile>(), It.IsAny<DrugResistanceProfile>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_OnlyUpdatesConfiguredNumberOfNotifications()
        {
            // Arrange
            var drugResistanceProfiles = Enumerable.Range(1, MaxNumberOfUpdates + 1)
                .ToDictionary(i => i, i => drpWithoutMdr);
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(drugResistanceProfiles));

            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(mockNotificationWithMdr));

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(
                x => x.UpdateDrugResistanceProfileAsync(It.IsAny<DrugResistanceProfile>(), It.IsAny<DrugResistanceProfile>()),
                Times.Exactly(MaxNumberOfUpdates));
            mockMdrService.Verify(
                x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()),
                Times.Exactly(MaxNumberOfUpdates));
        }

        [Theory]
        [InlineData(MaxNumberOfUpdates - 1, 0)]
        [InlineData(MaxNumberOfUpdates, 0)]
        [InlineData(MaxNumberOfUpdates + 1, 1)]
        public async Task UpdateDrugResistanceProfile_ReturnsCorrectNumberOfUpdatesLeftToDo(int totalNotifications, int expectedRemaining)
        {
            // Arrange
            var drugResistanceProfiles = Enumerable.Range(1, totalNotifications)
                .ToDictionary(i => i, i => drpWithoutMdr);
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(drugResistanceProfiles));

            mockNotificationRepository
                .Setup(x => x.GetNotificationForDrugResistanceImportAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(mockNotificationWithMdr));

            // Act
            var notificationsRemaining = await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            Assert.Equal(expectedRemaining, notificationsRemaining);
        }

        private static Task<NotificationForDrugResistanceImport> GetNotificationWithEmptyDrugResistanceProfile(int notificationId)
        {
            return Task.FromResult(new NotificationForDrugResistanceImport
            {
                NotificationId = notificationId,
                DrugResistanceProfile = new DrugResistanceProfile()
            });
        }
    }
}
