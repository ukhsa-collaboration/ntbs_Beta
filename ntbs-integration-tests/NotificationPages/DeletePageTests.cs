using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class DeletePageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.Delete;

        public DeletePageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification(){ NotificationId = Utilities.DELETE_WITH_DESCRIPTION, NotificationStatus = NotificationStatus.Draft },
                new Notification(){ NotificationId = Utilities.DELETE_NO_DESCRIPTION, NotificationStatus = NotificationStatus.Draft },
            };
        }

        public static IEnumerable<object[]> DeleteRoutes()
        {
            yield return new object[] { Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.DENOTIFIED_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.NOTIFIED_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.NEW_ID, HttpStatusCode.NotFound };
        }

        [Theory, MemberData(nameof(DeleteRoutes))]
        public async Task GetDeletePage_ReturnsCorrectStatusCode_DependentOnId(int id, HttpStatusCode code)
        {
            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(id));

            // Assert
            Assert.Equal(code, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.Contains(GetRedirectLocation(response), GetPathForId(NotificationSubPaths.Overview, id));
            }
        }

        [Fact]
        public async Task Post_DeletesAndDisplaysConfirmationPage_IfModelValidWithNoDescription()
        {
            // Arrange
            const int id = Utilities.DELETE_NO_DESCRIPTION;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString()
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");


            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var postFormSubmissionResult = await GetDocumentAsync(result);
            Assert.NotNull(postFormSubmissionResult?.QuerySelector(".return-to-homepage-link"));
        }

        [Fact]
        public async Task Post_DeletesAndDisplaysConfirmationPage_IfModelValidWithDescription()
        {
            // Arrange
            const int id = Utilities.DELETE_WITH_DESCRIPTION;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["DeletionReason"] = "A great reason"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");


            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var postFormSubmissionResult = await GetDocumentAsync(result);
            Assert.NotNull(postFormSubmissionResult?.QuerySelector(".return-to-homepage-link"));
        }

        [Fact]
        public async Task Post_ReturnsPageWithModelErrors_IfReasonHasInvalidCharacters()
        {
            // Arrange
            const int id = Utilities.DELETE_WITH_DESCRIPTION;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["DeletionReason"] = "A bad reason $#|"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("reason", "Deletion reason can only contain letters, numbers and the symbols ' - . , /");
        }
    }
}
