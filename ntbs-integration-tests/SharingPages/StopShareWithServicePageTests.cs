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
    public class StopShareWithServicePageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.StopShareWithService;
        public StopShareWithServicePageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new()
                {
                    NotificationId = Utilities.NOTIFICATION_SHARED_TO_GATESHEAD,
                    NotificationStatus = NotificationStatus.Notified,
                    NotificationDate = new DateTime(2022, 02, 06),
                    // Requires a notification site to pass full validation
                    NotificationSites =
                        new List<NotificationSite>
                        {
                            new() {NotificationId = Utilities.NOTIFICATION_SHARED_TO_GATESHEAD, SiteId = (int)SiteId.PULMONARY}
                        },
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID),
                        NotificationId = Utilities.NOTIFICATION_SHARED_TO_GATESHEAD,
                        SecondaryTBServiceCode = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                        ReasonForTBServiceShare = "Gateshead have all the answers."
                    },
                }
            };
        }
        
        [Fact]
        public async Task Post_StopShareWithService_RedirectsToOverviewPage()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_SHARED_TO_GATESHEAD;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>();

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo($"/Notifications/{id}");
        }

        public static TheoryData<TestUser> unpermittedUsers = new() {
            { TestUser.GatesheadCaseManager }, 
            { TestUser.ServiceUserWithNoTbServices }
        };
            
        [Theory, MemberData(nameof(unpermittedUsers))]
        public async Task OnGet_UserWhoIsNotPermittedToEditNotification_RedirectsToOverviewPage(TestUser user)
        {
            const int id = Utilities.NOTIFICATION_SHARED_TO_GATESHEAD;
            using (var client = Factory.WithUserAuth(user)
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
