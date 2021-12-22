using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class HospitalDetailsPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditHospitalDetails;

        public HospitalDetailsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFIED_WITH_TBSERVICE,
                    NotificationStatus = NotificationStatus.Notified,
                    HospitalDetails = new HospitalDetails()
                    {
                        TBServiceCode = "A code",
                        TBService = new TBService() { Name = "A name", Code = "A code"}
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.DRAFT_WITH_TBSERVICE,
                    NotificationStatus = NotificationStatus.Draft,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFIED_WITH_ACTIVE_HOSPITAL,
                    NotificationStatus = NotificationStatus.Notified,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_SOUTH_TYNESIDE_DISTRICT_HOSPITAL_ID),
                        CaseManagerId = Utilities.CASEMANAGER_GATESHEAD_ID1,
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFIED_WITH_INACTIVE_CASEMANAGER,
                    NotificationStatus = NotificationStatus.Notified,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_SOUTH_TYNESIDE_DISTRICT_HOSPITAL_ID),
                        CaseManagerId = Utilities.CASEMANAGER_GATESHEAD_INACTIVE_ID,
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFIED_WITH_LEGACY_HOSPITAL,
                    NotificationStatus = NotificationStatus.Notified,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_DUNSTON_HILL_HOSPITAL_GATESHEAD_ID),
                        CaseManagerId = Utilities.CASEMANAGER_GATESHEAD_ID1,
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFIED_WITH_ACTIVE_CASEMANAGER_NOT_IN_TB_SERVICE,
                    NotificationStatus = NotificationStatus.Notified,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                        HospitalId = Guid.Parse(Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID),
                        CaseManagerId = Utilities.CASEMANAGER_GATESHEAD_ID2,
                    }
                }
            };
        }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date does not have a valid date selection");
        }

        [Fact]
        public async Task PostDraft_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            var expectedMessage = "Notification date must not be before 01/01/2010";
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", expectedMessage);
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRequiredError_IfNotificationDateNotSet()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date is a mandatory field");
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateInvalidError_IfNotificationDateIsNotDate()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "aa",
                ["FormattedNotificationDate.Month"] = "aa",
                ["FormattedNotificationDate.Year"] = "aa",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date does not have a valid date selection");
        }

        [Fact]
        public async Task PostNotified_ReturnsWithNotificationDateRangeError_IfNotificationDateIsNotInRange()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "1990",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date must not be before 01/01/2010");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllRequiredErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString()
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("Notification-NotificationDate", "notification-date", "Notification date is a mandatory field");
            resultDocument.AssertErrorSummaryMessage("HospitalDetails-HospitalId", "hospital", "Hospital is a mandatory field");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithTextFieldInputError_IfTextIsInvalid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["HospitalDetails.Consultant"] = "Name-1£"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("HospitalDetails-Consultant", "consultant", "Invalid character found in Consultant");
        }

        [Fact]
        public async Task PostDraft_RedirectToNextPage_IfModelIsValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.Consultant"] = "Consultant",
                ["HospitalDetails.CaseManagerId"] = Utilities.CASEMANAGER_ABINGDON_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditClinicalDetails, id), GetRedirectLocation(result));
        }

        [Fact]
        public async Task PostNotifiedChangingCaseManager_RedirectToNextPage_IfModelIsValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.Consultant"] = "Consultant",
                ["HospitalDetails.CaseManagerId"] = Utilities.CASEMANAGER_ABINGDON_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        }

        [Fact]
        public async Task OnGetNotifiedWithInactiveCaseManager_HasCaseManagerInDropdown()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_INACTIVE_CASEMANAGER;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            // Assert
            var caseManagerDropdown = (IHtmlSelectElement)initialDocument.GetElementById("HospitalDetails_CaseManagerId");
            Assert.Contains(Utilities.CASEMANAGER_GATESHEAD_INACTIVE_DISPLAY_NAME, caseManagerDropdown.Options.Select(c => c.Label));
        }

        [Fact]
        public async Task OnGetNotifiedWithActiveCaseManager_HasNoInactiveCaseManagerInDropdown()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_ACTIVE_HOSPITAL;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            // Assert
            var caseManagerDropdown = (IHtmlSelectElement)initialDocument.GetElementById("HospitalDetails_CaseManagerId");
            var caseManagerOptions = caseManagerDropdown.Options
                .Where(option => !string.IsNullOrEmpty(option.Value))
                .Select(option => new OptionValue {Text = option.TextContent, Value = option.Value});
            AssertAreGatesheadCaseManagers(caseManagerOptions);
        }

        [Fact]
        public async Task PostNotified_WithInactiveCaseManager_RedirectToNextPage_IfModelIsValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_INACTIVE_CASEMANAGER;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_DUNSTON_HILL_HOSPITAL_GATESHEAD_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                ["HospitalDetails.Consultant"] = "Consultant",
                ["HospitalDetails.CaseManagerId"] = Utilities.CASEMANAGER_GATESHEAD_INACTIVE_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        }

        [Fact]
        public async Task PostDraft_HospitalDoesNotMatchTbService_ReturnsValidationError()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_FULWOOD_HALL_HOSPITAL_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID,
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("HospitalDetails-HospitalId", "hospital", "Hospital must belong to selected TB Service");
        }

        [Fact]
        public async Task PostNotified_CaseManagerAlreadyOnNotificationDoesNotMatchTbService_IsValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_ACTIVE_CASEMANAGER_NOT_IN_TB_SERVICE;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.Consultant"] = "Consultant",
                ["HospitalDetails.CaseManagerId"] = Utilities.CASEMANAGER_GATESHEAD_ID2.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var sectionAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPath);
            result.AssertRedirectTo($"/Notifications/{id}#{sectionAnchorId}");
        }

        [Fact]
        public async Task PostNotified_CaseManagerChangesAndDoesNotMatchTbService_IsNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_ACTIVE_CASEMANAGER_NOT_IN_TB_SERVICE;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.Consultant"] = "Consultant",
                ["HospitalDetails.CaseManagerId"] = Utilities.CASEMANAGER_GATESHEAD_ID1.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("HospitalDetails-CaseManagerId", "case-manager", "Case Manager must be allowed for selected TB Service");
        }

        [Fact]
        public async Task PostNotified_AddCaseManagerThatDoesNotMatchTbService_IsNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["HospitalDetails.CaseManagerId"] = Utilities.CASEMANAGER_ABINGDON_ID.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_SOUTH_TYNESIDE_DISTRICT_HOSPITAL_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                ["HospitalDetails.Consultant"] = "Consultant",
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("HospitalDetails-CaseManagerId", "case-manager", "Case Manager must be allowed for selected TB Service");
        }

        [Fact]
        public async Task PostNotified_TBServiceHasChanged_ReturnsValidationError()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_WITH_TBSERVICE);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_WITH_TBSERVICE.ToString(),
                ["HospitalDetails.TBServiceCode"] = "ChangedTBServiceCode"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();
            // Here we check that the page has reloaded as the form is invalid, we can't check directly for an error as
            // no error is shown for this case
            Assert.NotNull(resultDocument.QuerySelector($"#hospital-details-page-content"));
        }

        [Fact]
        public async Task GetFilteredListsByTbService_ReturnsExpectedValues_ForDraftWithoutHospital()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
            };

            // Act
            var response = await Client.GetAsync(
                GetHandlerPath(formData, "GetFilteredListsByTbService", Utilities.DRAFT_WITH_TBSERVICE)
            );

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var filteredLists = JsonConvert.DeserializeObject<FilteredHospitalDetailsPageSelectLists>(result);
            Assert.NotNull(filteredLists);
            AssertAreGatesheadCaseManagers(filteredLists.CaseManagers);

            Assert.Collection(filteredLists.Hospitals.OrderBy(selectOption => selectOption.Value),
                selectOption =>
                    Assert.Equal(Utilities.HOSPITAL_QUEEN_ELIZABETH_HOSPITAL_GATESHEAD_ID, selectOption.Value.ToUpper()),
                selectOption =>
                    Assert.Equal(Utilities.HOSPITAL_SOUTH_TYNESIDE_DISTRICT_HOSPITAL_ID, selectOption.Value.ToUpper()));
        }

        [Fact]
        public async Task GetFilteredListsByTbService_ReturnsExpectedValues_ForNotifiedWithActiveHospital()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
            };

            // Act
            var response = await Client.GetAsync(
                GetHandlerPath(formData, "GetFilteredListsByTbService", Utilities.NOTIFIED_WITH_ACTIVE_HOSPITAL)
            );

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var filteredLists = JsonConvert.DeserializeObject<FilteredHospitalDetailsPageSelectLists>(result);
            Assert.NotNull(filteredLists);
            AssertAreGatesheadCaseManagers(filteredLists.CaseManagers);

            Assert.Collection(filteredLists.Hospitals.OrderBy(selectOption => selectOption.Value),
                selectOption =>
                    Assert.Equal(Utilities.HOSPITAL_QUEEN_ELIZABETH_HOSPITAL_GATESHEAD_ID, selectOption.Value.ToUpper()),
                selectOption =>
                    Assert.Equal(Utilities.HOSPITAL_SOUTH_TYNESIDE_DISTRICT_HOSPITAL_ID, selectOption.Value.ToUpper()));
        }

        [Fact]
        public async Task GetFilteredListsByTbService_ReturnsExpectedValues_ForNotifiedWithLegacyHospital()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
            };

            // Act
            var response = await Client.GetAsync(
                GetHandlerPath(formData, "GetFilteredListsByTbService", Utilities.NOTIFIED_WITH_LEGACY_HOSPITAL)
            );

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var filteredLists = JsonConvert.DeserializeObject<FilteredHospitalDetailsPageSelectLists>(result);
            Assert.NotNull(filteredLists);
            AssertAreGatesheadCaseManagers(filteredLists.CaseManagers);

            Assert.Collection(filteredLists.Hospitals.OrderBy(selectOption => selectOption.Value),
                selectOption =>
                    Assert.Equal(Utilities.HOSPITAL_DUNSTON_HILL_HOSPITAL_GATESHEAD_ID, selectOption.Value.ToUpper()),
                selectOption =>
                    Assert.Equal(Utilities.HOSPITAL_QUEEN_ELIZABETH_HOSPITAL_GATESHEAD_ID, selectOption.Value.ToUpper()),
                selectOption =>
                    Assert.Equal(Utilities.HOSPITAL_SOUTH_TYNESIDE_DISTRICT_HOSPITAL_ID, selectOption.Value.ToUpper()));
        }

        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment_ForNotified()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["HospitalDetails.HospitalId"] = Utilities.HOSPITAL_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.TBServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["HospitalDetails.Consultant"] = "Consultant",
                ["HospitalDetails.CaseManagerId"] = Utilities.CASEMANAGER_ABINGDON_ID.ToString(),
                ["FormattedNotificationDate.Day"] = "1",
                ["FormattedNotificationDate.Month"] = "1",
                ["FormattedNotificationDate.Year"] = "2012",
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
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }

        private static void AssertAreGatesheadCaseManagers(IEnumerable<OptionValue> caseManagers)
        {
            Assert.Collection(caseManagers.OrderBy(selectOption => selectOption.Value), selectOption =>
            {
                Assert.Equal(Utilities.CASEMANAGER_GATESHEAD_DISPLAY_NAME1, selectOption.Text);
                Assert.Equal(Utilities.CASEMANAGER_GATESHEAD_ID1.ToString(), selectOption.Value);
            }, selectOption =>
            {
                Assert.Equal(Utilities.CASEMANAGER_GATESHEAD_DISPLAY_NAME2, selectOption.Text);
                Assert.Equal(Utilities.CASEMANAGER_GATESHEAD_ID2.ToString(), selectOption.Value);
            });
        }
    }
}
