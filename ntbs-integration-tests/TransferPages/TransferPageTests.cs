using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.TransferPages
{
    public class TransferPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.TransferRequest;
        public TransferPageTests(NtbsWebApplicationFactory<Program> factory) : base(factory) { }

        private static DateTime _notificationDate = new(2019, 11, 2);
        private static DateTime _diagnosisDate = new(2019, 11, 1);
        private static DateTime _transferDate = new(2020, 1, 1);

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new()
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_TRANSFER,
                    NotificationDate = _notificationDate,
                    HospitalDetails = new HospitalDetails
                    {
                        TBServiceCode = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID,
                        NotificationId = Utilities.NOTIFICATION_WITH_TRANSFER 
                    },
                    ClinicalDetails = new ClinicalDetails
                    {
                        DiagnosisDate = _diagnosisDate
                    },
                    TreatmentEvents = new List<TreatmentEvent> {
                        new()
                        {
                            EventDate = _transferDate,
                            TreatmentEventType = TreatmentEventType.TransferOut,
                            NotificationId = Utilities.NOTIFICATION_WITH_TRANSFER,
                            TbServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID
                        },
                        new()
                        {
                            EventDate = _transferDate,
                            TreatmentEventType = TreatmentEventType.TransferIn,
                            NotificationId = Utilities.NOTIFICATION_WITH_TRANSFER,
                            TbServiceCode = Utilities.TBSERVICE_GATESHEAD_AND_SOUTH_TYNESIDE_ID
                        }
                    }
                }
            };
        }

        [Fact]
        public async Task CreateTransferAlert_ReturnsPageWithModelErrors_IfAlertNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferRequest.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferRequest.TransferReason"] = nameof(TransferReason.Relocation),
                ["TransferRequest.OtherReasonDescription"] = "|||",
                ["TransferRequest.TransferRequestNote"] = "|||"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("description", "Other description can only contain letters, numbers and the symbols ' - . , /");
            resultDocument.AssertErrorMessage("optional-note", "Optional note can only contain letters, numbers and the symbols ' - . , /");
        }

        public static TheoryData<DateTime?, string> transferDateData => new()
        {
            { _diagnosisDate.AddDays(-5), String.Format(ValidationMessages.DateShouldBeLaterThanNotificationStart, "Transfer date") },
            { _transferDate.AddDays(-5), String.Format(ValidationMessages.DateShouldBeLaterThanLatestTransfer, "Transfer date") },
            { new DateTime(101, 1, 12), ValidationMessages.DateValidityRangeStart("Transfer date", ValidDates.EarliestAllowedDate) },
            { null, String.Format(ValidationMessages.Mandatory, "Transfer date") }
        };

        [Theory]
        [MemberData(nameof(transferDateData))]
        public async Task CreateTransferAlert_ReturnsPageDateModelErrors_IfTransferDateNotValid(DateTime? transferDate, string expectedErrorMessage)
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_WITH_TRANSFER;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferRequest.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferRequest.TransferReason"] = nameof(TransferReason.Relocation),
                ["FormattedTransferDate.Day"] = transferDate?.Day.ToString(),
                ["FormattedTransferDate.Month"] = transferDate?.Month.ToString(),
                ["FormattedTransferDate.Year"] = transferDate?.Year.ToString()
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("transfer-date", expectedErrorMessage);
        }

        [Fact]
        public async Task CreateTransferAlert_RedirectsToOverviewPage_IfAlertValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_2;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferRequest.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferRequest.TransferReason"] = nameof(TransferReason.Relocation),
                ["FormattedTransferDate.Day"] = "3",
                ["FormattedTransferDate.Month"] = "12",
                ["FormattedTransferDate.Year"] = "2021",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            result.AssertRedirectTo($"/Notifications/{id}");
        }

        [Fact]
        public async Task NavigatingToPendingTransfer_ReturnsReadOnlyPartial_WhenTransferAlertAlreadyExists()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            Assert.NotNull(initialDocument.QuerySelector("#cancel-transfer-button"));
        }

        [Fact]
        public async Task TransferPageDisplaysServiceDirectoryLink()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);
            var directoryLinkText = document.QuerySelector("#ntbs-service-directory-hint");
            
            // Assert
            Assert.NotNull(directoryLinkText);
            Assert.Contains("You can search for TB services and case managers", directoryLinkText.InnerHtml);
        }

        [Fact]
        public async Task NTBSServiceDirectoryLink_LinksToServiceDirectoryPage()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);
            var directoryLink = document.QuerySelector("#ntbs-service-directory-hint > a");

            //Assert
            Assert.NotNull(directoryLink);
            Assert.Contains("NTBS service directory", directoryLink.InnerHtml);
            Assert.Equal("/ServiceDirectory", directoryLink.GetAttribute("href"));
        }
    }
}
