using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Services;
using Xunit;

namespace ntbs_integration_tests.LabResultsPage
{
    public class LabResultsPageTests : TestRunnerBase
    {
        public LabResultsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task NhsUser_CanViewSpecimensAccordingToPermissions()
        {
            using (var client = Factory.WithMockUserService<TestNhsUserService>()
                .CreateClientWithoutRedirects())
            {
                // Arrange
                var expectedLabReferenceNumbers = new List<string>
                {
                    MockSpecimenService.MockUnmatchedSpecimenForTbService.ReferenceLaboratoryNumber
                };
                var notExpectedLabReferenceNumbers = new List<string>
                {
                    MockSpecimenService.MockUnmatchedSpecimenForPhec.ReferenceLaboratoryNumber
                };
                
                //Act
                var response = await client.GetAsync("/LabResults");
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                
                // var specimenDetailsSections = document.QuerySelectorAll(".specimen-details");
                // Assert.Equal(expectedLabReferenceNumbers.Count, specimenDetailsSections.Length);
                
                foreach (var expectedLabReferenceNumber in expectedLabReferenceNumbers)
                {
                    var header = document.QuerySelector($"#specimen-{expectedLabReferenceNumber}");
                    Assert.NotNull(header);
                }

                foreach (var notExpectedLabReferenceNumber in notExpectedLabReferenceNumbers)
                {
                    var header = document.QuerySelector($"#specimen-{notExpectedLabReferenceNumber}");
                    Assert.Null(header);
                }
            }
        }

        [Fact]
        public async Task NhsUser_ShowsNoSpecimensIfNoPermissionForTbServices()
        {
            using (var client = Factory.WithMockUserService<TestWithoutTbServicesNhsUserService>()
                .CreateClientWithoutRedirects())
            {
                //Act
                var response = await client.GetAsync("/LabResults");
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var specimenDetailsSections = document.QuerySelectorAll(".specimen-details");
                Assert.Equal(0, specimenDetailsSections.Length);
            }   
        }

        [Fact]
        public async Task PheUser_CanViewSpecimensAccordingToPermissions()
        {
            using (var client = Factory.WithMockUserService<TestPheUserService>()
                .CreateClientWithoutRedirects())
            {
                // Arrange
                var expectedLabReferenceNumbers = new List<string>
                {
                    MockSpecimenService.MockUnmatchedSpecimenForPhec.ReferenceLaboratoryNumber
                };
                var notExpectedLabReferenceNumbers = new List<string>
                {
                    MockSpecimenService.MockUnmatchedSpecimenForTbService.ReferenceLaboratoryNumber
                };
                
                //Act
                var response = await client.GetAsync("/LabResults");
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                
                var specimenDetailsSections = document.QuerySelectorAll(".specimen-details");
                Assert.Equal(expectedLabReferenceNumbers.Count, specimenDetailsSections.Length);
                
                foreach (var expectedLabReferenceNumber in expectedLabReferenceNumbers)
                {
                    var header = document.QuerySelector($"#specimen-{expectedLabReferenceNumber}");
                    Assert.NotNull(header);
                }

                foreach (var notExpectedLabReferenceNumber in notExpectedLabReferenceNumbers)
                {
                    var header = document.QuerySelector($"#specimen-{notExpectedLabReferenceNumber}");
                    Assert.Null(header);
                }
            }
        }

        [Fact]
        public async Task NationalTeam_CanViewSpecimensAccordingToPermissions()
        {
            using (var client = Factory.WithMockUserService<TestNationalTeamUserService>()
                .CreateClientWithoutRedirects())
            {
                // Arrange
                var expectedLabReferenceNumbers = new List<string>
                {
                    MockSpecimenService.MockUnmatchedSpecimenForTbService.ReferenceLaboratoryNumber,
                    MockSpecimenService.MockUnmatchedSpecimenForPhec.ReferenceLaboratoryNumber
                };

                //Act
                var response = await client.GetAsync("/LabResults");
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                
                var specimenDetailsSections = document.QuerySelectorAll(".specimen-details");
                Assert.Equal(expectedLabReferenceNumbers.Count, specimenDetailsSections.Length);
                
                foreach (var expectedLabReferenceNumber in expectedLabReferenceNumbers)
                {
                    var header = document.QuerySelector($"#specimen-{expectedLabReferenceNumber}");
                    Assert.NotNull(header);
                }
            }
        }
    }
}
