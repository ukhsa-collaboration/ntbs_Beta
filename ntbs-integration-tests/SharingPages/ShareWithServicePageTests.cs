using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_integration_tests.SharingPages
{
    public class ShareWithServicePageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.ShareWithService;
        public ShareWithServicePageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new()
                {
                    NotificationId = Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE,
                    NotificationStatus = NotificationStatus.Notified,
                    NotificationDate = new DateTime(2022, 01, 05),
                    // Requires a notification site to pass full validation
                    NotificationSites =
                        new List<NotificationSite>
                        {
                            new() {NotificationId = Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE, SiteId = (int)SiteId.PULMONARY}
                        },
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID),
                        NotificationId = Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE
                    },
                }
            };
        }

        [Fact]
        public async Task ShareWithService_ReturnsPageWithModelErrors_IfRequestNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["ServiceShareViewModel.SharingTBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["ServiceShareViewModel.ReasonForTBServiceShare"] = "£££"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("tb-service", "Notification cannot be shared with the notification's current TB service");
            resultDocument.AssertErrorMessage("reason", "Invalid character found in Reason for TB Service share");
        }
        
        [Fact]
        public async Task ShareWithService_RedirectsToOverviewPage_IfShareValid()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["ServiceShareViewModel.SharingTBServiceCode"] = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                ["ServiceShareViewModel.ReasonForTBServiceShare"] = "THey know the contact tracing numbers, patient lives there"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo($"/Notifications/{id}");
        }

        [Fact]
        public async Task OnGet_UserWhoIsNotPermittedToEditNotification_RedirectsToOverviewPage()
        {
            const int id = Utilities.NOTIFICATION_IN_ABINGDON_TO_SHARE;
            using (var client = Factory.WithUserAuth(TestUser.GatesheadCaseManager)
                       .CreateClientWithoutRedirects())
            {
                // Act
                var url = GetCurrentPathForId(id);
                var response = await client.GetAsync(url);

                // Assert
                response.AssertRedirectTo($"/Notifications/{id}");
            }
        }
        
    }
}
