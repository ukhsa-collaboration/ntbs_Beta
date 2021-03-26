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
    public class MBovisAnimalExposurePageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditMBovisAnimalExposures;

        public MBovisAnimalExposurePageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_ANIMAL_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile {Species = "M. bovis"},
                    MBovisDetails = new MBovisDetails
                    {
                        AnimalExposureStatus = Status.Yes,
                        MBovisAnimalExposures = new List<MBovisAnimalExposure>
                        {
                            new MBovisAnimalExposure
                            {
                                YearOfExposure = 2000,
                                AnimalType = AnimalType.WildAnimal,
                                Animal = "Fake Animal",
                                ExposureDuration = 1,
                                AnimalTbStatus = AnimalTbStatus.ConfirmedTb,
                                CountryId = 1
                            }
                        }
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NULL_ANIMAL_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" },
                    MBovisDetails = new MBovisDetails { AnimalExposureStatus = null }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NO_ANIMAL_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" },
                    MBovisDetails = new MBovisDetails { AnimalExposureStatus = Status.No }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_UNKNOWN_ANIMAL_NO_ENTITIES,
                    NotificationStatus = NotificationStatus.Notified,
                    DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" },
                    MBovisDetails = new MBovisDetails { AnimalExposureStatus = Status.Unknown }
                }
            };
        }

        [Fact]
        public async Task IfNotificationHasKnownCases_DisplaysTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_MBOVIS_ANIMAL_ENTITIES;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(document.QuerySelector("#mbovis-animal-exposure-table"));
        }

        [Theory]
        [InlineData(Utilities.NOTIFICATION_ID_WITH_MBOVIS_NULL_ANIMAL_NO_ENTITIES)]
        [InlineData(Utilities.NOTIFICATION_ID_WITH_MBOVIS_NO_ANIMAL_NO_ENTITIES)]
        [InlineData(Utilities.NOTIFICATION_ID_WITH_MBOVIS_UNKNOWN_ANIMAL_NO_ENTITIES)]
        public async Task IfNotificationDoesNotHaveKnownCases_DoesNotDisplayTable(int notificationId)
        {
            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(document.QuerySelector("#mbovis-animal-exposure-table"));
        }

        [Fact]
        public async Task EditPage_RedirectsToOverviewWithCorrectAnchorFragmentAndSavesContent_IfModelValid()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_NULL_OTHER_CASE_NO_ENTITIES;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["MBovisDetails.AnimalExposureStatus"] = Status.No.ToString()
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var sectionAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPath);
            result.AssertRedirectTo($"/Notifications/{id}#{sectionAnchorId}");

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputRadioValue("has-exposure-no", true);
        }

        [Fact]
        public async Task NotifiedPageHasReturnLinkToOverview()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_ANIMAL_ENTITIES;
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
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_ANIMAL_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisAnimalExposure, id);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisAnimalExposure.YearOfExposure"] = "1899",
                ["MBovisAnimalExposure.CountryId"] = "",
                ["MBovisAnimalExposure.AnimalType"] = "",
                ["MBovisAnimalExposure.Animal"] = "",
                ["MBovisAnimalExposure.AnimalTbStatus"] = "",
                ["MBovisAnimalExposure.ExposureDuration"] = "111",
                ["MBovisAnimalExposure.OtherDetails"] = "£$£$£"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();

            resultDocument.AssertErrorSummaryMessage(
                "MBovisAnimalExposure-YearOfExposure",
                "year-of-exposure",
                "Year of exposure has an invalid year");
            resultDocument.AssertErrorSummaryMessage(
                "MBovisAnimalExposure-ExposureDuration",
                "duration",
                "The field Duration (years) must be between 1 and 99.");
            resultDocument.AssertErrorSummaryMessage(
                "MBovisAnimalExposure-OtherDetails",
                "other-details",
                "Invalid character found in Other details");
        }

        [Fact]
        public async Task AddPage_WhenModelValid_RedirectsToCollectionViewAndSavesChanges()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_MBOVIS_ANIMAL_ENTITIES;
            var url = GetPathForId(NotificationSubPaths.AddMBovisAnimalExposure, id);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["MBovisAnimalExposure.YearOfExposure"] = "2010",
                ["MBovisAnimalExposure.CountryId"] = "3",
                ["MBovisAnimalExposure.AnimalType"] = ((int)AnimalType.Pet).ToString(),
                ["MBovisAnimalExposure.Animal"] = "Badger",
                ["MBovisAnimalExposure.AnimalTbStatus"] = ((int)AnimalTbStatus.SuspectedTb).ToString(),
                ["MBovisAnimalExposure.ExposureDuration"] = "12",
                ["MBovisAnimalExposure.OtherDetails"] = "Some other testing details"
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertRedirectTo(
                RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditMBovisAnimalExposures));

            // Find the edit page for the newly added animal exposure event. We don't know what ID the database
            // will give this event, so we can't generate the URL. Instead, we take it from the event's edit link
            var animalExposuresDocument = await GetDocumentForUrlAsync(GetRedirectLocation(result));
            var newAnimalExposureUrl = animalExposuresDocument.QuerySelectorAll(".notification-edit-link")
                .First()
                .Attributes
                .GetNamedItem("href")
                .Value;
            var newAnimalExposureDocument = await GetDocumentForUrlAsync(newAnimalExposureUrl);

            newAnimalExposureDocument.AssertInputTextValue("MBovisAnimalExposure_YearOfExposure", "2010");
            newAnimalExposureDocument.AssertInputSelectValue("MBovisAnimalExposure_AnimalType",
                ((int)AnimalType.Pet).ToString());
            newAnimalExposureDocument.AssertInputSelectValue("MBovisAnimalExposure_CountryId", "3");
            newAnimalExposureDocument.AssertInputTextValue("MBovisAnimalExposure_Animal", "Badger");
            newAnimalExposureDocument.AssertInputSelectValue("MBovisAnimalExposure_AnimalTbStatus",
                ((int)AnimalTbStatus.SuspectedTb).ToString());
            newAnimalExposureDocument.AssertInputTextValue("MBovisAnimalExposure_ExposureDuration", "12");
            newAnimalExposureDocument.AssertTextAreaValue("MBovisAnimalExposure_OtherDetails",
                "Some other testing details");
        }
    }
}
