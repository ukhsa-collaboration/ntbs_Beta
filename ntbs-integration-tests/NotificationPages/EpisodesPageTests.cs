using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Helpers;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class EpisodesPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditEpisode;

        public EpisodesPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification()
                { 
                    NotificationId = Utilities.NOTIFIED_WITH_TBSERVICE, 
                    NotificationStatus = NotificationStatus.Notified, 
                    Episode = new Episode() 
                    {
                        TBServiceCode = "A code",
                        TBService = new TBService() { Name = "A name" }
                    }
                }
            };
        }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidDate),
                        resultDocument.GetError("notification-date"));
        }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            var expectedMessage = ValidationMessages.TodayOrEarlier;
            resultDocument.AssertErrorMessage("notification-date", expectedMessage);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRequiredError_IfNotificationDateNotSet()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.NotificationDateIsRequired);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.ValidDate);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.TodayOrEarlier);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllRequiredErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString()
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.NotificationDateIsRequired);
            resultDocument.AssertErrorMessage("hospital", ValidationMessages.HospitalIsRequired);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithTextFieldInputError_IfTextIsInvalid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["Episode.Consultant"] = "Name-1",
                ["Episode.CaseManager"] = "Name2,",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("consultant", ValidationMessages.StandardStringFormat);
            resultDocument.AssertErrorMessage("case-manager", ValidationMessages.StandardStringFormat);
        }

        [Fact]
        public async Task PostDraft_RedirectToNextPage_IfModelIsValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);


            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Episode.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["Episode.Consultant"] = "Consultant",
                ["Episode.CaseManager"] = "CaseManager",
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditClinicalDetails, id), GetRedirectLocation(result));
        }

        [Fact]
        public async Task PostDraft_HospitalDoesNotMatchTbService_ReturnsValidationError()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Episode.HospitalId"] = Utilities.HOSPITAL_FLEETWOOD_HOSPITAL_ID,
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID,
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("hospital", ValidationMessages.HospitalMustBelongToSelectedTbSerice);
        }

        [Fact]
        public async Task PostNotified_TBServiceHasChanged_ReturnsValidationError()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_WITH_TBSERVICE);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_WITH_TBSERVICE.ToString(),
                ["Episode.TBServiceCode"] = "ChangedTBServiceCode"
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            // Here we check that the page has reloaded as the form is invalid, we can't check directly for an error as
            // no error is shown for this case
            Assert.NotNull(resultDocument.QuerySelector($"div[id='episode-page-content']"));
        }
    }
}
