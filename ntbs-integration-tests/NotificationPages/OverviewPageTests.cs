using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models;
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

        private static readonly string PatientDetailsOverviewSectionId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPaths.EditPatientDetails);
        private static readonly string ClinicalDetailsOverviewSectionId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPaths.EditClinicalDetails);
        private static readonly string HospitalDetailsOverviewSectionId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPaths.EditHospitalDetails);
        private static readonly string TreatmentEventsOverviewSectionId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPaths.EditTreatmentEvents);
        private static readonly string ContactTracingOverviewSectionId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPaths.EditContactTracing);

        public OverviewPageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory) { }

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
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                        CaseManagerId = Utilities.CASEMANAGER_ABINGDON_ID
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
                    },
                    PatientDetails = new PatientDetails
                    {
                        GivenName = "Anna",
                        FamilyName = "Delvey",
                        CountryId = Countries.UnknownId
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.CLOSED_NOTIFICATION_IN_ABINGDON,
                    NotificationStatus = NotificationStatus.Closed,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_PREVIOUS_TB_SERVICE_OF_ABINGDON,
                    NotificationStatus = NotificationStatus.Notified,
                    PreviousTbServices = new List<PreviousTbService>
                    {
                        new PreviousTbService
                        {
                            NotificationId = Utilities.NOTIFICATION_WITH_PREVIOUS_TB_SERVICE_OF_ABINGDON,
                            TbServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
                        }
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_OVERVIEW,
                    NotificationStatus = NotificationStatus.Notified,
                    SocialContextAddresses = new List<SocialContextAddress>
                    {
                        new SocialContextAddress
                        {
                            Details = "{{abc}}"
                        },
                    },
                    SocialContextVenues = new List<SocialContextVenue>
                    {
                        new SocialContextVenue
                        {
                            Details = "{{def}}",
                            Name = "{{ghi}}"
                        }
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
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
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
        public async Task Get_RendersCorrectNavigationLinks()
        {
            // Arrange
            using (var client = Factory
                .WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                .WithNotificationAndTbServiceConnected(Utilities.LINKED_NOTIFICATION_ABINGDON_TB_SERVICE, Utilities.PERMITTED_SERVICE_CODE)
                .CreateClientWithoutRedirects())
            {
                // Act
                var changesPath = GetPathForId(NotificationSubPaths.Overview, Utilities.LINKED_NOTIFICATION_ABINGDON_TB_SERVICE);
                var response = await client.GetAsync(changesPath);
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var navLinks = document.GetElementsByClassName("app-subnav__link");
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Notification details")));
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Linked notifications (1)")));
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Notification changes")));
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Case manager details")));
            }
        }

        [Fact]
        public async Task Get_ShowsWarning_ForUserWithoutPermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                                        .WithNotificationAndTbServiceConnected(Utilities.NOTIFIED_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {

                //Act
                var response = await client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var document = await GetDocumentAsync(response);
                Assert.Contains(Messages.UnauthorizedWarning, document.Body.InnerHtml);
            }
        }

        [Fact]
        public async Task Get_ShowsShareMessageWithLink_ForSharedRecord()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                       .CreateClientWithoutRedirects())
            {
                //Act
                var response = await client.GetAsync(GetCurrentPathForId(Utilities.NOTIFICATION_SHARED_TO_GATESHEAD));

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var document = await GetDocumentAsync(response);
                Assert.Contains("This notification is shared with", document.Body.TextContent);
                var link = (IHtmlAnchorElement)document.GetElementById("shared-service-link");
                Assert.Contains(Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_NAME, link.TextContent);
                Assert.Contains(Utilities.NORTH_EAST_PHEC_CODE_GATESHEAD, link.Href);
                Assert.Contains(Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID, link.Href);
            }
        }

        [Fact]
        public async Task OverviewPageReturnsEditVersion_ForUserWithEditPermission()
        {
            // Arrange
            var url = GetPathForId(NotificationSubPaths.Overview, Utilities.NOTIFIED_ID);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            AssertEditOverview(document);
        }

        [Fact]
        public async Task Get_ReturnsOverviewPageReadOnlyVersion_ForUserWithReadOnlyPermissionFromLinkedNotification()
        {
            // Arrange
            // ServiceUserForAbingdonAndPermitted has been set up to have access to Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
            // which belong to the same Notification group as LINK_NOTIFICATION_ROYAL_FREE_LONDON_TB_SERVICE
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                .CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(Utilities.LINK_NOTIFICATION_ROYAL_FREE_LONDON_TB_SERVICE);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                AssertReadOnlyOverview(document);
            }
        }

        [Fact]
        public async Task OverviewPageReturnsReadOnlyVersion_ForRegionalUserWithMatchingPostcodePermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.RegionalUserWithPermittedPhecCode)
                .WithNotificationAndPostcodeConnected(Utilities.NOTIFICATION_WITH_TRANSFER, Utilities.PERMITTED_POSTCODE)
                .CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(Utilities.NOTIFICATION_WITH_TRANSFER);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                AssertReadOnlyOverview(document);
            }
        }

        [Fact]
        public async Task OverviewPageForSharedRecordReturnsReadOnlyVersionExceptContactTracing_ForUserInSharedTbService()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.GatesheadCaseManager).CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(Utilities.NOTIFICATION_SHARED_TO_GATESHEAD);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                AssertReadOnlyOverview(document);
                Assert.NotNull(document.QuerySelector($"#{ContactTracingOverviewSectionId}-edit-link"));
            }
        }

        [Fact]
        public async Task OverviewPageReturnsReadOnlyVersion_ForUserWhoHadEditPermissionsBeforeATransfer()
        {
            // Arrange
            // ServiceUserForAbingdonAndPermitted has been set up to have access to Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
            // which belong to the notification has previously been assigned to
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                .CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(Utilities.NOTIFICATION_WITH_PREVIOUS_TB_SERVICE_OF_ABINGDON);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                AssertReadOnlyOverview(document);
            }
        }

        [Fact]
        public async Task GetOnClosedNotification_ReturnsOverviewPageReadOnlyVersion_ForUserWithEditPermission()
        {
            // Arrange
            // ServiceUserForAbingdonAndPermitted has been set up to have access to Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
            // which belong to the same Notification group as LINK_NOTIFICATION_ROYAL_FREE_LONDON_TB_SERVICE
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                .CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(Utilities.CLOSED_NOTIFICATION_IN_ABINGDON);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                AssertReadOnlyOverview(document);
                Assert.Null(document.QuerySelector("#navigation-side-menu"));
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

        public static TheoryData<TestUser, bool, string, int> shareButtonUsers = new()
        {
            { TestUser.AbingdonCaseManager, true, "share-button", Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE },
            { TestUser.GatesheadCaseManager, false, "share-button", Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE },
            { TestUser.RegionalUserWithPermittedPhecCode, true, "share-button", Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE },
            { TestUser.ServiceUserWithNoTbServices, false, "share-button", Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE },
            { TestUser.AbingdonCaseManager, true, "stop-share-button", Utilities.NOTIFICATION_SHARED_TO_GATESHEAD },
            { TestUser.GatesheadCaseManager, false, "stop-share-button", Utilities.NOTIFICATION_SHARED_TO_GATESHEAD },
            { TestUser.RegionalUserWithPermittedPhecCode, true, "stop-share-button", Utilities.NOTIFICATION_SHARED_TO_GATESHEAD },
            { TestUser.ServiceUserWithNoTbServices, false, "stop-share-button", Utilities.NOTIFICATION_SHARED_TO_GATESHEAD }
        };

        [Theory, MemberData(nameof(shareButtonUsers))]
        public async Task OverviewPageShowsCorrectShareButton_IfUserHasEditRights(TestUser user, bool seeShareButton, string buttonId, int notificationId)
        {
            using (var client = Factory.WithUserAuth(user).CreateClientWithoutRedirects())
            {
                // Arrange
                var url = GetCurrentPathForId(notificationId);
                var response = await client.GetAsync(url);
                var document = await GetDocumentAsync(response);

                // Assert
                if (seeShareButton)
                {
                    Assert.NotNull(document.GetElementById(buttonId));
                }
                else
                {
                    Assert.Null(document.GetElementById(buttonId));
                }
            }
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
            Assert.Contains("Other - a great reason", document.QuerySelector("#overview-hospital-details-denotification-reason").TextContent);
        }

        [Fact]
        public async Task OverviewPageShowsDenotificationEvent_ForDenotifiedRecord()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DENOTIFIED_ID);
            var document = await GetDocumentForUrlAsync(url);
            var eventText = document.QuerySelector("#overview-episodes").TextContent;
            Assert.Contains("25 Dec 2020", eventText);
            Assert.Contains("Denotification", eventText);
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
                Assert.NotNull(document.QuerySelector($"#{expectedAnchorString}-title"));
            });
        }

        [Fact]
        public async Task OverviewPageRemovesCurlyBracketsFromComment()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_OVERVIEW);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Asset
            var detailsContainer = document.GetElementById("maincontent").TextContent;

            Assert.DoesNotContain("{", detailsContainer);
            Assert.DoesNotContain("}", detailsContainer);
            Assert.Contains("abc", detailsContainer);
            Assert.Contains("def", detailsContainer);
            Assert.Contains("ghi", detailsContainer);
        }

        private void AssertEditOverview(IHtmlDocument document)
        {
            AssertSectionTitlesInDocument(document);
            Assert.NotNull(document.QuerySelector($"#{PatientDetailsOverviewSectionId}-edit-link"));
            Assert.NotNull(document.QuerySelector($"#{ClinicalDetailsOverviewSectionId}-edit-link"));
            Assert.NotNull(document.QuerySelector($"#{HospitalDetailsOverviewSectionId}-edit-link"));
            Assert.NotNull(document.QuerySelector($"#{TreatmentEventsOverviewSectionId}-edit-link"));
        }

        private void AssertReadOnlyOverview(IHtmlDocument document)
        {
            AssertSectionTitlesInDocument(document);
            Assert.Null(document.QuerySelector($"#{PatientDetailsOverviewSectionId}-edit-link"));
            Assert.Null(document.QuerySelector($"#{ClinicalDetailsOverviewSectionId}-edit-link"));
            Assert.Null(document.QuerySelector($"#{HospitalDetailsOverviewSectionId}-edit-link"));
            Assert.Null(document.QuerySelector($"#{TreatmentEventsOverviewSectionId}-edit-link"));
        }

        private void AssertSectionTitlesInDocument(IHtmlDocument document)
        {
            Assert.NotNull(document.QuerySelector($"#{PatientDetailsOverviewSectionId}-title"));
            Assert.NotNull(document.QuerySelector($"#{ClinicalDetailsOverviewSectionId}-title"));
            Assert.NotNull(document.QuerySelector($"#{HospitalDetailsOverviewSectionId}-title"));
            Assert.NotNull(document.QuerySelector($"#{TreatmentEventsOverviewSectionId}-title"));
        }
    }
}
