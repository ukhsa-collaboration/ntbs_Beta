using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Pages_Patients;
using Xunit;

namespace ntbs_service_tests.UnitTests.Patients
{
    public class IndexPageTest
    {
        private Mock<INotificationRepository> mockRepository;
        public IndexPageTest() 
        {
            mockRepository = new Mock<INotificationRepository>();
        }

        [Fact]
        public async Task OnGetAsync_PopulatesPageModel_WithPatients()
        {
            // Arrange
            mockRepository.Setup(rep => rep.GetNotificationsWithPatientsAsync())
                                 .Returns(Task.FromResult(GetSamplePatients()));

            var pageModel = new IndexModel(mockRepository.Object);

            // Act
            await pageModel.OnGetAsync();
            var result = Assert.IsAssignableFrom<List<Notification>>(pageModel.Notifications);
            Assert.True(result.Count == 1);
            Assert.Equal("Bob", result[0].PatientDetails.GivenName);
        }

        public IList<Notification> GetSamplePatients()
        {
            var patient = new PatientDetails() { GivenName = "Bob" };

            return new List<Notification> { new Notification{ PatientDetails = patient } };
        }
    }
}
