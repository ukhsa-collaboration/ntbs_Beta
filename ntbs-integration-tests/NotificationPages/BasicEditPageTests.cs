using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace ntbs_integration_tests.NotificationPages
{
    public class BasicEditPageTests : TestRunnerNotificationBase
    {
        private readonly ITestOutputHelper _output;

        public BasicEditPageTests(NtbsWebApplicationFactory<EntryPoint> factory, ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Theory, MemberData(nameof(EditPageSubPaths))]
        public async Task Get_ReturnsNotFound_ForNewId(string subPath)
        {
            // Act
            var response = await Client.GetAsync(GetPathForId(subPath, Utilities.NEW_ID));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory, MemberData(nameof(OkEditPathToIdCombinations))]
        public async Task Get_ReturnsOk_ForExistingIds(string subPath, int id)
        {
            //Act
            var response = await Client.GetAsync(GetPathForId(subPath, id));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Get_ReturnsOk_ForServiceUserWithPermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
               {
                   // Act
                   var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                   _output.WriteLine("Testing subpath {0}", subPath);
                   // Assert
                   Assert.Equal(HttpStatusCode.OK, response.StatusCode);
               });
            }
        }

        [Fact]
        public void Get_ReturnsRedirect_ForServiceUserWithoutPermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
               {
                   // Act
                   var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                   _output.WriteLine("Testing subpath {0}", subPath);
                   // Assert
                   Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
               });
            }
        }

        [Fact]
        public void Get_ReturnsOk_ForRegionalUserWithMatchingServicePermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.RegionalUserWithPermittedPhecCode)
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
                {
                    // Act
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                    _output.WriteLine("Testing subpath {0}", subPath);
                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                });
            }
        }

        [Fact]
        public void Get_ReturnsRedirectToOverview_ForRegionalUserWithMatchingPostcodePermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.RegionalUserWithPermittedPhecCode)
                                        .WithNotificationAndPostcodeConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_POSTCODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
               {
                   // ActAccessToDisposedClosure
                   var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                   _output.WriteLine("Testing subpath {0}", subPath);

                   // Assert
                   response.AssertRedirectTo($"/Notifications/{Utilities.DRAFT_ID}");
               });
            }
        }

        [Fact]
        public void Get_ReturnsRedirectToOverview_ForDraftForReadOnlyUser()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.ReadOnlyUser)
                .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
                {
                    // ActAccessToDisposedClosure
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                    _output.WriteLine("Testing subpath {0}", subPath);

                    // Assert
                    response.AssertRedirectTo($"/Notifications/{Utilities.DRAFT_ID}");
                });
            }
        }

        [Fact]
        public void Get_ReturnsRedirectToOverview_ForNotifiedForReadOnlyUser()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.ReadOnlyUser)
                .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
                {
                    // ActAccessToDisposedClosure
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.NOTIFIED_ID)).Result;

                    _output.WriteLine("Testing subpath {0}", subPath);
                   
                    // Assert
                    response.AssertRedirectTo($"/Notifications/{Utilities.NOTIFIED_ID}");
                });
            }
        }

        [Fact]
        public void Get_ReturnsRedirect_ForRegionalUserWithoutMatchingServicePermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.RegionalUserWithPermittedPhecCode)
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
               {
                   // Act
                   var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                   _output.WriteLine("Testing subpath {0}", subPath);
                   // Assert
                   Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
               });
            }
        }

        [Fact]
        public void Get_ReturnsRedirect_ForRegionalUserWithoutMatchingPostcodePermission()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.RegionalUserWithPermittedPhecCode)
                                        .WithNotificationAndPostcodeConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_POSTCODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach(subPath =>
               {
                   // Act
                   var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                   _output.WriteLine("Testing subpath {0}", subPath);
                   // Assert
                   Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
               });
            }
        }

        private static readonly List<int> OkNotificationIds = new List<int>()
        {
            Utilities.DRAFT_ID,
            Utilities.NOTIFIED_ID,
            Utilities.DENOTIFIED_ID
        };

        public static IEnumerable<object[]> EditPageSubPaths()
        {
            return EditSubPaths.Select(path => new object[] { path });
        }

        public static IEnumerable<object[]> OkEditPathToIdCombinations()
        {
            return EditSubPaths.SelectMany(path =>
                OkNotificationIds.Select(id => new object[] { path, id })
            );
        }

        private static readonly List<string> EditSubPaths = new List<string>()
        {
            NotificationSubPaths.EditPatientDetails,
            NotificationSubPaths.EditHospitalDetails,
            NotificationSubPaths.EditClinicalDetails,
            NotificationSubPaths.EditContactTracing,
            NotificationSubPaths.EditSocialRiskFactors,
            NotificationSubPaths.EditTravel,
            NotificationSubPaths.EditComorbidities,
            NotificationSubPaths.EditPreviousHistory,
            NotificationSubPaths.EditMDRDetails,
            NotificationSubPaths.EditSocialContextVenues,
            NotificationSubPaths.EditSocialContextAddresses,
            NotificationSubPaths.EditMBovisExposureToKnownCases,
            NotificationSubPaths.EditMBovisUnpasteurisedMilkConsumptions,
            NotificationSubPaths.EditMBovisOccupationExposures,
            NotificationSubPaths.EditMBovisAnimalExposures
        };
    }
}



