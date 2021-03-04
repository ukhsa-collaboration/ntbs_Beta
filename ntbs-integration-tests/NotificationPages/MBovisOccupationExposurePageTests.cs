using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class MBovisOccupationExposurePageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditMBovisOccupationExposures;

        public MBovisOccupationExposurePageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile {Species = "M. bovis"},
                    MBovisDetails = new MBovisDetails
                    {
                        HasOccupationExposure = true,
                        MBovisOccupationExposures = new List<MBovisOccupationExposure>
                        {
                            new MBovisOccupationExposure
                            {
                                YearOfExposure = 2000,
                                OccupationDuration = 12,
                                OccupationSetting = OccupationSetting.Farm,
                                CountryId = 1
                            }
                        }
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile {Species = "M. bovis"}
                }
            };
        }

        [Fact]
        public async Task IfNotificationHasKnownCases_DisplaysTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_ENTITIES;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(document.QuerySelector("#mbovis-occupation-exposure-table"));
        }

        [Fact]
        public async Task IfNotificationDoesNotHaveKnownCases_DoesNotDisplayTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_NO_ENTITIES;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(document.QuerySelector("#mbovis-occupation-exposure-table"));
        }

        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_ENTITIES;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["MBovisDetails.HasOccupationExposure"] = "false"
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
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_ENTITIES;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }

        [Fact]
        public async Task AddPage_WhenModelInvalid_ShowsExpectedValidationMessages()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisOccupationExposure, id);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisOccupationExposure.YearOfExposure"] = "1100",
                ["MBovisOccupationExposure.CountryId"] = "",
                ["MBovisOccupationExposure.OccupationSetting"] = "",
                ["MBovisOccupationExposure.OccupationDuration"] = "1100",
                ["MBovisOccupationExposure.OtherDetails"] = "$£$£$£"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();

            resultDocument.AssertErrorSummaryMessage(
                "MBovisOccupationExposure-YearOfExposure",
                "year-of-exposure",
                "Year of exposure has an invalid year");
            resultDocument.AssertErrorSummaryMessage(
                "MBovisOccupationExposure-OccupationDuration",
                "duration",
                "The field Duration (years) must be between 1 and 99.");
            resultDocument.AssertErrorSummaryMessage(
                "MBovisOccupationExposure-OtherDetails",
                "other-details",
                "Invalid character found in Other details");
        }

        [Fact]
        public async Task AddPage_WhenModelValid_RedirectsToCollectionView()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_OCCUPATION_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisOccupationExposure, id);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisOccupationExposure.YearOfExposure"] = "2000",
                ["MBovisOccupationExposure.CountryId"] = "1",
                ["MBovisOccupationExposure.OccupationSetting"] = ((int)OccupationSetting.Farm).ToString(),
                ["MBovisOccupationExposure.OccupationDuration"] = "1"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertRedirectTo(
                RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditMBovisOccupationExposures));
        }
    }
}
