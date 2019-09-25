using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;
using ntbs_service.Pages;
using ntbs_service.Pages_Notifications;
using Xunit;

namespace ntbs_service_tests.UnitTests.Patients
{
    public class IndexPageTest
    {
        private Mock<INotificationService> mockService;
        public IndexPageTest() 
        {
            mockService = new Mock<INotificationService>();
        }

        [Fact]
        public async Task OnGetAsync_PopulatesPageModel_WithRecentAndDraftNotifications()
        {
            // Arrange
            var recent = Task.FromResult(GetRecentNotifications());
            var drafts = Task.FromResult(GetDraftNotifications());
            mockService.Setup(s => s.GetRecentNotificationsAsync(It.IsAny<List<string>>())).Returns(recent);
            mockService.Setup(s => s.GetDraftNotificationsAsync(It.IsAny<List<string>>())).Returns(drafts);           

            var pageModel = new IndexModel(mockService.Object);

            // Act
            await pageModel.OnGetAsync();
            var resultRecent = Assert.IsAssignableFrom<List<Notification>>(pageModel.RecentNotifications);
            var resultDraft = Assert.IsAssignableFrom<List<Notification>>(pageModel.DraftNotifications);

            // Assert
            Assert.True(resultRecent.Count == 1);
            Assert.Equal("Bob", resultRecent[0].PatientDetails.GivenName);
            Assert.True(resultDraft.Count == 1);
            Assert.Equal("Ross", resultDraft[0].PatientDetails.GivenName);
        }

        public IList<Notification> GetRecentNotifications()
        {
            var patient = new PatientDetails() { GivenName = "Bob" };

            return new List<Notification> { new Notification{ PatientDetails = patient } };
        }

        public IList<Notification> GetDraftNotifications()
        {
            var patient = new PatientDetails() { GivenName = "Ross" };

            return new List<Notification> { new Notification{ PatientDetails = patient } };
        }
    }
}
