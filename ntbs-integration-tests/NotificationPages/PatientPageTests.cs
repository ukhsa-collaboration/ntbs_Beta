using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class PatientPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditPatientDetails;

        public PatientPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IEnumerable<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification
                {
                    NotificationId = Utilities.PATIENT_GROUPED_NOTIFIED_NOTIFICATION_1_SHARED_NHS_NUMBER_1,
                    NotificationStatus = NotificationStatus.Notified,
                    GroupId = Utilities.PATIENT_NOTIFICATION_GROUP_ID,
                    PatientDetails = new PatientDetails
                    {
                        NhsNumberNotKnown = false,
                        NhsNumber = Utilities.NHS_NUMBER_SHARED_1
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.PATIENT_GROUPED_NOTIFIED_NOTIFICATION_2_SHARED_NHS_NUMBER_1,
                    NotificationStatus = NotificationStatus.Denotified,
                    GroupId = Utilities.PATIENT_NOTIFICATION_GROUP_ID,
                    PatientDetails = new PatientDetails
                    {
                        NhsNumberNotKnown = false,
                        NhsNumber = Utilities.NHS_NUMBER_SHARED_1
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.PATIENT_DRAFT_NOTIFICATION_SHARED_NHS_NUMBER_1,
                    NotificationStatus = NotificationStatus.Draft,
                    PatientDetails = new PatientDetails
                    {
                        NhsNumberNotKnown = false,
                        NhsNumber = Utilities.NHS_NUMBER_SHARED_1
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.PATIENT_NOTIFIED_NOTIFICATION_SHARED_NHS_NUMBER_1,
                    NotificationStatus = NotificationStatus.Notified,
                    PatientDetails = new PatientDetails
                    {
                        NhsNumberNotKnown = false,
                        NhsNumber = Utilities.NHS_NUMBER_SHARED_1
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.PATIENT_NOTIFIED_NOTIFICATION_1_SHARED_NHS_NUMBER_2,
                    NotificationStatus = NotificationStatus.Notified,
                    PatientDetails = new PatientDetails
                    {
                        NhsNumberNotKnown = false,
                        NhsNumber = Utilities.NHS_NUMBER_SHARED_2
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.PATIENT_DENOTIFIED_NOTIFICATION_2_SHARED_NHS_NUMBER_2,
                    NotificationStatus = NotificationStatus.Denotified,
                    PatientDetails = new PatientDetails
                    {
                        NhsNumberNotKnown = false,
                        NhsNumber = Utilities.NHS_NUMBER_SHARED_2
                    }
                }
            };
        }

        [Fact]
        public async Task PostDraft_ReturnsPageWithModelErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["PatientDetails.GivenName"] = "111",
                ["PatientDetails.FamilyName"] = "111",
                ["FormattedDob.Day"] = "31",
                ["FormattedDob.Month"] = "12",
                ["FormattedDob.Year"] = "1899",
                ["PatientDetails.NhsNumber"] = "123",
                ["PatientDetails.Address"] = "$$$",
                ["PatientDetails.LocalPatientId"] = "|||"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorSummaryMessage("PatientDetails-GivenName", "given-name", "Given name can only contain letters and the symbols ' - . ,");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-FamilyName", "family-name", "Family name can only contain letters and the symbols ' - . ,");
            // Cannot easily check for equality with FullErrorMessage here as the error field is formatted oddly due to there being two fields in the error span.
            Assert.Contains("Date of birth must not be before 01/01/1900", resultDocument.GetError("dob"));
            resultDocument.AssertErrorSummaryMessage("PatientDetails-NhsNumber", "nhs-number", "NHS number needs to be 10 digits long");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-Address", "address", "Address can only contain letters, numbers and the symbols ' - . , /");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-LocalPatientId", "local-patient-id", "Invalid character found in Local patient ID");
        }

        [Fact]
        public async Task PostDraft_ReturnsPageWithModelErrors_IfYearOfUkEntryBeforeDob()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedDob.Day"] = "31",
                ["FormattedDob.Month"] = "12",
                ["FormattedDob.Year"] = "2000",
                ["PatientDetails.CountryId"] = "1",
                ["PatientDetails.YearOfUkEntry"] = "1999"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorSummaryMessage(
                "PatientDetails-YearOfUkEntry",
                "year-of-entry",
                "Year of uk entry must be later than date of birth year");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["PatientDetails.NhsNumberNotKnown"] = "false",
                ["PatientDetails.NoFixedAbode"] = "false",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorSummaryMessage("PatientDetails-FamilyName", "family-name", "Family name is a mandatory field");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-GivenName", "given-name", "Given name is a mandatory field");
            Assert.Contains("Date of birth is a mandatory field", resultDocument.GetError("dob"));
            resultDocument.AssertErrorSummaryMessage("PatientDetails-NhsNumber", "nhs-number", "NHS number is a mandatory field");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-Postcode", "postcode", "Postcode is a mandatory field");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-SexId", "sex", "Sex is a mandatory field");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-EthnicityId", "ethnicity", "Ethnic group is a mandatory field");
            resultDocument.AssertErrorSummaryMessage("PatientDetails-CountryId", "birth-country", "Birth country is a mandatory field");
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            const string givenName = "Test";
            const string familyName = "User";
            const string birthDay = "5";
            const string birthMonth = "5";
            const string birthYear = "1992";
            const string nhsNumber = "9123456789";
            const string address = "123 Fake Street, London";
            const string ethnicityId = "1";
            const string sexId = "2";
            const string countryId = "3";
            const string localPatientId = "123#";
            const string occupationId = "1";
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["PatientDetails.GivenName"] = givenName,
                ["PatientDetails.FamilyName"] = familyName,
                ["FormattedDob.Day"] = birthDay,
                ["FormattedDob.Month"] = birthMonth,
                ["FormattedDob.Year"] = birthYear,
                ["PatientDetails.NhsNumber"] = nhsNumber,
                ["PatientDetails.Address"] = address,
                ["PatientDetails.NoFixedAbode"] = "true",
                ["PatientDetails.Postcode"] = "NW5 1TL",
                ["PatientDetails.EthnicityId"] = ethnicityId,
                ["PatientDetails.SexId"] = sexId,
                ["PatientDetails.CountryId"] = countryId,
                ["PatientDetails.LocalPatientId"] = localPatientId,
                ["PatientDetails.OccupationId"] = occupationId
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditHospitalDetails, id), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputTextValue("PatientDetails_GivenName", givenName);
            reloadedDocument.AssertInputTextValue("PatientDetails_FamilyName", familyName);
            reloadedDocument.AssertInputTextValue("FormattedDob_Day", birthDay);
            reloadedDocument.AssertInputTextValue("FormattedDob_Month", birthMonth);
            reloadedDocument.AssertInputTextValue("FormattedDob_Year", birthYear);
            reloadedDocument.AssertInputTextValue("PatientDetails_NhsNumber", nhsNumber);
            reloadedDocument.AssertInputTextValue("PatientDetails_Postcode", string.Empty);
            reloadedDocument.AssertInputTextValue("PatientDetails_LocalPatientId", localPatientId);
            reloadedDocument.AssertInputSelectValue("PatientDetails_EthnicityId", ethnicityId);
            reloadedDocument.AssertInputSelectValue("PatientDetails_CountryId", countryId);
            reloadedDocument.AssertInputSelectValue("PatientDetails_OccupationId", occupationId);
            reloadedDocument.AssertInputRadioValue("sexId-2", true);
            reloadedDocument.AssertTextAreaValue("PatientDetails_Address", address);
        }

        [Fact]
        public async Task NonDuplicateNhsNumber_DoesNotShowWarning()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);

            // Act
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            // Assert
            Assert.True(initialDocument.GetElementById("nhs-number-warning").ClassList.Contains("hidden"));
        }

        public static IEnumerable<object[]> WarningScenarios()
        {
            yield return new object[]
            {
                Utilities.PATIENT_DRAFT_NOTIFICATION_SHARED_NHS_NUMBER_1,
                new List<int>
                {
                    Utilities.PATIENT_NOTIFIED_NOTIFICATION_SHARED_NHS_NUMBER_1,
                    Utilities.PATIENT_GROUPED_NOTIFIED_NOTIFICATION_1_SHARED_NHS_NUMBER_1
                }
            };
            yield return new object[]
            {
                Utilities.PATIENT_NOTIFIED_NOTIFICATION_SHARED_NHS_NUMBER_1,
                new List<int>
                {
                    Utilities.PATIENT_GROUPED_NOTIFIED_NOTIFICATION_1_SHARED_NHS_NUMBER_1
                }
            };
        }

        [Theory, MemberData(nameof(WarningScenarios))]
        public async Task DuplicateNhsNumber_ShowsWarningsWithExpectedIds(int pageNotificationId, List<int> expectedWarningNotificationIds)
        {
            // Arrange
            var url = GetCurrentPathForId(pageNotificationId);

            // Act
            var initialDocument = await GetDocumentForUrlAsync(url);

            // Assert
            Assert.False(initialDocument.GetElementById("nhs-number-warning").ClassList.Contains("hidden"));
            var linksContainer = initialDocument.GetElementById("nhs-number-links");
            Assert.Equal(expectedWarningNotificationIds.Count, linksContainer.ChildElementCount);

            foreach (var notificationId in expectedWarningNotificationIds)
            {
                var warningUrl = RouteHelper.GetNotificationPath(notificationId, NotificationSubPaths.Overview);
                Assert.Equal($"#{notificationId}", linksContainer.QuerySelector($"a[href='{warningUrl}']").TextContent);
            }
        }

        [Fact]
        public async Task OnPostNhsNumberDuplicates_ReturnsExpectedEmptyResponseForNonDuplicate()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            const string nonDuplicateNhsNumber = "9876543219";
            var initialPage = await Client.GetAsync(GetCurrentPathForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new NhsNumberValidationModel
            {
                NotificationId = id,
                NhsNumber = nonDuplicateNhsNumber
            };

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "NhsNumberDuplicates", id),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("{}", result);
        }

        [Fact]
        public async Task OnPostNhsNumberDuplicates_ReturnsEmptyResponseForDenotifiedDuplicateNhsNumber()
        {
            // Arrange
            const int id = Utilities.PATIENT_NOTIFIED_NOTIFICATION_1_SHARED_NHS_NUMBER_2;
            const string nhsNumber = Utilities.NHS_NUMBER_SHARED_2;
            var initialPage = await Client.GetAsync(GetCurrentPathForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new NhsNumberValidationModel
            {
                NotificationId = id,
                NhsNumber = nhsNumber
            };

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "NhsNumberDuplicates", id),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("{}", result);
        }

        [Fact]
        public async Task OnPostNhsNumberDuplicates_ReturnsResponseWithLegacyDuplicate()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            string nhsNumber = Utilities.NHS_NUMBER_LEGACY_DUPLICATE;
            var initialPage = await Client.GetAsync(GetCurrentPathForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new NhsNumberValidationModel
            {
                NotificationId = id,
                NhsNumber = nhsNumber
            };

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "NhsNumberDuplicates", id),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains(Utilities.NHS_NUMBER_LEGACY_DUPLICATE_SOURCE
                            + ": " + Utilities.NHS_NUMBER_LEGACY_DUPLICATE_ID, result);
        }

        [Fact]
        public async Task OnPostNhsNumberDuplicates_ReturnsExpectedResponseForGroupedDuplicateNhsNumber()
        {
            // Arrange
            const int id = Utilities.PATIENT_GROUPED_NOTIFIED_NOTIFICATION_1_SHARED_NHS_NUMBER_1;
            const string nhsNumber = Utilities.NHS_NUMBER_SHARED_1;
            var initialPage = await Client.GetAsync(GetCurrentPathForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new NhsNumberValidationModel
            {
                NotificationId = id,
                NhsNumber = nhsNumber
            };
            const int expectedWarningNotificationId = Utilities.PATIENT_NOTIFIED_NOTIFICATION_SHARED_NHS_NUMBER_1;
            var expectedWarningUrl = RouteHelper.GetNotificationPath(expectedWarningNotificationId, NotificationSubPaths.Overview);

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "NhsNumberDuplicates", Utilities.NOTIFIED_ID),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains($"\"{expectedWarningNotificationId}\":\"{expectedWarningUrl}\"", result);
        }

        [Fact]
        public async Task IfDateTooEarly_ValidatePatientDate_ReturnsEarliestBirthDateErrorMessage()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new DateValidationModel
            {
                Key = "Dob",
                Day = "1",
                Month = "1",
                Year = "1899"
            };

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "ValidatePatientDetailsDate", Utilities.NOTIFIED_ID),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Date of birth must not be before 01/01/1900", result);
        }

        [Theory]
        [InlineData("ABC", "NHS number can only contain digits 0-9")]
        [InlineData("123", "NHS number needs to be 10 digits long")]
        [InlineData("5647382911", "This NHS number is not valid. Confirm you have entered it correctly")]
        public async Task WhenNhsNumberInvalid_ValidatePatientProperty_ReturnsExpectedResult(string nhsNumber, string validationResult)
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new InputValidationModel
            {
                Key = "NhsNumber",
                Value = nhsNumber
            };

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "ValidatePatientDetailsProperty", Utilities.NOTIFIED_ID),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }

        [Theory]
        [InlineData(true, "NHS number is a mandatory field")]
        [InlineData(false, "")]
        public async Task DependentOnShouldValidateFull_ValidatePatientProperty_ReturnsRequiredOrNoError(bool shouldValidateFull, string validationResult)
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new InputValidationModel
            {
                ShouldValidateFull = shouldValidateFull,
                Key = "NhsNumber"
            };

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "ValidatePatientDetailsProperty", Utilities.NOTIFIED_ID),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }

        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment_ForNotified()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_2;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["PatientDetails.GivenName"] = "Test",
                ["PatientDetails.FamilyName"] = "User",
                ["FormattedDob.Day"] = "5",
                ["FormattedDob.Month"] = "5",
                ["FormattedDob.Year"] = "2000",
                ["PatientDetails.NhsNumber"] = "5864552852",
                ["PatientDetails.Postcode"] = "NW5 1TL",
                ["PatientDetails.EthnicityId"] = "1",
                ["PatientDetails.SexId"] = "1",
                ["PatientDetails.CountryId"] = "1",
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
    }
}
