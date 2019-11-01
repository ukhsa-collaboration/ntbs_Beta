using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class EpisodesPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Episode;

        public EpisodesPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.ValidDate);
        }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

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
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.NotificationDateIsRequired);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.ValidDate);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate));
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllRequiredErrors_IfModelNotValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString()
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

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
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["Episode.Consultant"] = "Name-1",
                ["Episode.CaseManager"] = "Name2,",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

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
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(initialPage);

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
            var result = await SendPostFormWithData(document, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildEditRoute(Routes.ClinicalDetails, Utilities.DRAFT_ID), GetRedirectLocation(result));
        }

        [Fact]
        public async Task PostDraft_HospitalDoesNotMatchTbService_ReturnsValdationError()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Episode.HospitalId"] = Utilities.HOSPITAL_FLEETWOOD_HOSPITAL_ID,
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID,
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("hospital", ValidationMessages.HospitalMustBelongToSelectedTbSerice);
        }
    }
}