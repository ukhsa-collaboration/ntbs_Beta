using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
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
            var venueTextContent = resultDocument.GetElementById("social-context-venues-table")
                .GetElementsByTagName("tbody")[0]
                .GetElementsByTagName("tr")[0]
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
            var venueBeforeChanges = editDocument.GetElementById($"social-context-venue-{VENUE_ID}").TextContent;
            Assert.Contains("Test venue", venueBeforeChanges);
            Assert.Contains("Test address", venueBeforeChanges);

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
            var venueTextContent = resultDocument.GetElementById($"social-context-venue-{VENUE_ID}").TextContent;

            Assert.Contains("New venue", venueTextContent);
            Assert.Contains("New address", venueTextContent);
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
            resultDocument.AssertErrorMessage("date-to", ValidationMessages.VenueDateToShouldBeLaterThanDateFrom);
        }

        [Fact]
        public async Task GetEditForMissingVenye_ReturnsNotFound()
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
    }
}
