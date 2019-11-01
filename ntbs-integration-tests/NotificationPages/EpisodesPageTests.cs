using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class EpisodesPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditEpisode;

        public EpisodesPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

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
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            var expectedMessage = ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate);
            resultDocument.AssertErrorMessage("notification-date", expectedMessage);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRequiredError_IfNotificationDateNotSet()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

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
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

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
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate));
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllRequiredErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString()
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.NotificationDateIsRequired);
            resultDocument.AssertErrorMessage("tb-service", ValidationMessages.TBServiceIsRequired);
            resultDocument.AssertErrorMessage("hospital", ValidationMessages.HospitalIsRequired);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithTextFieldInputError_IfTextIsInvalid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["Episode.Consultant"] = "Name-1",
                ["Episode.CaseManager"] = "Name2,",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

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
            var initialUrl = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(initialUrl);
            var initialDocument = await GetDocumentAsync(initialPage);


            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Episode.HospitalId"] = Utilities.HOSPITAL_FLEETWOOD_HOSPITAL_ID,
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ROYAL_DERBY_HOSPITAL_ID,
                ["Episode.Consultant"] = "Consultant",
                ["Episode.CaseManager"] = "CaseManager",
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, initialUrl);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditClinicalDetails, id), GetRedirectLocation(result));
        }

        [Fact]
        public async Task PostDraft_HospitalDoesNotMatchTbService_ReturnsValdationError()
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
    }
}
