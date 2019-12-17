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
    public class SocialContextAddressEditPageTests : TestRunnerNotificationBase
    {
        const int ADDRESS_ID = 10;
        const int ADDRESS_TO_DELETE_ID = 11;
        protected override string NotificationSubPath => NotificationSubPaths.EditSocialContextAddressSubPath;

        public SocialContextAddressEditPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_ADDRESSES,
                    NotificationStatus = NotificationStatus.Notified,
                    SocialContextAddresses = new List<SocialContextAddress> () {
                        new SocialContextAddress {
                            SocialContextAddressId = ADDRESS_ID,
                            DateFrom = new DateTime(2012, 1, 1),
                            DateTo = new DateTime(2013, 1, 1),
                            Address = "Softwire London",
                            Postcode = "NW5 1TL"
                        },
                        new SocialContextAddress {
                            SocialContextAddressId = ADDRESS_TO_DELETE_ID,
                            DateFrom = new DateTime(2012, 1, 1),
                            DateTo = new DateTime(2013, 1, 1),
                            Address = "Softwire Manchester",
                            Postcode = "M4 4BF"
                        },
                    }
                }
            };
        }

        [Fact]
        public async Task PostNewAddress_ReturnsSuccessAndAddsResultToTable()
        {
            // Arrange
            const int notificationId = Utilities.DRAFT_ID;
            var url = GetPathForId(NotificationSubPaths.AddSocialContextAddress, notificationId);
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
                ["Address.Address"] = "123 Fake Street",
                ["Address.Postcode"] = "M4 4BF"
            };
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var socialContextAddressesPage = await AssertAndFollowRedirect(result, GetPathForId(NotificationSubPaths.EditSocialContextAddresses, notificationId));
            // Follow the redirect to see results table
            var resultDocument = await GetDocumentAsync(socialContextAddressesPage);
            // We can't pick based on id, as we don't know the id created
            var addressTextContent = resultDocument.GetElementById("social-context-addresses-list")
                .TextContent;

            Assert.Contains("123 Fake Street", addressTextContent);
            Assert.Contains("M4 4BF", addressTextContent);
        }

        [Fact]
        public async Task PostEditOfAddress_ReturnsSuccessAndAmendsResultInTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_ADDRESSES;
            var editUrl = GetCurrentPathForId(notificationId) + ADDRESS_ID;

            var editPage = await Client.GetAsync(editUrl);
            var editDocument = await GetDocumentAsync(editPage);
            var addressBodyBeforeChanges = editDocument.GetElementById($"address-body-{ADDRESS_ID}").TextContent;
            Assert.Contains("Softwire London", addressBodyBeforeChanges);
            Assert.Contains("NW5 1TL", addressBodyBeforeChanges);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedDateFrom.Day"] = "1",
                ["FormattedDateFrom.Month"] = "1",
                ["FormattedDateFrom.Year"] = "1999",
                ["FormattedDateTo.Day"] = "1",
                ["FormattedDateTo.Month"] = "1",
                ["FormattedDateTo.Year"] = "2000",
                ["Address.Address"] = "New address",
                ["Address.Postcode"] = "M4 4BF"
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);

            // Assert
            var socialContextAddressesPage = await AssertAndFollowRedirect(result, GetPathForId(NotificationSubPaths.EditSocialContextAddresses, notificationId));
            // Follow the redirect to see results table
            var resultDocument = await GetDocumentAsync(socialContextAddressesPage);
            var addressBodyTextContent = resultDocument.GetElementById($"address-body-{ADDRESS_ID}").TextContent;

            Assert.Contains("New address", addressBodyTextContent);
            Assert.Contains("M4 4BF", addressBodyTextContent);
        }

        [Fact]
        public async Task PostEditOfAddressWithMissingFields_ReturnsAllRequiredValidationErrors()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_ADDRESSES;
            var editUrl = GetCurrentPathForId(notificationId) + ADDRESS_ID;
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
                ["Address.Address"] = "",
                ["Address.Postcode"] = ""
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();

            resultDocument.AssertErrorMessage("postcode", string.Format(ValidationMessages.RequiredEnter, "Postcode"));
            resultDocument.AssertErrorMessage("address", string.Format(ValidationMessages.RequiredEnter, "Address"));
            resultDocument.AssertErrorMessage("date-from", string.Format(ValidationMessages.RequiredEnter, "From"));
            resultDocument.AssertErrorMessage("date-to", string.Format(ValidationMessages.RequiredEnter, "To"));
        }

        [Fact]
        public async Task PostEditWithDateFromBeforeDateTo_ReturnsErrors()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_ADDRESSES;
            var editUrl = GetCurrentPathForId(notificationId) + ADDRESS_ID;
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
                ["Address.Address"] = "New address",
                ["Address.Postcode"] = "M4 4BF"
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();
            resultDocument.AssertErrorMessage("date-to", "To must be later than date from");
        }

        [Fact]
        public async Task GetEditForMissingAddress_ReturnsNotFound()
        {
            // Arrange
            const int notificationId = Utilities.DRAFT_ID;
            var editUrl = GetCurrentPathForId(notificationId) + ADDRESS_ID;

            // Act
            var editPage = await Client.GetAsync(editUrl);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, editPage.StatusCode);
        }

        [Fact]
        public async Task PostDelete_ReturnsSuccessAndRemovesResult()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_ADDRESSES;
            var editUrl = GetCurrentPathForId(notificationId) + ADDRESS_TO_DELETE_ID;
            var editDocument = await GetDocumentForUrl(editUrl);

            // Act
            var formData = new Dictionary<string, string> { };
            var result = await SendPostFormWithData(editDocument, formData, editUrl, "Delete");

            // Assert
            var socialContextAddressesPage = await AssertAndFollowRedirect(result, GetPathForId(NotificationSubPaths.EditSocialContextAddresses, notificationId));
            // Follow the redirect to see results table
            var resultDocument = await GetDocumentAsync(socialContextAddressesPage);
            Assert.Null(resultDocument.GetElementById($"social-context-address-{ADDRESS_TO_DELETE_ID}"));
        }

        [Fact]
        public async Task ValidateSocialContextDates_ReturnsErrorIfDateToBeforeDateFrom()
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
            var url = GetCurrentPathForId(0) + ADDRESS_ID;
            var response = await Client.GetAsync($"{url}/ValidateSocialContextDates?{string.Join("&", keyValuePairs)}");

            // Assert check just response.Content
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("To must be later than date from", result);
        }
    }
}
