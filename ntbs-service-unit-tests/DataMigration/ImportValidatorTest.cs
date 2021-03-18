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
    public class ImportValidatorTest
    {
        private readonly IImportValidator _importValidator;
        private readonly Mock<IReferenceDataRepository> _referenceDataRepositoryMock =
            new Mock<IReferenceDataRepository>();

        public ImportValidatorTest()
        {
            var importLogger = new ImportLogger();
            _importValidator = new ImportValidator(importLogger, _referenceDataRepositoryMock.Object);
        }

        [Fact]
        public async Task ImportValidatorValidatesBaseModels()
        {
            // Arrange
            var notification = new Notification
            {
                NotificationDate = new DateTime(2020,1,1),
                TestData = new TestData{ManualTestResults = new List<ManualTestResult>()},
                MDRDetails =
                {
                    RelationshipToCase = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"
                },
                PatientDetails = new PatientDetails {FamilyName = "<>Davis<>"},
                ClinicalDetails = new ClinicalDetails{DiagnosisDate = new DateTime(2000,1,1)},
                TravelDetails = new TravelDetails{HasTravel = Status.Yes, TotalNumberOfCountries = 51},
                ImmunosuppressionDetails = new ImmunosuppressionDetails{HasOther = true, OtherDescription = "<><>"},
                HospitalDetails = new HospitalDetails{Consultant = "<>Fred<>"},
                VisitorDetails = new VisitorDetails{HasVisitor = Status.Yes, StayLengthInMonths1 = 3},
                ContactTracing = new ContactTracing{AdultsIdentified = -1},
                PreviousTbHistory = new PreviousTbHistory{PreviousTbDiagnosisYear = 1899},
                MBovisDetails = new MBovisDetails{MBovisExposureToKnownCases = 
                    new List<MBovisExposureToKnownCase>{new MBovisExposureToKnownCase()}, HasExposureToKnownCases = false},
                DrugResistanceProfile = new DrugResistanceProfile{Species = "M. bovis"}
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, "test-request-V", notification);
            
            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Contains("The field Relationship of the current case to the contact must be a string or array type with a maximum length of '90'.", 
                errorMessages);
            Assert.Contains(String.Format(ValidationMessages.StandardStringFormat, "Family name"), 
                errorMessages);
            Assert.Contains(ValidationMessages.DateValidityRangeStart("Diagnosis date", "01/01/2010"), 
                errorMessages);
            Assert.Contains("The field total number of countries must be between 1 and 50.", 
                errorMessages);
            Assert.Contains(String.Format(ValidationMessages.InvalidCharacter, "Immunosuppression type description"), 
                errorMessages);
            Assert.Contains(String.Format(ValidationMessages.InvalidCharacter, "Consultant"), 
                errorMessages);
            Assert.Contains(ValidationMessages.TravelOrVisitDurationHasCountry,
                errorMessages);
            Assert.Contains(ValidationMessages.ValidYear,
                errorMessages);
            Assert.Contains(String.Format(ValidationMessages.RequiredSelect, "Exposure setting"),
                errorMessages);
        }

        [Fact]
        public async Task ImportValidatorValidatesCollectionModels()
        {
            // Arrange
            var notification = new Notification
            {
                NotificationDate = new DateTime(2020,1,1),
                TestData = new TestData{ManualTestResults = new List<ManualTestResult>()},
                SocialContextAddresses = new List<SocialContextAddress>{new SocialContextAddress()},
                SocialContextVenues = new List<SocialContextVenue>{new SocialContextVenue()},
                TreatmentEvents = new List<TreatmentEvent>{new TreatmentEvent{EventDate = new DateTime(1899, 3, 3)}},
                MBovisDetails = new MBovisDetails
                {
                    HasOccupationExposure = true, MBovisOccupationExposures = new List<MBovisOccupationExposure>(),
                    HasExposureToKnownCases = true, MBovisExposureToKnownCases = new List<MBovisExposureToKnownCase>(),
                    HasAnimalExposure = true, MBovisAnimalExposures = new List<MBovisAnimalExposure>(),
                    HasUnpasteurisedMilkConsumption = true, MBovisUnpasteurisedMilkConsumptions = new List<MBovisUnpasteurisedMilkConsumption>()
                }
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, "test-request-V", notification);
            
            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            
            Assert.Contains("Event Date must not be before 01/01/2010", 
                errorMessages);
            Assert.Contains(ValidationMessages.SupplyOneOfTheAddressFields,
                errorMessages);
            Assert.Contains(ValidationMessages.SupplyOneOfTheVenueFields,
                errorMessages);
            Assert.Contains(ValidationMessages.HasNoExposureRecords,
                errorMessages);
            Assert.Contains(ValidationMessages.HasNoAnimalExposureRecords,
                errorMessages);
            Assert.Contains(ValidationMessages.HasNoOccupationExposureRecords,
                errorMessages);
            Assert.Contains(ValidationMessages.HasNoUnpasteurisedMilkConsumptionRecords,
                errorMessages);
        }

        [Fact]
        public async Task ImportValidatorCleansContactTracingValues()
        {
            // Arrange
            var notification = new Notification
            {
                NotificationDate = new DateTime(2020,1,1),
                TestData = new TestData{ManualTestResults = new List<ManualTestResult>()},
                ContactTracing = new ContactTracing{AdultsIdentified = 1, AdultsScreened = 3} // these values would fail validation
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, "test-request-V", notification);
            
            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            
            Assert.Empty(errorMessages);
        }

        [Fact]
        public async Task ImportValidatorCleansTestDataValues()
        {
            // Arrange
            var notification = new Notification
            {
                NotificationDate = new DateTime(2020,1,1),
                TestData = new TestData 
                { 
                    ManualTestResults = new List<ManualTestResult> 
                    {
                        new ManualTestResult { Result = Result.Positive, TestDate = null } // these values would fail validation
                    }
                }
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, "test-request-V", notification);
            
            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            
            Assert.Empty(errorMessages);
        }
    }
}
