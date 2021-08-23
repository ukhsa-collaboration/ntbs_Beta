using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    // This test suite is not designed to test that all validation is correct, only that the validation is
    // happening on all parts of a notification that we expect it to happen on.
    // We aim to write much more in depth tests for services that are missing them, including validation,
    // with this test suite being the start of that process.
    public partial class ImportValidatorTest
    {
        private readonly IImportValidator _importValidator;
        private readonly Mock<IReferenceDataRepository> _referenceDataRepositoryMock =
            new Mock<IReferenceDataRepository>();
        private readonly Mock<IImportLogger> _importLoggerMock = new Mock<IImportLogger>();

        private const int RunId = 12345;

        public ImportValidatorTest()
        {
            _importValidator = new ImportValidator(_importLoggerMock.Object, _referenceDataRepositoryMock.Object);
        }

        [Fact]
        public async Task ImportValidatorValidatesBaseModels()
        {
            // Arrange
            var notification = CreateBasicNotification();

            notification.MDRDetails = new MDRDetails
            {
                RelationshipToCase = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"
            };
            notification.PatientDetails = new PatientDetails { FamilyName = "<>Davis<>" };
            notification.ClinicalDetails = new ClinicalDetails { DiagnosisDate = new DateTime(2000, 1, 1) };
            notification.TravelDetails = new TravelDetails { HasTravel = Status.Yes, TotalNumberOfCountries = 51 };
            notification.ImmunosuppressionDetails = new ImmunosuppressionDetails
            {
                HasOther = true,
                OtherDescription = "<><>"
            };
            notification.HospitalDetails = new HospitalDetails { Consultant = "<>Fred<>" };
            notification.VisitorDetails = new VisitorDetails{ HasVisitor = Status.Yes, StayLengthInMonths1 = 3 };
            notification.ContactTracing = new ContactTracing { AdultsIdentified = -1 };
            notification.PreviousTbHistory = new PreviousTbHistory { PreviousTbDiagnosisYear = 1899 };
            notification.MBovisDetails.MBovisExposureToKnownCases = new List<MBovisExposureToKnownCase>
            {
                new MBovisExposureToKnownCase()
            };
            notification.MBovisDetails.ExposureToKnownCasesStatus = Status.No;
            notification.DrugResistanceProfile = new DrugResistanceProfile { Species = "M. bovis" };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, notification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Contains("MDRDetails: The field Relationship of the current case to the contact must be a string or array type with a maximum length of '90'.",
                errorMessages);
            Assert.Contains("PatientDetails: " + String.Format(ValidationMessages.StandardStringFormat, "Family name"),
                errorMessages);
            Assert.Contains("ClinicalDetails: " + ValidationMessages.DateValidityRangeStart("Diagnosis date", "01/01/2010"),
                errorMessages);
            Assert.Contains("TravelDetails: The field total number of countries must be between 1 and 50.",
                errorMessages);
            Assert.Contains("ImmunosuppressionDetails: " + String.Format(ValidationMessages.InvalidCharacter, "Immunosuppression type description"),
                errorMessages);
            Assert.Contains("HospitalDetails: " + String.Format(ValidationMessages.InvalidCharacter, "Consultant"),
                errorMessages);
            Assert.Contains("VisitorDetails: " + ValidationMessages.TravelOrVisitDurationHasCountry,
                errorMessages);
            Assert.Contains("PreviousTbHistory: " + ValidationMessages.ValidYear,
                errorMessages);
            Assert.Contains("MBovisExposureToKnownCase: " + String.Format(ValidationMessages.RequiredSelect, "Exposure setting"),
                errorMessages);
        }

        [Fact]
        public async Task ImportValidatorValidatesCollectionModels()
        {
            // Arrange
            var notification = CreateBasicNotification();

            notification.SocialContextAddresses.Add(new SocialContextAddress());
            notification.SocialContextVenues.Add(new SocialContextVenue());
            notification.TreatmentEvents.Add(new TreatmentEvent { EventDate = new DateTime(1899, 3, 3) });
            notification.MBovisDetails = new MBovisDetails
            {
                OccupationExposureStatus = Status.Yes,
                MBovisOccupationExposures = new List<MBovisOccupationExposure>(),
                ExposureToKnownCasesStatus = Status.Yes,
                MBovisExposureToKnownCases = new List<MBovisExposureToKnownCase>(),
                AnimalExposureStatus = Status.Yes,
                MBovisAnimalExposures = new List<MBovisAnimalExposure>(),
                UnpasteurisedMilkConsumptionStatus = Status.Yes,
                MBovisUnpasteurisedMilkConsumptions = new List<MBovisUnpasteurisedMilkConsumption>()
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, notification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();

            Assert.Contains("TreatmentEvent: Event Date must not be before 01/01/2010",
                errorMessages);
            Assert.Contains("SocialContextAddress: " + ValidationMessages.SupplyOneOfTheAddressFields,
                errorMessages);
            Assert.Contains("SocialContextVenue: " + ValidationMessages.SupplyOneOfTheVenueFields,
                errorMessages);
            Assert.Contains("MBovisDetails: " + ValidationMessages.HasNoExposureRecords,
                errorMessages);
            Assert.Contains("MBovisDetails: " + ValidationMessages.HasNoAnimalExposureRecords,
                errorMessages);
            Assert.Contains("MBovisDetails: " + ValidationMessages.HasNoOccupationExposureRecords,
                errorMessages);
            Assert.Contains("MBovisDetails: " + ValidationMessages.HasNoUnpasteurisedMilkConsumptionRecords,
                errorMessages);
        }

        [Fact]
        public async Task ImportValidatorCleansTestDataValues()
        {
            // Arrange
            var notification = CreateBasicNotification();
            notification.TestData = new TestData
            {
                ManualTestResults = new List<ManualTestResult>
                {
                    new ManualTestResult { Result = Result.Positive, TestDate = null } // these values would fail validation
                }
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, notification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();

            Assert.Empty(errorMessages);
        }

        [Fact]
        public async Task ImportValidatorCleansStrings()
        {
            // Arrange
            var notification = new Notification
            {
                PatientDetails = new PatientDetails
                {
                    GivenName = " Sher\tlock  ",
                    FamilyName = "Holm\t es",
                    LocalPatientId = "  PatientID\t1234 ",
                    Address = " 123 \t Some Street  ",
                    Postcode = " NW1\t6XE  ",
                    OccupationOther = "  Detective \tWork "
                },
                ClinicalDetails = new ClinicalDetails
                {
                    Notes = " Clinical\t  notes",
                    HealthcareDescription = " health \t\t care  ",
                    TreatmentRegimenOtherDescription = " treatment \t regimen "
                },
                HospitalDetails = new HospitalDetails {  Consultant = " Dr\tJohn\tDorian  " },
                ImmunosuppressionDetails = new ImmunosuppressionDetails
                {
                    OtherDescription = "  immune  system\tsuppressed "
                },
                DenotificationDetails = new DenotificationDetails
                {
                    OtherDescription = "  notification was  a\tmistake   "
                },
                MDRDetails = new MDRDetails
                {
                    RelationshipToCase = "  mystery\tsolving partner   "
                },
                TestData = new TestData { ManualTestResults = new List<ManualTestResult>() },
                NotificationSites = new List<NotificationSite>
                {
                    new NotificationSite { SiteDescription = " Hair\tand fingernails  "}
                },
                TreatmentEvents = new List<TreatmentEvent>
                {
                    new TreatmentEvent{ EventDate = new DateTime(1899, 3, 3), Note = " A \tnote   " }
                },
                SocialContextAddresses = new List<SocialContextAddress>
                {
                    new SocialContextAddress
                    {
                        Address = " 221B\tBaker St., London   ",
                        Details = "  Elementary,\tWatson ",
                        Postcode = " NW1\t6XE  "
                    }
                },
                SocialContextVenues = new List<SocialContextVenue>
                {
                    new SocialContextVenue
                    {
                        Address = "  32 Windsor\tGardens ",
                        Details = "  Peruvian\tbears ",
                        Name = "      Favourite bear's\thouse   ",
                        Postcode = "  W9\t3RG "
                    }
                },
                MBovisDetails = new MBovisDetails
                {
                    OccupationExposureStatus = Status.Yes,
                    MBovisOccupationExposures = new List<MBovisOccupationExposure>
                    {
                        new MBovisOccupationExposure { OtherDetails = " A friend\tat work  " }
                    },
                    ExposureToKnownCasesStatus = Status.Yes,
                    MBovisExposureToKnownCases = new List<MBovisExposureToKnownCase>
                    {
                        new MBovisExposureToKnownCase { OtherDetails = "  His\tsister" }
                    },
                    AnimalExposureStatus = Status.Yes,
                    MBovisAnimalExposures = new List<MBovisAnimalExposure>
                    {
                        new MBovisAnimalExposure { OtherDetails = " Cows and\t badgers  " }
                    },
                    UnpasteurisedMilkConsumptionStatus = Status.Yes,
                    MBovisUnpasteurisedMilkConsumptions = new List<MBovisUnpasteurisedMilkConsumption>
                    {
                        new MBovisUnpasteurisedMilkConsumption { OtherDetails = "\tCheese  " }
                    }
                }
            };

            // Act
            await _importValidator.CleanAndValidateNotification(null, RunId, notification);

            // Assert
            Assert.Equal("Sher lock", notification.PatientDetails.GivenName);
            Assert.Equal("Holm  es", notification.PatientDetails.FamilyName);
            Assert.Equal("PatientID 1234", notification.PatientDetails.LocalPatientId);
            Assert.Equal("123   Some Street", notification.PatientDetails.Address);
            Assert.Equal("NW1 6XE", notification.PatientDetails.Postcode);
            Assert.Equal("Detective  Work", notification.PatientDetails.OccupationOther);

            Assert.Equal("Clinical   notes", notification.ClinicalDetails.Notes);
            Assert.Equal("health    care", notification.ClinicalDetails.HealthcareDescription);
            Assert.Equal("treatment   regimen", notification.ClinicalDetails.TreatmentRegimenOtherDescription);

            Assert.Equal("Dr John Dorian", notification.HospitalDetails.Consultant);
            Assert.Equal("immune  system suppressed", notification.ImmunosuppressionDetails.OtherDescription);
            Assert.Equal("notification was  a mistake", notification.DenotificationDetails.OtherDescription);
            Assert.Equal("mystery solving partner", notification.MDRDetails.RelationshipToCase);

            Assert.Equal("Hair and fingernails", notification.NotificationSites.First().SiteDescription);
            Assert.Equal("A  note", notification.TreatmentEvents.First().Note);

            var socialContextAddress = notification.SocialContextAddresses.First();
            Assert.Equal("221B Baker St., London", socialContextAddress.Address);
            Assert.Equal("Elementary, Watson", socialContextAddress.Details);
            Assert.Equal("NW1 6XE", socialContextAddress.Postcode);

            var socialContextVenue = notification.SocialContextVenues.First();
            Assert.Equal("32 Windsor Gardens", socialContextVenue.Address);
            Assert.Equal("Peruvian bears", socialContextVenue.Details);
            Assert.Equal("Favourite bear's house", socialContextVenue.Name);
            Assert.Equal("W9 3RG", socialContextVenue.Postcode);

            Assert.Equal("His sister", notification.MBovisDetails.MBovisExposureToKnownCases.First().OtherDetails);
            Assert.Equal("A friend at work", notification.MBovisDetails.MBovisOccupationExposures.First().OtherDetails);
            Assert.Equal("Cows and  badgers", notification.MBovisDetails.MBovisAnimalExposures.First().OtherDetails);
            Assert.Equal("Cheese", notification.MBovisDetails.MBovisUnpasteurisedMilkConsumptions.First().OtherDetails);
        }

        private static Notification CreateBasicNotification() =>
            new Notification
            {
                NotificationDate = new DateTime(2020,1,1),
                TestData = new TestData { ManualTestResults = new List<ManualTestResult>() },
                ClinicalDetails = new ClinicalDetails(),
                SocialContextAddresses = new List<SocialContextAddress>(),
                SocialContextVenues = new List<SocialContextVenue>(),
                TreatmentEvents = new List<TreatmentEvent>(),
                MBovisDetails = new MBovisDetails
                {
                    MBovisOccupationExposures = new List<MBovisOccupationExposure>(),
                    MBovisExposureToKnownCases = new List<MBovisExposureToKnownCase>(),
                    MBovisAnimalExposures = new List<MBovisAnimalExposure>(),
                    MBovisUnpasteurisedMilkConsumptions = new List<MBovisUnpasteurisedMilkConsumption>()
                }
            };
    }
}
