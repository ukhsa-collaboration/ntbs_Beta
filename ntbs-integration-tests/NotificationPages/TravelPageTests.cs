using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class TravelPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditTravel;

        public TravelPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostDraft_RedirectsToNextPage_IfModelValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "No",
                ["VisitorDetails.HasVisitor"] = "No"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditComorbidities, id), GetRedirectLocation(result));
        }

        [Fact]
        public async Task PostNotified_RedirectsToCorrectSectionOfOverview_IfModelValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "No",
                ["VisitorDetails.HasVisitor"] = "No"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            var sectionAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPath);
            result.AssertRedirectTo($"/Notifications/{id}#{sectionAnchorId}");
        }

        // Rather than testing all possible validation rules in here, we check one to verify the errors
        // are correctly wired up on the page.
        // The more extensive checks are left to unit tests
        [Fact]
        public async Task Post_WithErrors_DisplaysErrorsCorrectly()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),

                ["TravelDetails.HasTravel"] = "Yes",
                ["TravelDetails.TotalNumberOfCountries"] = "3",
                ["TravelDetails.Country1Id"] = "1",
                ["TravelDetails.StayLengthInMonths1"] = "10",
                ["TravelDetails.Country2Id"] = "2",
                ["TravelDetails.StayLengthInMonths2"] = "10",
                ["TravelDetails.Country3Id"] = "3",
                ["TravelDetails.StayLengthInMonths3"] = "10",

                ["VisitorDetails.HasVisitor"] = "Yes",
                ["VisitorDetails.TotalNumberOfCountries"] = "3",
                ["VisitorDetails.Country1Id"] = "1",
                ["VisitorDetails.StayLengthInMonths1"] = "10",
                ["VisitorDetails.Country2Id"] = "2",
                ["VisitorDetails.StayLengthInMonths2"] = "10",
                ["VisitorDetails.Country3Id"] = "3",
                ["VisitorDetails.StayLengthInMonths3"] = "10"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorMessage("travel-length1", "Total duration of travel must not exceed 24 months");
            resultDocument.AssertErrorMessage("travel-length2", "Total duration of travel must not exceed 24 months");
            resultDocument.AssertErrorMessage("travel-length3", "Total duration of travel must not exceed 24 months");
            resultDocument.AssertErrorMessage("visitor-length1", "Total duration of visits must not exceed 24 months");
            resultDocument.AssertErrorMessage("visitor-length2", "Total duration of visits must not exceed 24 months");
            resultDocument.AssertErrorMessage("visitor-length3", "Total duration of visits must not exceed 24 months");
        }

        [Fact]
        public async Task PostNotified_WithLeftOverDataInHiddenInputs_IsValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                // These would normally make the models invalid...
                ["TravelDetails.StayLengthInMonths1"] = "30",
                ["VisitorDetails.StayLengthInMonths1"] = "30",
                // ... but the user says they are not providing these details
                ["TravelDetails.HasTravel"] = "No",
                ["VisitorDetails.HasVisitor"] = "No"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        }

        [Theory]
        [InlineData("HasTravel")]
        [InlineData("HasVisitor")]
        public async Task DynamicValidate_ReturnsOk(string travelOrVisitorKey)
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                [$"TravelDetails.{travelOrVisitorKey}"] = "Yes",
                ["TravelDetails.ShouldValidateFull"] = "false"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateTravel"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task NotifiedPageHasReturnLinkToOverview()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }
    }
}
