using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class DenotifyPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Denotify;

        public DenotifyPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification(){ NotificationId = Utilities.DENOTIFY_WITH_DESCRIPTION, NotificationStatus = NotificationStatus.Notified },
                new Notification(){ NotificationId = Utilities.DENOTIFY_NO_DESCRIPTION, NotificationStatus = NotificationStatus.Notified }
            };
        }

        public static IEnumerable<object[]> DenotifyRoutes()
        {
            yield return new object[] { Utilities.DRAFT_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.DENOTIFIED_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.NOTIFIED_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.NEW_ID, HttpStatusCode.NotFound };
        }

        [Theory, MemberData(nameof(DenotifyRoutes))]
        public async Task GetDenotifyPage_ReturnsCorrectStatusCode_DependentOnId(int id, HttpStatusCode code)
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
        public async Task Post_RedirectsToNextPageAndDenotifies_IfModelValidWithNoDescription()
        {
            // Arrange
            const int id = Utilities.DENOTIFY_NO_DESCRIPTION;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string denotifyDateDay = "1";
            const string denotifyDateMonth = "1";
            const string denotifyDateYear = "2000";
            const string reason = "DuplicateEntry";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedDenotificationDate.Day"] = denotifyDateDay,
                ["FormattedDenotificationDate.Month"] = denotifyDateMonth,
                ["FormattedDenotificationDate.Year"] = denotifyDateYear,
                ["DenotificationDetails.Reason"] = reason
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData, "Confirm");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildRoute(Routes.Overview, id), GetRedirectLocation(result));

            var redirectPage = await client.GetAsync(GetRedirectLocation(result));
            var redirectDocument = await GetDocumentAsync(redirectPage);
            Assert.Contains("notification-banner--denotified", redirectDocument.QuerySelector("dl.notification-banner")?.ClassList);
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndDenotifies_IfModelValidWithDescription()
        {
            // Arrange
            const int id = Utilities.DENOTIFY_WITH_DESCRIPTION;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string denotifyDateDay = "1";
            const string denotifyDateMonth = "1";
            const string denotifyDateYear = "2000";
            const string reason = "DuplicateEntry";
            const string description = "Test Description";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedDenotificationDate.Day"] = denotifyDateDay,
                ["FormattedDenotificationDate.Month"] = denotifyDateMonth,
                ["FormattedDenotificationDate.Year"] = denotifyDateYear,
                ["DenotificationDetails.Reason"] = reason,
                ["DenotificationDetails.OtherDescription"] = description
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData, "Confirm");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildRoute(Routes.Overview, id), GetRedirectLocation(result));

            var redirectPage = await client.GetAsync(GetRedirectLocation(result));
            var redirectDocument = await GetDocumentAsync(redirectPage);
            Assert.Contains("notification-banner--denotified", redirectDocument.QuerySelector("dl.notification-banner")?.ClassList);
        }

        [Fact]
        public async Task Post_ReturnsPageWithModelErrors_IfDateInvalid()
        {
            // Arrange
            var id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string denotifyDateDay = "0";
            const string denotifyDateMonth = "1";
            const string denotifyDateYear = "2000";
            const string reason = "DuplicateEntry";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedDenotificationDate.Day"] = denotifyDateDay,
                ["FormattedDenotificationDate.Month"] = denotifyDateMonth,
                ["FormattedDenotificationDate.Year"] = denotifyDateYear,
                ["DenotificationDetails.Reason"] = reason
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidDate), resultDocument.QuerySelector("span[id='date-error']").TextContent);
        }

        [Fact]
        public async Task Post_ReturnsPageWithModelErrors_IfDateInFuture()
        {
            // Arrange
            var id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string denotifyDateDay = "1";
            const string denotifyDateMonth = "1";
            const string denotifyDateYear = "2099";
            const string reason = "DuplicateEntry";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedDenotificationDate.Day"] = denotifyDateDay,
                ["FormattedDenotificationDate.Month"] = denotifyDateMonth,
                ["FormattedDenotificationDate.Year"] = denotifyDateYear,
                ["DenotificationDetails.Reason"] = reason
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.DenotificationDateLatestToday), resultDocument.QuerySelector("span[id='date-error']").TextContent);
        }

        [Fact(Skip = "Requires NotificationDate to be implemented")]
        public async Task Post_ReturnsPageWithModelErrors_IfDateIsBeforeDateOfNotification()
        {
            // Arrange
            var id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string denotifyDateDay = "1";
            const string denotifyDateMonth = "1";
            const string denotifyDateYear = "1910";
            const string reason = "DuplicateEntry";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedDenotificationDate.Day"] = denotifyDateDay,
                ["FormattedDenotificationDate.Month"] = denotifyDateMonth,
                ["FormattedDenotificationDate.Year"] = denotifyDateYear,
                ["DenotificationDetails.Reason"] = reason
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.DenotificationDateAfterNotification), resultDocument.QuerySelector("span[id='date-error']").TextContent);
        }

        [Fact]
        public async Task Post_ReturnsPageWithModelErrors_IfMissingReason()
        {
            // Arrange
            var id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string denotifyDateDay = "1";
            const string denotifyDateMonth = "1";
            const string denotifyDateYear = "2000";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedDenotificationDate.Day"] = denotifyDateDay,
                ["FormattedDenotificationDate.Month"] = denotifyDateMonth,
                ["FormattedDenotificationDate.Year"] = denotifyDateYear
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.DenotificationReasonRequired), resultDocument.QuerySelector("span[id='reason-error']").TextContent);
        }

        [Fact]
        public async Task Post_ReturnsPageWithModelErrors_IfReasonOtherMissingDescription()
        {
            // Arrange
            var id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string denotifyDateDay = "1";
            const string denotifyDateMonth = "1";
            const string denotifyDateYear = "2000";
            const string reason = "Other";

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedDenotificationDate.Day"] = denotifyDateDay,
                ["FormattedDenotificationDate.Month"] = denotifyDateMonth,
                ["FormattedDenotificationDate.Year"] = denotifyDateYear,
                ["DenotificationDetails.Reason"] = reason
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.DenotificationReasonOtherRequired), resultDocument.QuerySelector("span[id='description-error']").TextContent);
        }
    }
}
