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

        private int numberOfUpdatesOnLastRun;

        private readonly DrugResistanceProfileService drpService;
        private readonly Mock<INotificationRepository> mockNotificationRepository;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly Mock<IEnhancedSurveillanceAlertsService> mockMdrService;
        private readonly Mock<IDrugResistanceProfileRepository> mockDrugResistanceProfileRepository;

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
            new NotificationForDrugResistanceImport
            {
                NotificationId = 1,
                DrugResistanceProfile = drpWithMdr
            };

        private readonly NotificationForDrugResistanceImport mockNotificationWithoutMdr =
            new NotificationForDrugResistanceImport
            {
                NotificationId = 2,
                DrugResistanceProfile = drpWithoutMdr
            };

        private readonly NotificationForDrugResistanceImport mockNotificationWithMbovis =
            new NotificationForDrugResistanceImport
            {
                NotificationId = 3,
                DrugResistanceProfile = drpWithMbovis
            };

        private readonly NotificationForDrugResistanceImport mockNotificationWithoutMbovis =
            new NotificationForDrugResistanceImport
            {
                NotificationId = 4,
                DrugResistanceProfile = drpWithoutMbovis
            };

        public DrugResistanceProfileServiceTest()
        {
            mockNotificationRepository = new Mock<INotificationRepository>();
            mockNotificationService = new Mock<INotificationService>();
            mockNotificationService
                .Setup(x => x.UpdateDrugResistanceProfilesAsync(
                    It.IsAny<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>()))
                .Callback<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>(x =>
                {
                    numberOfUpdatesOnLastRun = x.Count();
                });
            mockMdrService = new Mock<IEnhancedSurveillanceAlertsService>();
            mockDrugResistanceProfileRepository = new Mock<IDrugResistanceProfileRepository>();

            drpService = new DrugResistanceProfileService(
                mockNotificationService.Object,
                mockNotificationRepository.Object,
                mockDrugResistanceProfileRepository.Object,
                mockMdrService.Object);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_DoesNotUpdateRecordIfNotMatching()
        {
            // Arrange
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {3, drpWithMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns(Task.FromResult<IEnumerable<NotificationForDrugResistanceImport>>(
                    new List<NotificationForDrugResistanceImport>()));

            // Act
            Func<Task> act = () => drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            await Assert.ThrowsAsync<DataException>(act);
            mockNotificationService.Verify(
                x => x.UpdateDrugResistanceProfilesAsync(
                    It.IsAny<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>()),
                Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_DoesUpdateRecordIfMatching()
        {
            // Arrange
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns<IEnumerable<int>>(GetNotificationsWithEmptyDrugResistanceProfile);

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfilesAsync(
                    It.IsAny<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>()),
                Times.Once);
            Assert.Equal(2, numberOfUpdatesOnLastRun);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Exactly(2));
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_WhenNoUpdatedDrp_NoAlertCreated()
        {
            // Arrange
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns(Task.FromResult<IEnumerable<NotificationForDrugResistanceImport>>(
                    new List<NotificationForDrugResistanceImport>
                    {
                        mockNotificationWithMdr,
                        mockNotificationWithoutMdr
                    }));

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(
                x => x.UpdateDrugResistanceProfilesAsync(
                    It.IsAny<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>()),
                Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_UpdatedOnlyChangedDrpString()
        {
            // Arrange
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns(Task.FromResult<IEnumerable<NotificationForDrugResistanceImport>>(
                    new List<NotificationForDrugResistanceImport>
                    {
                        mockNotificationWithMdr,
                        mockNotificationWithoutMdr
                    }));

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfilesAsync(
                    It.IsAny<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>()),
                Times.Once);
            Assert.Equal(1, numberOfUpdatesOnLastRun);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_UpdatedOnlyChangedDrpSpecies()
        {
            // Arrange
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {3, drpWithMbovis},
                    {4, drpWithMbovis}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns(Task.FromResult<IEnumerable<NotificationForDrugResistanceImport>>(
                    new List<NotificationForDrugResistanceImport>
                    {
                        mockNotificationWithMbovis,
                        mockNotificationWithoutMbovis
                    }));

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfilesAsync(
                    It.IsAny<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>()),
                Times.Once);
            Assert.Equal(1, numberOfUpdatesOnLastRun);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<INotificationForDrugResistanceImport>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfile_OnlyUpdatesConfiguredNumberOfNotifications()
        {
            // Arrange
            var drugResistanceProfiles = Enumerable.Range(1, MaxNumberOfUpdates + 1)
                .ToDictionary(i => i, i => drpWithoutMdr);
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(drugResistanceProfiles));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns<IEnumerable<int>>(GetNotificationsWithMdrProfile);

            // Act
            await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfilesAsync(
                    It.IsAny<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>()),
                Times.Once);
            Assert.Equal(MaxNumberOfUpdates, numberOfUpdatesOnLastRun);
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
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(drugResistanceProfiles));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns<IEnumerable<int>>(GetNotificationsWithMdrProfile);

            // Act
            var notificationsRemaining = await drpService.UpdateDrugResistanceProfiles(MaxNumberOfUpdates);

            // Assert
            Assert.Equal(expectedRemaining, notificationsRemaining);
        }

        [Fact]
        public async Task UpdateDrugResistanceProfileForNotifications_OnlyUpdatesThoseNotifications()
        {
            // Arrange
            mockDrugResistanceProfileRepository.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    { 1, new DrugResistanceProfile { NotificationId = 1, Species = "M. bovis" } },
                    { 2, new DrugResistanceProfile { NotificationId = 2, DrugResistanceProfileString = "RR/MDR/XDR" } },
                    { 3, new DrugResistanceProfile { NotificationId = 3, Species = "M. tuberculosis" } }
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationsForDrugResistanceImportAsync(It.IsAny<IEnumerable<int>>()))
                .Returns<IEnumerable<int>>(GetNotificationsWithEmptyDrugResistanceProfile);

            var notificationsToUpdate = new List<Notification>
            {
                new Notification { NotificationId = 1 },
                new Notification { NotificationId = 3 }
            };

            // Act
            await drpService.UpdateDrugResistanceProfiles(notificationsToUpdate);

            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfilesAsync(
                    It.Is<IEnumerable<(DrugResistanceProfile, DrugResistanceProfile)>>(list =>
                        list.Any(pair => pair.Item2.NotificationId == 1) &&
                        list.Any(pair => pair.Item2.NotificationId == 3) &&
                        !list.Any(pair => pair.Item2.NotificationId == 2)
                    )),
                Times.Once);
        }


        private static Task<IEnumerable<NotificationForDrugResistanceImport>> GetNotificationsWithEmptyDrugResistanceProfile(
            IEnumerable<int> notificationIds)
        {
            return Task.FromResult(notificationIds.Select(id => new NotificationForDrugResistanceImport
            {
                NotificationId = id,
                DrugResistanceProfile = new DrugResistanceProfile()
            }));
        }

        private static Task<IEnumerable<NotificationForDrugResistanceImport>> GetNotificationsWithMdrProfile(
            IEnumerable<int> notificationIds)
        {
            return Task.FromResult(notificationIds.Select(id => new NotificationForDrugResistanceImport
            {
                NotificationId = id,
                DrugResistanceProfile = drpWithMdr
            }));
        }
    }
}
