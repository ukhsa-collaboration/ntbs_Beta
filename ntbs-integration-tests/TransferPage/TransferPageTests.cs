using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.TransferPage
{
    public class TransferPageTests : TestRunnerBase
    {
        public TransferPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Alert> GetSeedingAlerts()
        {
            return new List<Alert>
            {
                new TransferAlert 
                {
                    AlertType = AlertType.TransferRequest,
                    AlertId = Utilities.TRANSFER_ALERT_ID,
                    NotificationId = Utilities.NOTIFIED_ID,
                    TbServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                    CaseManagerEmail = Utilities.CASEMANAGER_ABINGDON_EMAIL,
                    AlertStatus = AlertStatus.Open
                }
            };
        }

        [Fact]
        public async Task CreateTransferAlert_ReturnsPageWithModelErrors_IfAlertNotValid()
        {
            // Arrange
        }

        public async Task CreateTransferAlert_ReturnsOverviewPage_IfAlertValid()
        {
            // Arrange
        }

        public async Task ClickingPendingTransferButton_ReturnsReadOnlyPartial_WhenTransferAlertAlreadyExists()
        {
            // Arrange
        }

        public async Task CancellingPendingTransfer_ReturnsCancelConfirmationPage()
        {
            // Arrange
        }
    }
}