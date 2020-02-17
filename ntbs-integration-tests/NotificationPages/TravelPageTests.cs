using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
                ["TravelDetails.HasTravel"] = "false",
                ["VisitorDetails.HasVisitor"] = "false"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditComorbidities, id), GetRedirectLocation(result));
        }

        [Fact]
        public async Task PostNotified_Redirects_IfModelValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "false",
                ["VisitorDetails.HasVisitor"] = "false"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        }

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.NOTIFIED_ID)]
        public async Task Post_ReturnsPageWithModelErrors_IfTotalNumberIsLessThanInputCountries(int id)
        {
            // Arrange
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.TotalNumberOfCountries"] = "1",
                ["TravelDetails.Country1Id"] = "1",
                ["TravelDetails.Country2Id"] = "2",

                ["VisitorDetails.HasVisitor"] = "true",
                ["VisitorDetails.TotalNumberOfCountries"] = "1",
                ["VisitorDetails.Country1Id"] = "1",
                ["VisitorDetails.Country2Id"] = "2",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage("Number of countries entered exceeds total number of countries travelled to"),
                resultDocument.GetError("travel-total"));

            Assert.Equal(
                FullErrorMessage("Number of countries entered exceeds total number of countries visited from"),
                resultDocument.GetError("visitor-total"));
        }

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.NOTIFIED_ID)]
        public async Task Post_ReturnsPageWithModelErrors_IfTotalDurationIsMoreThan24(int id)
        {
            // Arrange
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.TotalNumberOfCountries"] = "3",
                ["TravelDetails.Country1Id"] = "1",
                ["TravelDetails.StayLengthInMonths1"] = "10",
                ["TravelDetails.Country2Id"] = "2",
                ["TravelDetails.StayLengthInMonths2"] = "10",
                ["TravelDetails.Country3Id"] = "3",
                ["TravelDetails.StayLengthInMonths3"] = "10",

                ["VisitorDetails.HasVisitor"] = "true",
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

            Assert.Equal(
                FullErrorMessage("Total duration of travel must not exceed 24 months"),
                resultDocument.GetError("travel-length1"));
            Assert.Equal(
                FullErrorMessage("Total duration of travel must not exceed 24 months"),
                resultDocument.GetError("travel-length2"));
            Assert.Equal(
                FullErrorMessage("Total duration of travel must not exceed 24 months"),
                resultDocument.GetError("travel-length3"));

            Assert.Equal(
                FullErrorMessage("Total duration of visits must not exceed 24 months"),
                resultDocument.GetError("visitor-length1"));
            Assert.Equal(
                FullErrorMessage("Total duration of visits must not exceed 24 months"),
                resultDocument.GetError("visitor-length2"));
            Assert.Equal(
                FullErrorMessage("Total duration of visits must not exceed 24 months"),
                resultDocument.GetError("visitor-length3"));
        }

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.NOTIFIED_ID)]
        public async Task Post_ReturnsPageWithModelErrors_IfCountriesAreNonUnique(int id)
        {
            // Arrange
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.TotalNumberOfCountries"] = "3",
                ["TravelDetails.Country1Id"] = "1",
                ["TravelDetails.Country2Id"] = "1",
                ["TravelDetails.Country3Id"] = "1",

                ["VisitorDetails.HasVisitor"] = "true",
                ["VisitorDetails.TotalNumberOfCountries"] = "3",
                ["VisitorDetails.Country1Id"] = "1",
                ["VisitorDetails.Country2Id"] = "1",
                ["VisitorDetails.Country3Id"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage("Multiple visits to same country - record as single period of travel"),
                resultDocument.GetError("travel-country2Id"));
            Assert.Equal(
                FullErrorMessage("Multiple visits to same country - record as single period of travel"),
                resultDocument.GetError("travel-country3Id"));

            Assert.Equal(
                FullErrorMessage("Multiple visits from same country - record as single visit"),
                resultDocument.GetError("visitor-country2Id"));
            Assert.Equal(
                FullErrorMessage("Multiple visits from same country - record as single visit"),
                resultDocument.GetError("visitor-country3Id"));
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithModelErrors_CountriesOutOfOrder()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.TotalNumberOfCountries"] = "2",
                ["TravelDetails.Country1Id"] = "1",
                ["TravelDetails.StayLengthInMonths1"] = "1",
                ["TravelDetails.Country3Id"] = "2",

                ["VisitorDetails.HasVisitor"] = "true",
                ["VisitorDetails.TotalNumberOfCountries"] = "2",
                ["VisitorDetails.Country1Id"] = "1",
                ["VisitorDetails.StayLengthInMonths1"] = "1",
                ["VisitorDetails.Country3Id"] = "2",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage("Travel must be recorded in chronological order"),
                resultDocument.GetError("travel-country3Id"));

            Assert.Equal(
                FullErrorMessage("Visits must be recorded in chronological order"),
                resultDocument.GetError("visitor-country3Id"));
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithModelErrors_DurationNoCountry()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.TotalNumberOfCountries"] = "2",
                ["TravelDetails.Country1Id"] = "1",
                ["TravelDetails.StayLengthInMonths1"] = "1",
                ["TravelDetails.Country2Id"] = "",
                ["TravelDetails.StayLengthInMonths2"] = "1",

                ["VisitorDetails.HasVisitor"] = "true",
                ["VisitorDetails.TotalNumberOfCountries"] = "2",
                ["VisitorDetails.Country1Id"] = "1",
                ["VisitorDetails.StayLengthInMonths1"] = "1",
                ["VisitorDetails.Country2Id"] = "",
                ["VisitorDetails.StayLengthInMonths2"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage("Duration cannot be added without a corresponding country"),
                resultDocument.GetError("travel-length2"));

            Assert.Equal(
                FullErrorMessage("Duration cannot be added without a corresponding country"),
                resultDocument.GetError("visitor-length2"));
        }

        [Fact]
        public async Task ValidateNotFullTravel_ReturnsExpectedResult()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.ShouldValidateFull"] = "false"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateTravel"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task ValidateNotFullVisit_ReturnsExpectedResult()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["VisitorDetails.HasVisitor"] = "true",
                ["VisitorDetails.ShouldValidateFull"] = "false"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateVisitor"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Empty(result);
        }
        
        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment_ForNotified()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "false",
                ["VisitorDetails.HasVisitor"] = "false"
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var sectionAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPath);
            result.AssertRedirectTo($"/Notifications/{id}#{sectionAnchorId}");
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
