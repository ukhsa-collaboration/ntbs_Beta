using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.ReferenceEntities;
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
            var initialDocument = await GetDocumentForUrl(url);

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
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date does not have a valid date selection");
        }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

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
            var expectedMessage = "Notification date must not be before 01/01/2010";
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", expectedMessage);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRequiredError_IfNotificationDateNotSet()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date is a mandatory field");
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

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
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date does not have a valid date selection");
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

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
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date must not be before 01/01/2010");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllRequiredErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString()
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date is a mandatory field");
            resultDocument.AssertErrorSummaryMessage("Episode-HospitalId", "hospital", "Hospital is a mandatory field");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithTextFieldInputError_IfTextIsInvalid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["Episode.Consultant"] = "Name-1"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Episode-Consultant", "consultant", "Consultant can only contain letters and the symbols ' - . ,");
        }

        [Fact]
        public async Task PostDraft_RedirectToNextPage_IfModelIsValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["Episode.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["Episode.Consultant"] = "Consultant",
                ["Episode.CaseManagerUsername"] = Utilities.CASEMANAGER_ABINGDON_EMAIL,
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
        public async Task PostNotifiedChangingCaseManager_RedirectToNextPage_IfModelIsValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["Episode.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["Episode.Consultant"] = "Consultant",
                ["Episode.CaseManagerEmail"] = Utilities.CASEMANAGER_ABINGDON_EMAIL2,
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            // Actual and expected flipped to accomodate trailing slash differences
            Assert.Contains(GetRedirectLocation(result), GetPathForId(NotificationSubPaths.Overview, id));
        }

        [Fact]
        public async Task PostDraft_HospitalDoesNotMatchTbService_ReturnsValidationError()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Episode.HospitalId"] = Utilities.HOSPITAL_FLEETWOOD_HOSPITAL_ID,
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID,
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Episode-HospitalId", "hospital", "Hospital must belong to selected TB Service");
        }

        [Fact]
        public async Task PostDraft_CaseManagerDoesNotMatchTbService_ReturnsValidationError()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Episode.TBServiceCode"] = Utilities.TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID,
                ["Episode.CaseManagerUsername"] = Utilities.CASEMANAGER_ABINGDON_EMAIL
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Episode-CaseManagerUsername", "case-manager", "Case Manager must be allowed for selected TB Service");
        }

        [Fact]
        public async Task PostNotified_TBServiceHasChanged_ReturnsValidationError()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_WITH_TBSERVICE);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_WITH_TBSERVICE.ToString(),
                ["Episode.TBServiceCode"] = "ChangedTBServiceCode"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            // Here we check that the page has reloaded as the form is invalid, we can't check directly for an error as
            // no error is shown for this case
            Assert.NotNull(resultDocument.QuerySelector($"div[id='episode-page-content']"));
        }

        [Fact]
        public async Task GetFilteredListsByTbService_ReturnsExpectedValues()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "GetFilteredListsByTbService"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var filteredLists = JsonConvert.DeserializeObject<FilteredEpisodePageSelectLists>(result);
            Assert.Equal(Utilities.CASEMANAGER_ABINGDON_EMAIL, filteredLists.CaseManagers.First().Value);
            Assert.Equal(Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID, filteredLists.Hospitals.First().Value.ToUpperInvariant());
        }
    }
}
