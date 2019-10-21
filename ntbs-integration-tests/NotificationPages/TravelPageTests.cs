using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class TravelPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Travel;

        public TravelPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostDraft_RedirectsToNextPage_IfModelValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "false",
                ["VisitorDetails.HasVisitor"] = "false"
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildEditRoute(Routes.Comorbidities, id), GetRedirectLocation(result));
        }

        [Fact]
        public async Task PostNotified_RedirectsToOverview_IfModelValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "false",
                ["VisitorDetails.HasVisitor"] = "false"
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildRoute(Routes.Overview, id), GetRedirectLocation(result));
        }

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.NOTIFIED_ID)]
        public async Task Post_ReturnsPageWithModelErrors_IfTotalNumberIsLessThanInputCountries(int id)
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

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
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelOrVisitTotalNumberOfCountriesGreaterThanInputNumber),
                resultDocument.QuerySelector("span[id='travel-total-error']").TextContent);

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelOrVisitTotalNumberOfCountriesGreaterThanInputNumber),
                resultDocument.QuerySelector("span[id='visitor-total-error']").TextContent);
        }

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.NOTIFIED_ID)]
        public async Task Post_ReturnsPageWithModelErrors_IfTotalDurationIsMoreThan24(int id)
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

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
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelTotalDurationWithinLimit),
                resultDocument.QuerySelector("span[id='travel-length1-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelTotalDurationWithinLimit),
                resultDocument.QuerySelector("span[id='travel-length2-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelTotalDurationWithinLimit),
                resultDocument.QuerySelector("span[id='travel-length3-error']").TextContent);

            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitTotalDurationWithinLimit),
                resultDocument.QuerySelector("span[id='visitor-length1-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitTotalDurationWithinLimit),
                resultDocument.QuerySelector("span[id='visitor-length2-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitTotalDurationWithinLimit),
                resultDocument.QuerySelector("span[id='visitor-length3-error']").TextContent);
        }

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.NOTIFIED_ID)]
        public async Task Post_ReturnsPageWithModelErrors_IfCountriesAreNonUnique(int id)
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

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
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelUniqueCountry),
                resultDocument.QuerySelector("span[id='travel-country2Id-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelUniqueCountry),
                resultDocument.QuerySelector("span[id='travel-country3Id-error']").TextContent);

            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitUniqueCountry),
                resultDocument.QuerySelector("span[id='visitor-country2Id-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitUniqueCountry),
                resultDocument.QuerySelector("span[id='visitor-country3Id-error']").TextContent);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithModelErrors_RequiredFields()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["VisitorDetails.HasVisitor"] = "true"
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelOrVisitTotalNumberOfCountriesRequired),
                resultDocument.QuerySelector("span[id='travel-total-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelMostRecentCountryRequired),
                resultDocument.QuerySelector("span[id='travel-country1Id-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelCountryRequiresDuration),
                resultDocument.QuerySelector("span[id='travel-length1-error']").TextContent);

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelOrVisitTotalNumberOfCountriesRequired),
                resultDocument.QuerySelector("span[id='visitor-total-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitMostRecentCountryRequired),
                resultDocument.QuerySelector("span[id='visitor-country1Id-error']").TextContent);
            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitCountryRequiresDuration),
                resultDocument.QuerySelector("span[id='visitor-length1-error']").TextContent);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithModelErrors_CountriesOutOfOrder()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

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
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelIsChronological),
                resultDocument.QuerySelector("span[id='travel-country3Id-error']").TextContent);

            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitIsChronological),
                resultDocument.QuerySelector("span[id='visitor-country3Id-error']").TextContent);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithModelErrors_DurationNoCountry()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

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
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelOrVisitDurationHasCountry),
                resultDocument.QuerySelector("span[id='travel-length2-error']").TextContent);

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelOrVisitDurationHasCountry),
                resultDocument.QuerySelector("span[id='visitor-length2-error']").TextContent);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithModelErrors_CountryNoDuration()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialPage = await client.GetAsync(GetPageRouteForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.TotalNumberOfCountries"] = "2",
                ["TravelDetails.Country1Id"] = "1",
                ["TravelDetails.StayLengthInMonths1"] = "1",
                ["TravelDetails.Country2Id"] = "2",
                ["TravelDetails.StayLengthInMonths2"] = "",

                ["VisitorDetails.HasVisitor"] = "true",
                ["VisitorDetails.TotalNumberOfCountries"] = "2",
                ["VisitorDetails.Country1Id"] = "1",
                ["VisitorDetails.StayLengthInMonths1"] = "1",
                ["VisitorDetails.Country2Id"] = "2",
                ["VisitorDetails.StayLengthInMonths2"] = "",
            };

            // Act
            var result = await SendFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(
                FullErrorMessage(ValidationMessages.TravelCountryRequiresDuration),
                resultDocument.QuerySelector("span[id='travel-length2-error']").TextContent);

            Assert.Equal(
                FullErrorMessage(ValidationMessages.VisitCountryRequiresDuration),
                resultDocument.QuerySelector("span[id='visitor-length2-error']").TextContent);
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
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateTravel"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task ValidateFullTravel_ReturnsError_RequiredFields()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["TravelDetails.HasTravel"] = "true",
                ["TravelDetails.ShouldValidateFull"] = "true"
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateTravel"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains(ValidationMessages.TravelOrVisitTotalNumberOfCountriesRequired, result);
            Assert.Contains(ValidationMessages.TravelMostRecentCountryRequired, result);
            Assert.Contains(ValidationMessages.TravelCountryRequiresDuration, result);
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
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateVisitor"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task ValidateFullVisit_ReturnsError_RequiredFields()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["VisitorDetails.HasVisitor"] = "true",
                ["VisitorDetails.ShouldValidateFull"] = "true"
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateVisitor"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains(ValidationMessages.TravelOrVisitTotalNumberOfCountriesRequired, result);
            Assert.Contains(ValidationMessages.VisitMostRecentCountryRequired, result);
            Assert.Contains(ValidationMessages.VisitCountryRequiresDuration, result);
        }
    }
}
