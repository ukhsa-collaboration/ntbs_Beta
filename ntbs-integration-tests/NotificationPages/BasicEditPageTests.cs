﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class BasicEditPageTests : TestRunnerNotificationBase
    {
        public BasicEditPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

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
        public async Task Get_ReturnsOk_ForNhsUserWithPermission()
        {
            // Arrange
            using (var client = Factory.WithUser<NhsUserForAbingdonAndPermitted>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach( subPath =>
                {
                    // Act
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                });
            }
        }

        [Fact]
        public async Task Get_ReturnsRedirect_ForNhsUserWithoutPermission()
        {
            // Arrange
            using (var client = Factory.WithUser<NhsUserForAbingdonAndPermitted>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach( subPath =>
                {
                    // Act
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                    // Assert
                    Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                });
            }
        }

        [Fact]
        public async Task Get_ReturnsOk_ForPheUserWithMatchingServicePermission()
        {
            // Arrange
            using (var client = Factory.WithUser<PheUserWithPermittedPhecCode>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach( subPath =>
                {
                    // Act
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                });
            }
        }

        [Fact]
        public async Task Get_ReturnsOk_ForPheUserWithMatchingPostcodePermission()
        {
            // Arrange
            using (var client = Factory.WithUser<PheUserWithPermittedPhecCode>()
                                        .WithNotificationAndPostcodeConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_POSTCODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach( subPath =>
                {
                    // Act
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                });
            }
        }

        [Fact]
        public async Task Get_ReturnsRedirect_ForPheUserWithoutMatchingServicePermission()
        {
            // Arrange
            using (var client = Factory.WithUser<PheUserWithPermittedPhecCode>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach( subPath =>
                {
                    // Act
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

                    // Assert
                    Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                });
            }
        }

        [Fact]
        public async Task Get_ReturnsRedirect_ForPheUserWithoutMatchingPostcodePermission()
        {
            // Arrange
            using (var client = Factory.WithUser<PheUserWithPermittedPhecCode>()
                                        .WithNotificationAndPostcodeConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_POSTCODE)
                                        .CreateClientWithoutRedirects())
            {
                EditSubPaths.ForEach( subPath =>
                {
                    // Act
                    var response = client.GetAsync(GetPathForId(subPath, Utilities.DRAFT_ID)).Result;

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



