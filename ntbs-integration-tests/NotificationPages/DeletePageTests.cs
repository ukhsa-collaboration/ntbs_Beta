using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using Xunit;
using System;

namespace ntbs_integration_tests.NotificationPages
{
    public class DeletePageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Delete;

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
            var response = await client.GetAsync(GetPageRouteForId(id));

            // Assert
            Assert.Equal(code, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.Equal(BuildRoute(Routes.Overview, id), GetRedirectLocation(response));
            }
        }

        [Fact]
        public async Task Post_DeletesAndDisplaysConfirmationPage_IfModelValidWithNoDescription()
        {
            // Arrange
            const int id = Utilities.DELETE_NO_DESCRIPTION;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);


            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString()
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, "Confirm");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(Routes.DeleteConfirmation, GetRedirectLocation(result));

            var redirectPage = await client.GetAsync(GetRedirectLocation(result));
            var redirectDocument = await GetDocumentAsync(redirectPage);
            Assert.NotNull(redirectDocument?.QuerySelector(".return-to-homepage-link"));
        }

        [Fact]
        public async Task Post_DeletesAndDisplaysConfirmationPage_IfModelValidWithDescription()
        {
            // Arrange
            const int id = Utilities.DELETE_WITH_DESCRIPTION;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            
            const string reason = "A great reason";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["DeletionReason"] = reason
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, "Confirm");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(Routes.DeleteConfirmation, GetRedirectLocation(result));

            var redirectPage = await client.GetAsync(GetRedirectLocation(result));
            var redirectDocument = await GetDocumentAsync(redirectPage);
            Assert.NotNull(redirectDocument?.QuerySelector(".return-to-homepage-link"));
        }

        [Fact]
        public async Task Post_ReturnsPageWithModelErrors_IfReasonHasInvalidCharacters()
        {
            // Arrange
            const int id = Utilities.DELETE_WITH_DESCRIPTION;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            
            const string reason = "A bad reason $#|";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["DeletionReason"] = reason
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.StringWithNumbersAndForwardSlashFormat), resultDocument.GetError("reason"));
        }
    }
}
