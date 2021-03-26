using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class MBovisUnpasteurisedMilkConsumptionPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditMBovisUnpasteurisedMilkConsumptions;

        public MBovisUnpasteurisedMilkConsumptionPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_MILK_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" },
                    MBovisDetails = new MBovisDetails
                    {
                        UnpasteurisedMilkConsumptionStatus = Status.Yes,
                        MBovisUnpasteurisedMilkConsumptions = new List<MBovisUnpasteurisedMilkConsumption>
                        {
                            new MBovisUnpasteurisedMilkConsumption
                            {
                                YearOfConsumption = 2000,
                                MilkProductType = MilkProductType.Cheese,
                                CountryId = 1,
                                ConsumptionFrequency = ConsumptionFrequency.Once
                            }
                        }
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NULL_MILK_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" },
                    MBovisDetails = new MBovisDetails { UnpasteurisedMilkConsumptionStatus = null }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NO_MILK_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" },
                    MBovisDetails = new MBovisDetails { UnpasteurisedMilkConsumptionStatus = Status.No }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_UNKNOWN_MILK_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" },
                    MBovisDetails = new MBovisDetails { UnpasteurisedMilkConsumptionStatus = Status.Unknown }
                }
            };
        }

        [Fact]
        public async Task IfNotificationHasKnownCases_DisplaysTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_MILK_ENTITIES;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(document.QuerySelector("#mbovis-milk-consumption-table"));
        }

        [Theory]
        [InlineData(Utilities.NOTIFICATION_ID_WITH_MBOVIS_NULL_MILK_NO_ENTITIES)]
        [InlineData(Utilities.NOTIFICATION_ID_WITH_MBOVIS_NO_MILK_NO_ENTITIES)]
        [InlineData(Utilities.NOTIFICATION_ID_WITH_MBOVIS_UNKNOWN_MILK_NO_ENTITIES)]
        public async Task IfNotificationDoesNotHaveKnownCases_DoesNotDisplayTable(int notificationId)
        {
            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(document.QuerySelector("#mbovis-milk-consumption-table"));
        }

        [Fact]
        public async Task EditPage_RedirectsToOverviewWithCorrectAnchorFragmentAndSavesContent_IfModelValid()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NULL_MILK_NO_ENTITIES;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["MBovisDetails.UnpasteurisedMilkConsumptionStatus"] = Status.No.ToString()
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var sectionAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPath);
            result.AssertRedirectTo($"/Notifications/{id}#{sectionAnchorId}");

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputRadioValue("has-milk-consumption-no", true);
        }

        [Fact]
        public async Task NotifiedPageHasReturnLinkToOverview()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_MILK_ENTITIES;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }

        [Fact]
        public async Task AddPage_WhenModelInvalid_ShowsExpectedValidationMessages()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_MILK_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisUnpasteurisedMilkConsumption, id);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisUnpasteurisedMilkConsumption.YearOfConsumption"] = "1899",
                ["MBovisUnpasteurisedMilkConsumption.CountryId"] = "",
                ["MBovisUnpasteurisedMilkConsumption.MilkProductType"] = "",
                ["MBovisUnpasteurisedMilkConsumption.ConsumptionFrequency"] = "",
                ["MBovisUnpasteurisedMilkConsumption.OtherDetails"] = "$£$£$£"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();

            resultDocument.AssertErrorSummaryMessage(
                "MBovisUnpasteurisedMilkConsumption-YearOfConsumption",
                "year-of-consumption",
                "Year of consumption has an invalid year");
            resultDocument.AssertErrorSummaryMessage(
                "MBovisUnpasteurisedMilkConsumption-OtherDetails",
                "other-details",
                "Invalid character found in Other details");
        }

        [Fact]
        public async Task AddPage_WhenModelValid_RedirectsToCollectionViewAndSavesChanges()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_MILK_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisUnpasteurisedMilkConsumption, id);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisUnpasteurisedMilkConsumption.YearOfConsumption"] = "2010",
                ["MBovisUnpasteurisedMilkConsumption.CountryId"] = "3",
                ["MBovisUnpasteurisedMilkConsumption.MilkProductType"] = ((int)MilkProductType.Milk).ToString(),
                ["MBovisUnpasteurisedMilkConsumption.ConsumptionFrequency"] =
                    ((int)ConsumptionFrequency.Occasionally).ToString(),
                ["MBovisUnpasteurisedMilkConsumption.OtherDetails"] = "Some other testing details"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertRedirectTo(
                RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditMBovisUnpasteurisedMilkConsumptions));

            // Find the edit page for the newly added milk exposure event. We don't know what ID the database
            // will give this event, so we can't generate the URL. Instead, we take it from the event's edit link
            var milkExposureDocument = await GetDocumentForUrlAsync(GetRedirectLocation(result));
            var milkExposureUrl = milkExposureDocument.QuerySelectorAll(".notification-edit-link")
                .First()
                .Attributes
                .GetNamedItem("href")
                .Value;
            var newMilkExposureDocument = await GetDocumentForUrlAsync(milkExposureUrl);

            newMilkExposureDocument.AssertInputTextValue("MBovisUnpasteurisedMilkConsumption_YearOfConsumption",
                "2010");
            newMilkExposureDocument.AssertInputSelectValue("MBovisUnpasteurisedMilkConsumption_CountryId", "3");
            newMilkExposureDocument.AssertInputSelectValue("MBovisUnpasteurisedMilkConsumption_MilkProductType",
                ((int)MilkProductType.Milk).ToString());
            newMilkExposureDocument.AssertInputSelectValue("MBovisUnpasteurisedMilkConsumption_ConsumptionFrequency",
                ((int)ConsumptionFrequency.Occasionally).ToString());
            newMilkExposureDocument.AssertTextAreaValue("MBovisUnpasteurisedMilkConsumption_OtherDetails",
                "Some other testing details");
        }
    }
}
