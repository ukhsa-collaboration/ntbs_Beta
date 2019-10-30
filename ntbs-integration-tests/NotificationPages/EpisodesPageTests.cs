using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class EpisodesPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Episode;

        public EpisodesPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

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
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidDate), 
                        resultDocument.GetError("notification-date"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate)), 
                        resultDocument.GetError("notification-date"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.NotificationDateIsRequired), 
                        resultDocument.GetError("notification-date"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidDate), 
                        resultDocument.GetError("notification-date"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate)), 
                        resultDocument.GetError("notification-date"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.NotificationDateIsRequired), 
                        resultDocument.GetError("notification-date"));
            Assert.Equal(ValidationMessages.TBServiceIsRequired, resultDocument.GetError("tb-service"));
            Assert.Equal(ValidationMessages.HospitalIsRequired, resultDocument.GetError("hospital"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), 
                        resultDocument.GetError("consultant"));
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), 
                        resultDocument.GetError("case-manager"));
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
                ["Episode.HospitalId"] = "1EE2B39A-428F-44C7-B4BB-000649636591",
                ["Episode.TBServiceCode"] = "TBS0181",
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


    }
}