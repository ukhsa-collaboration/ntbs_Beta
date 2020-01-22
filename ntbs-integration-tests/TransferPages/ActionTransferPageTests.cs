using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_integration_tests.TransferPage
{
    public class ActionTransferPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.ActionTransferRequest;
        public ActionTransferPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFIED_ID_WITH_TRANSFER_REQUEST_TO_REJECT,
                    NotificationStatus = NotificationStatus.Notified,
                    // Requires a notification site to pass full validation
                    NotificationSites = new List<NotificationSite>
                    {
                        new NotificationSite { NotificationId = Utilities.NOTIFIED_ID_WITH_TRANSFER_REQUEST_TO_REJECT, SiteId = (int)SiteId.PULMONARY }
                    },
                    Episode = new Episode
                    {
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID),
                        CaseManagerUsername = Utilities.CASEMANAGER_ABINGDON_EMAIL
                    }
                }
            };
        }

        [Fact]
        public async Task DeclineTransferAlert_ReturnsPageWithModelErrors_IfReasonNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "false",
                ["DeclineTransferReason"] = "|||",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("comment", "Explanatory comment can only contain letters, numbers and the symbols ' - . , /");
        }

        [Fact]
        public async Task ActionTransferAlertPage_ReturnsPageWithModelErrors_IfNoChoiceMade()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            // Act
            var result = await SendPostFormWithData(initialDocument, null, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("action", "Please accept or decline the transfer");
        }

        [Fact]
        public async Task AcceptTransferAlert_SuccessfullyChangesTbServiceOfNotificationAndDismissesAlert()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_WITH_NOTIFICATION_DATE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            Assert.Equal("  ", initialDocument.QuerySelector("#banner-tb-service").TextContent);
            Assert.Equal("  ", initialDocument.QuerySelector("#banner-case-manager").TextContent);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "true"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            Assert.NotNull(resultDocument.QuerySelector("#return-to-notification"));
            var overviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
            var overviewPage = await GetDocumentForUrl(overviewUrl);
            Assert.Contains("Abingdon Community Hospital", overviewPage.QuerySelector("#banner-tb-service").TextContent);
            Assert.Contains("TestCase TestManager", overviewPage.QuerySelector("#banner-case-manager").TextContent);
            Assert.Null(overviewPage.QuerySelector("#alert-3"));
        }


        // This is untested as currently we are unable to add alerts to the in-memory database, we can only seed and remove them.
        // Some more investigation needs to be done if we want to implement this test.
        // in 
        //
        // [Fact]
        // public async Task DeclineTransferAlert_CreatesNewTransferRejectionAlert()
        // {
        //     // Arrange
        //     const int id = Utilities.NOTIFIED_ID_WITH_TRANSFER_REQUEST_TO_REJECT;
        //     var url = GetCurrentPathForId(id);
        //     var initialDocument = await GetDocumentForUrl(url);

        //     var formData = new Dictionary<string, string>
        //     {
        //         ["AcceptTransfer"] = "false",
        //         ["DeclineTransferReason"] = "nah",
        //     };

        //     // Act
        //     var result = await SendPostFormWithData(initialDocument, formData, url);

        //     // Assert
        //     var overviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
        //     var overviewPage = await GetDocumentForUrl(overviewUrl);
        //     Assert.NotNull(overviewPage.QuerySelector(".overview-alerts-container"));
        // }
    }
}