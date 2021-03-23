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
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID),
                        CaseManagerUsername = Utilities.CASEMANAGER_ABINGDON_EMAIL
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_TRANSFER_REQUEST_TO_ACCEPT,
                    NotificationStatus = NotificationStatus.Notified,
                    HospitalDetails = new HospitalDetails()
                    {
                        TBServiceCode = Utilities.TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID,
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
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "false",
                ["DeclineTransferReason"] = "|||",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

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
            var initialDocument = await GetDocumentForUrlAsync(url);

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, null, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("action", "Please accept or decline the transfer");
        }

        [Fact]
        public async Task AcceptTransferAlert_SuccessfullyChangesTbServiceCaseManagerAndHospitalOfNotificationAndDismissesAlert()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_WITH_NOTIFICATION_DATE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            Assert.Equal("  ", initialDocument.QuerySelector("#banner-tb-service").TextContent);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "true",
                ["TargetCaseManagerUsername"] = Utilities.CASEMANAGER_ABINGDON_EMAIL,
                ["TargetHospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            Assert.NotNull(resultDocument.QuerySelector("#return-to-notification"));
            var overviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
            var overviewPage = await GetDocumentForUrlAsync(overviewUrl);
            Assert.Contains("Abingdon Community Hospital", overviewPage.QuerySelector("#banner-tb-service").TextContent);
            Assert.Contains("TestCase TestManager", overviewPage.QuerySelector("#banner-case-manager").TextContent);
            Assert.Contains("ABINGDON COMMUNITY HOSPITAL", overviewPage.QuerySelector("#overview-hospital-name").TextContent);
            Assert.Null(overviewPage.QuerySelector("#alert-20003"));
        }

        [Fact]
        public async Task AcceptTransferAlert_CreatesTransferInAndOutTreatmentEventsForNotification()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_WITH_TRANSFER_REQUEST_TO_ACCEPT;
            var treatmentEventsUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditTreatmentEvents);
            var initialTreatmentEventsPage = await GetDocumentForUrlAsync(treatmentEventsUrl);
            Assert.Null(initialTreatmentEventsPage.QuerySelector("#treatment-events"));

            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "true",
                ["TargetCaseManagerUsername"] = Utilities.CASEMANAGER_ABINGDON_EMAIL,
                ["TargetHospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID
            };

            // Act
            await Client.SendPostFormWithData(initialDocument, formData, url);

            var reloadedTreatmentEventsPage = await GetDocumentForUrlAsync(treatmentEventsUrl);

            // Assert
            var reloadedTreatmentEventsTable = reloadedTreatmentEventsPage.QuerySelector("#treatment-events");
            Assert.Contains("Transfer in", reloadedTreatmentEventsTable.InnerHtml);
            Assert.Contains("Transfer out", reloadedTreatmentEventsTable.InnerHtml);
        }

        [Fact]
        public async Task DeclineTransferAlert_CreatesNewTransferRejectionAlert()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_WITH_TRANSFER_REQUEST_TO_REJECT;

            var overviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
            var overviewPage = await GetDocumentForUrlAsync(overviewUrl);
            Assert.NotNull(overviewPage.QuerySelector("#alert-20004"));

            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "false",
                ["DeclineTransferReason"] = "nah"
            };

            // Act
            await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var reloadedOverviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
            var reloadedOverviewPage = await GetDocumentForUrlAsync(reloadedOverviewUrl);
            var alertsContainer = reloadedOverviewPage.QuerySelector(".overview-alerts-container");
            Assert.Null(alertsContainer.QuerySelector("#alert-20004"));
            Assert.Contains("Transfer request rejected", alertsContainer.InnerHtml);
        }
    }
}
