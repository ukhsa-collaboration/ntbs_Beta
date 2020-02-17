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
    public class MBovisExposureToKnownCasesPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditMBovisExposureToKnownCases;

        public MBovisExposureToKnownCasesPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }
        
        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_AND_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile {Species = "M. bovis"},
                    MBovisDetails = new MBovisDetails
                    {
                        HasExposureToKnownCases = true,
                        MBovisExposureToKnownCases = new List<MBovisExposureToKnownCase>
                        {
                            new MBovisExposureToKnownCase
                            {
                                ExposureSetting = ExposureSetting.Household,
                                YearOfExposure = 2010,
                                ExposureNotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NO_ENTITIES
                            }
                        }
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile {Species = "M. bovis"}
                }
            };
        }

        [Fact]
        public async Task IfNotificationHasKnownCases_DisplaysTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_AND_ENTITIES;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(document.QuerySelector("div[id='mbovis-exposure-to-known-cases-table']"));
        }

        [Fact]
        public async Task IfNotificationDoesNotHaveKnownCases_DoesNotDisplayTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NO_ENTITIES;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(document.QuerySelector("div[id='mbovis-exposure-to-known-cases-table']"));
        }
        
        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_AND_ENTITIES;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["MBovisDetails.HasExposureToKnownCases"] = "false"
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
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_AND_ENTITIES;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrl(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }

        [Fact]
        public async Task AddPage_WhenModelInvalid_ShowsExpectedValidationMessages()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_AND_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisExposureToKnownCase, id);
            var document = await GetDocumentForUrl(url);
            
            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisExposureToKnownCase.YearOfExposure"] = "10",
                ["MBovisExposureToKnownCase.ExposureSetting"] = "",
                ["MBovisExposureToKnownCase.ExposureNotificationId"] = "0",
                ["MBovisExposureToKnownCase.OtherDetails"] = "£$%^"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();

            resultDocument.AssertErrorSummaryMessage(
                "MBovisExposureToKnownCase-YearOfExposure", 
                "year-of-exposure", 
                ValidationMessages.InvalidYear("Year of exposure"));
            resultDocument.AssertErrorSummaryMessage(
                "MBovisExposureToKnownCase-ExposureSetting", 
                "exposure-setting", 
                string.Format(ValidationMessages.RequiredSelect, "Exposure setting"));
            resultDocument.AssertErrorSummaryMessage(
                "MBovisExposureToKnownCase-ExposureNotificationId", 
                "related-notification", 
                ValidationMessages.RelatedNotificationIdInvalid);
            resultDocument.AssertErrorSummaryMessage(
                "MBovisExposureToKnownCase-OtherDetails", 
                "other-details", 
                string.Format(ValidationMessages.StringWithNumbersAndForwardSlashFormat, "Other details"));
        }
        
        [Fact]
        public async Task AddPage_WhenModelValid_RedirectsToCollectionView()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_AND_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisExposureToKnownCase, id);
            var document = await GetDocumentForUrl(url);
            
            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisExposureToKnownCase.YearOfExposure"] = "2000",
                ["MBovisExposureToKnownCase.ExposureSetting"] = ((int)ExposureSetting.Household).ToString(),
                ["MBovisExposureToKnownCase.ExposureNotificationId"] = $"{Utilities.NOTIFIED_ID}"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertRedirectTo(RouteHelper.GetNotificationPath(id,
                NotificationSubPaths.EditMBovisExposureToKnownCases));
        }
    }
}
