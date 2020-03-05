using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Pages;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    // TODO: Complete tests for this page
    public class OverviewPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.Overview;

        public OverviewPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                { 
                    NotificationId = Utilities.NOTIFICATION_DATE_TODAY,
                    NotificationStatus = NotificationStatus.Notified,
                    NotificationDate = DateTime.Now
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_DATE_OVER_YEAR_AGO,
                    NotificationStatus = NotificationStatus.Notified,
                    NotificationDate = new DateTime(2015, 1, 1)
                },
                new Notification
                {
                    NotificationId = Utilities.LINKED_NOTIFICATION_ABINGDON_TB_SERVICE,
                    NotificationStatus = NotificationStatus.Notified,
                    GroupId = Utilities.OVERVIEW_NOTIFICATION_GROUP_ID,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.LINK_NOTIFICATION_ROYAL_FREE_LONDON_TB_SERVICE,
                    NotificationStatus = NotificationStatus.Notified,
                    GroupId = Utilities.OVERVIEW_NOTIFICATION_GROUP_ID,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID
                    }
                }
            };
        }

        public static IEnumerable<object[]> OverviewRoutes()
        {
            yield return new object[] { Utilities.DRAFT_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.NOTIFIED_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.DENOTIFIED_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.NEW_ID, HttpStatusCode.NotFound };
        }

        [Theory, MemberData(nameof(OverviewRoutes))]
        public async Task GetOverviewPage_ReturnsCorrectStatusCode_DependentOnId(int id, HttpStatusCode code)
        {
            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(id));

            // Assert
            Assert.Equal(code, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.Contains(GetPathForId(NotificationSubPaths.EditPatientDetails, id), GetRedirectLocation(response));
            }
        }

        [Fact]
        public async Task Get_ReturnsOverviewPage_ForUserWithPermission()
        {
            // Arrange
            using (var client = Factory.WithUser<NhsUserForAbingdonAndPermitted>()
                                        .WithNotificationAndTbServiceConnected(Utilities.NOTIFIED_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {

                //Act
                var response = await client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var document = await GetDocumentAsync(response);
                Assert.True(document.GetElementsByClassName("notification-overview-type-and-edit-container").Length > 0);
            }
        }

        [Fact]
        public async Task Get_ShowsWarning_ForUserWithoutPermission()
        {
            // Arrange
            using (var client = Factory.WithUser<NhsUserForAbingdonAndPermitted>()
                                        .WithNotificationAndTbServiceConnected(Utilities.NOTIFIED_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {

                //Act
                var response = await client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var document = await GetDocumentAsync(response);
                Assert.Contains(Messages.UnauthorizedWarning, document.GetElementById("unauthorized-warning").TextContent);
            }
        }
        
        [Fact]
        public async Task OverviewPageReturnsEditVersion_ForUserWithEditPermission()
        {
            // Arrange
            // Act
            var url = GetPathForId(NotificationSubPaths.Overview, Utilities.DRAFT_ID);
            var document = await GetDocumentForUrlAsync(url);
            
            // Assert
            Assert.NotNull(document.QuerySelectorAll("#patient-details-overview-header"));
            Assert.Null(document.QuerySelector("#patient-details-edit-link"));
        }
        
        [Fact]
        public async Task Get_ReturnsOverviewPageReadOnlyVersion_ForUserWithReadOnlyPermission()
        {
            // Arrange
            // NhsUserForAbingdonAndPermitted has been set up to have access to Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
            // which belong to the same Notification group as LINK_NOTIFICATION_ROYAL_FREE_LONDON_TB_SERVICE
            using (var client = Factory.WithUser<NhsUserForAbingdonAndPermitted>()
                .CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(Utilities.LINK_NOTIFICATION_ROYAL_FREE_LONDON_TB_SERVICE);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.NotNull(document.QuerySelectorAll("#patient-details-overview-header"));
                Assert.Null(document.QuerySelector("#patient-details-edit-link"));
            }
        }
        
        [Fact]
        public async Task OverviewPageReturnsReadOnlyVersion_ForPheUserWithMatchingPostcodePermission()
        {
            // Arrange
            using (var client = Factory.WithUser<PheUserWithPermittedPhecCode>()
                .WithNotificationAndPostcodeConnected(Utilities.NOTIFIED_ID, Utilities.PERMITTED_POSTCODE)
                .CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.NotNull(document.QuerySelectorAll("#patient-details-overview-header"));
                Assert.Null(document.QuerySelector("patient-details-edit-link"));
            }
        }
        

        [Fact]
        public async Task DismissAlert_CorrectlyDismissesAlertAndReturnsOverviewPage()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var document = await GetDocumentForUrlAsync(url);
            const string dismissPageRoute = "/Alerts/20001/Dismiss?Page=Overview";
            Assert.NotNull(document.QuerySelector("#alert-20001"));

            // Act
            var result = await Client.SendPostFormWithData(document, null, dismissPageRoute);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetRedirectLocation(result), url);
            var reloadedDocument = await GetDocumentForUrlAsync(GetRedirectLocation(result));
            Assert.Null(reloadedDocument.QuerySelector("#alert-20001"));
        }

        [Fact]
        public async Task OverviewPageHidesCreateNewNotificationForThisPatientButton_IfNotificationNotOverOneYearOld()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFICATION_DATE_TODAY);
            var document = await GetDocumentForUrlAsync(url);
            
            // Assert
            Assert.Null(document.QuerySelector("#new-linked-notification-button"));
        }

        [Fact]
        public async Task OverviewPageShowsCreateNewNotificationForThisPatientButton_IfNotificationOverOneYearOld()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFICATION_DATE_OVER_YEAR_AGO);
            var document = await GetDocumentForUrlAsync(url);
            
            // Assert
            Assert.NotNull(document.QuerySelector("#new-linked-notification-button"));
        }
        
        [Fact]
        public async Task OverviewPageShowsPrintOverviewButton_IfNotificationIsNotified()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var document = await GetDocumentForUrlAsync(url);
            
            Assert.NotNull(document.QuerySelector("#print-notification-overview-button"));
        }
        
        [Fact]
        public async Task OverviewPageShowsDenotificationDetails_ForDenotifiedRecord()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DENOTIFIED_ID);
            var document = await GetDocumentForUrlAsync(url);
            Assert.Contains("Other - a great reason", document.QuerySelector("#overview-denotification-reason").TextContent);
        }
        
        [Fact]
        public async Task OverviewPageDoesNotShowDenotificationDetails_ForNotifiedRecord()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var document = await GetDocumentForUrlAsync(url);
            Assert.Null(document.QuerySelector("#overview-denotification-reason"));
        }

        [Fact]
        public async Task OverviewPageContainsAnchorId_ForNotificationSubPath()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            
            // Act
            var document = await GetDocumentForUrlAsync(url);
            
            // Assert
            new List<string>
            {
                NotificationSubPaths.EditPatientDetails,
                NotificationSubPaths.EditHospitalDetails,
                NotificationSubPaths.EditClinicalDetails,
                NotificationSubPaths.EditContactTracing,
                NotificationSubPaths.EditTestResults,
                NotificationSubPaths.EditSocialRiskFactors,
                NotificationSubPaths.EditTravel,
                NotificationSubPaths.EditComorbidities,
                NotificationSubPaths.EditSocialContextAddresses,
                NotificationSubPaths.EditSocialContextVenues,
                NotificationSubPaths.EditPreviousHistory,
                NotificationSubPaths.EditMDRDetails,
                NotificationSubPaths.EditMBovisExposureToKnownCases,
                NotificationSubPaths.EditMBovisUnpasteurisedMilkConsumptions,
                NotificationSubPaths.EditMBovisOccupationExposures,
                NotificationSubPaths.EditMBovisAnimalExposures
            }.ForEach(subPath =>
            {
                var expectedAnchorString = OverviewSubPathToAnchorMap.GetOverviewAnchorId(subPath);
                Assert.NotNull(document.QuerySelector($"#{expectedAnchorString}"));
            });
        }
    }
}
