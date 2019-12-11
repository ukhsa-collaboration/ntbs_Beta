using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class SocialContextVenueEditPageTests : TestRunnerNotificationBase
    {
        const int VENUE_ID = 10;
        const int VENUE_TO_DELETE_ID = 11;
        protected override string NotificationSubPath => NotificationSubPaths.EditSocialContextVenueSubPath;

        public SocialContextVenueEditPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_VENUES,
                    NotificationStatus = NotificationStatus.Notified,
                    SocialContextVenues = new List<SocialContextVenue> () {
                        new SocialContextVenue {
                            SocialContextVenueId = VENUE_ID,
                            DateFrom = new DateTime(2012, 1, 1),
                            DateTo = new DateTime(2013, 1, 1),
                            Name = "Test venue",
                            Address = "Test address",
                            VenueTypeId = 1
                        },
                        new SocialContextVenue {
                            SocialContextVenueId = VENUE_TO_DELETE_ID,
                            DateFrom = new DateTime(2012, 1, 1),
                            DateTo = new DateTime(2013, 1, 1),
                            Name = "Test venue 2",
                            Address = "Test address 2",
                            VenueTypeId = 2
                        },
                    }
                }
            };
        }

        [Fact]
        public async Task PostNewVenue_ReturnsSuccessAndAddsResultToTable()
        {
            // Arrange
            const int notificationId = Utilities.DRAFT_ID;
            var url = GetPathForId(NotificationSubPaths.AddSocialContextVenue, notificationId);
            var initialDocument = await GetDocumentForUrl(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedDateFrom.Day"] = "1",
                ["FormattedDateFrom.Month"] = "1",
                ["FormattedDateFrom.Year"] = "1999",
                ["FormattedDateTo.Day"] = "1",
                ["FormattedDateTo.Month"] = "1",
                ["FormattedDateTo.Year"] = "2000",
                ["Venue.Name"] = "Club",
                ["Venue.Address"] = "123 Fake Street",
                ["Venue.Frequency"] = ((int)Frequency.Weekly).ToString(),
                ["Venue.VenueTypeId"] = "1"
            };
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditSocialContextVenues, notificationId));
            var socialContextVenuesPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var resultDocument = await GetDocumentAsync(socialContextVenuesPage);
            // We can't pick based on id, as we don't know the id created
            var venueTextContent = resultDocument.GetElementById("social-context-venues-list")
                .TextContent;

            Assert.Contains("Club", venueTextContent);
            Assert.Contains("123 Fake Street", venueTextContent);
            Assert.Contains("Weekly", venueTextContent);
        }

        [Fact]
        public async Task PostEditOfVenue_ReturnsSuccessAndAmendsResultInTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_VENUES;
            var editUrl = GetCurrentPathForId(notificationId) + VENUE_ID;

            var editPage = await Client.GetAsync(editUrl);
            var editDocument = await GetDocumentAsync(editPage);
            var venueHeadingBeforeChanges = editDocument.GetElementById($"venue-heading-{VENUE_ID}").TextContent;
            var venueBodyBeforeChanges = editDocument.GetElementById($"venue-body-{VENUE_ID}").TextContent;
            Assert.Contains("Test venue", venueHeadingBeforeChanges);
            Assert.Contains("Test address", venueBodyBeforeChanges);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedDateFrom.Day"] = "1",
                ["FormattedDateFrom.Month"] = "1",
                ["FormattedDateFrom.Year"] = "1999",
                ["FormattedDateTo.Day"] = "1",
                ["FormattedDateTo.Month"] = "1",
                ["FormattedDateTo.Year"] = "2000",
                ["Venue.Name"] = "New venue",
                ["Venue.Address"] = "New address",
                ["Venue.VenueTypeId"] = "1"
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditSocialContextVenues, notificationId));
            var socialContextVenuesPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var resultDocument = await GetDocumentAsync(socialContextVenuesPage);
            var venueHeadingTextContent = resultDocument.GetElementById($"venue-heading-{VENUE_ID}").TextContent;
            var venueBodyTextContent = resultDocument.GetElementById($"venue-body-{VENUE_ID}").TextContent;

            Assert.Contains("New venue", venueHeadingTextContent);
            Assert.Contains("New address", venueBodyTextContent);
        }

        [Fact]
        public async Task PostEditOfVenueWithMissingFields_ReturnsAllRequiredValidationErrors()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_VENUES;
            var editUrl = GetCurrentPathForId(notificationId) + VENUE_ID;
            var editDocument = await GetDocumentForUrl(editUrl);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedDateFrom.Day"] = "",
                ["FormattedDateFrom.Month"] = "",
                ["FormattedDateFrom.Year"] = "",
                ["FormattedDateTo.Day"] = "",
                ["FormattedDateTo.Month"] = "",
                ["FormattedDateTo.Year"] = "",
                ["Venue.Name"] = "",
                ["Venue.Address"] = "",
                ["Venue.VenueTypeId"] = ""
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();

            resultDocument.AssertErrorMessage("venue-type", string.Format(ValidationMessages.RequiredSelect, "Venue type"));
            resultDocument.AssertErrorMessage("venue-name", string.Format(ValidationMessages.RequiredEnter, "Venue name"));
            resultDocument.AssertErrorMessage("address", string.Format(ValidationMessages.RequiredEnter, "Address"));
            resultDocument.AssertErrorMessage("date-from", string.Format(ValidationMessages.RequiredEnter, "From"));
            resultDocument.AssertErrorMessage("date-to", string.Format(ValidationMessages.RequiredEnter, "To"));
        }

        [Fact]
        public async Task PostEditWithDateFromBeforeDateTo_ReturnsErrors()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_VENUES;
            var editUrl = GetCurrentPathForId(notificationId) + VENUE_ID;
            var editDocument = await GetDocumentForUrl(editUrl);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedDateFrom.Day"] = "1",
                ["FormattedDateFrom.Month"] = "1",
                ["FormattedDateFrom.Year"] = "2000",
                ["FormattedDateTo.Day"] = "1",
                ["FormattedDateTo.Month"] = "1",
                ["FormattedDateTo.Year"] = "1999",
                ["Venue.Name"] = "New venue",
                ["Venue.Address"] = "New address",
                ["Venue.VenueTypeId"] = "1"
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();
            resultDocument.AssertErrorMessage("date-to", "To must be later than date from");
        }

        [Fact]
        public async Task GetEditForMissingVenue_ReturnsNotFound()
        {
            // Arrange
            const int notificationId = Utilities.DRAFT_ID;
            var editUrl = GetCurrentPathForId(notificationId) + VENUE_ID;

            // Act
            var editPage = await Client.GetAsync(editUrl);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, editPage.StatusCode);
        }

        [Fact]
        public async Task PostDelete_ReturnsSuccessAndRemovesResult()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_VENUES;
            var editUrl = GetCurrentPathForId(notificationId) + VENUE_TO_DELETE_ID;
            var editDocument = await GetDocumentForUrl(editUrl);

            // Act
            var formData = new Dictionary<string, string> { };
            var result = await SendPostFormWithData(editDocument, formData, editUrl, "Delete");

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditSocialContextVenues, notificationId));
            var socialContextVenuesPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var resultDocument = await GetDocumentAsync(socialContextVenuesPage);
            Assert.Null(resultDocument.GetElementById($"social-context-venue-{VENUE_TO_DELETE_ID}"));
        }

        [Fact]
        public async Task ValidateVenueDates_ReturnsErrorIfDateToBeforeDateFrom()
        {
            // Arrange
            var keyValuePairs = new string[]
            {
                "keyValuePairs[0][key]=DateFrom",
                "keyValuePairs[0][day]=1",
                "keyValuePairs[0][month]=1",
                "keyValuePairs[0][year]=2000",
                "keyValuePairs[1][key]=DateTo",
                "keyValuePairs[1][day]=1",
                "keyValuePairs[1][month]=1",
                "keyValuePairs[1][year]=1999",
            };

            // Act
            var url = GetCurrentPathForId(0) + VENUE_ID;
            var response = await Client.GetAsync($"{url}/ValidateVenueDates?{string.Join("&", keyValuePairs)}");

            // Assert check just response.Content
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("To must be later than date from", result);
        }
    }
}
