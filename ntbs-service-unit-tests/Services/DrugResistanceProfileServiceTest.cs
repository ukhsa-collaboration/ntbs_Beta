using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class DrugResistanceProfileServiceTest
    {
        private readonly DrugResistanceProfileService drpService;
        private readonly Mock<INotificationRepository> mockNotificationRepository;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly Mock<IEnhancedSurveillanceAlertsService> mockMdrService;
        private readonly Mock<IDrugResistanceProfileRepository> mockDrugResistanceProfileService;


        private static readonly DrugResistanceProfile drpWithMdr = new DrugResistanceProfile
        {
            Species = "TestSpecies", DrugResistanceProfileString = "RR/MDR/XDR"
        };        
        
        private static readonly DrugResistanceProfile drpWithoutMdr = new DrugResistanceProfile
        {
            Species = "TestSpecies", DrugResistanceProfileString = "No Mdr"
        };
        
        private static readonly DrugResistanceProfile drpWithMbovis = new DrugResistanceProfile
        {
            Species = "M. bovis", DrugResistanceProfileString = "No Mdr"
        };
        
        private static readonly DrugResistanceProfile drpWithoutMbovis = new DrugResistanceProfile
        {
            Species = "No M. bovis", DrugResistanceProfileString = "No Mdr"
        };
        
        private readonly Notification mockNotificationWithMdr =
            new Notification {NotificationId = 1, DrugResistanceProfile = drpWithMdr };
        
        private readonly Notification mockNotificationWithoutMdr =
            new Notification {NotificationId = 2, DrugResistanceProfile = drpWithoutMdr};
        
        private readonly Notification mockNotificationWithMbovis =
            new Notification {NotificationId = 3, DrugResistanceProfile = drpWithMbovis};
        
        private readonly Notification mockNotificationWithoutMbovis =
            new Notification {NotificationId = 4, DrugResistanceProfile = drpWithoutMbovis};

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
        public void UpdateDrugResistanceProfile_DoesNotUpdateRecordIfNotMatching()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {3, drpWithMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(3))
                .Returns(Task.FromResult<Notification>(null));
            
            // Act
            drpService.UpdateDrugResistanceProfiles();
            
            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<Notification>()), Times.Never);
        }
        
        [Fact]
        public void UpdateDrugResistanceProfile_DoesUpdateRecordIfMatching()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(It.IsAny<int>()))
                .Returns<int>(GetNotificationWithEmptyDrugResistanceProfile);    
            
            // Act
            drpService.UpdateDrugResistanceProfiles();
            
            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Exactly(2));
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>()), Times.Exactly(2));
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<Notification>()), Times.Exactly(2));
        }

        [Fact]
        public void UpdateDrugResistanceProfile_WhenNoUpdatedDrp_NoAlertCreated()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));
            
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMdr));           
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithoutMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMdr));
            
            // Act
            drpService.UpdateDrugResistanceProfiles();
            
            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<Notification>()), Times.Never);
        }       
        
        [Fact]
        public void UpdateDrugResistanceProfile_UpdatedOnlyChangedDrpString()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithMdr}
                }));
            
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMdr));           
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithoutMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMdr));
            
            // Act
            drpService.UpdateDrugResistanceProfiles();
            
            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<Notification>()), Times.Once);
        }
        
        [Fact]
        public void UpdateDrugResistanceProfile_UpdatedOnlyChangedDrpSpecies()
        {
            // Arrange
            mockDrugResistanceProfileService.Setup(x => x.GetDrugResistanceProfilesAsync())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {3, drpWithMbovis},
                    {4, drpWithMbovis}
                }));
            
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithMbovis.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMbovis));           
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithoutMbovis.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMbovis));
            
            // Act
            drpService.UpdateDrugResistanceProfiles();
            
            // Assert
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMBovisAlert(It.IsAny<Notification>()), Times.Once);
        }
        
        private Task<Notification> GetNotificationWithEmptyDrugResistanceProfile(int notificationId)
        {
            return Task.FromResult(new Notification
            {
                NotificationId = notificationId, DrugResistanceProfile = new DrugResistanceProfile()
            });
        }
    }
}
