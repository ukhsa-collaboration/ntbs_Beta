using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_tests.UnitTests.ntbs_service_tests
{
    public class NotificationServiceTest
    {
        private INotificationService service;
        private Mock<INotificationRepository> mockRepository;
        private Mock<NtbsContext> mockContext;

        private const int UkId = 1;
        public NotificationServiceTest()
        {
            mockRepository = new Mock<INotificationRepository>();
            mockContext = new Mock<NtbsContext>();
            service = new NotificationService(mockRepository.Object, mockContext.Object);
        }

        [Fact]
        public void TreatmentStartDate_IsSetToNullIfDidNotStartTreatmentTrue()
        {
            // Arrange
            var notification = new Notification();
            var timeline = new ClinicalDetails() { TreatmentStartDate = DateTime.Now, DidNotStartTreatment = true };

            // Act
            service.UpdateTimelineAsync(notification, timeline);

            // Assert
            Assert.Null(timeline.TreatmentStartDate);
        }

        [Fact]
        public void DeathDate_IsSetToNullIfPostMortemFalse()
        {
            // Arrange
            var notification = new Notification();
            var timeline = new ClinicalDetails() { DeathDate = DateTime.Now, IsPostMortem = false };

            // Act
            service.UpdateTimelineAsync(notification, timeline);

            // Assert
            Assert.Null(timeline.DeathDate);
        }

        [Fact]
        public void BCGVaccinationYear_IsSetToNullIfVaccinationStateNo()
        {
            // Arrange
            var notification = new Notification();
            var timeline = new ClinicalDetails() { BCGVaccinationState = State.No, BCGVaccinationYear = 2000 };

            // Act
            service.UpdateTimelineAsync(notification, timeline);

            // Assert
            Assert.Null(timeline.BCGVaccinationYear);
        }

        [Fact]
        public void BCGVaccinationYear_IsSetToNullIfVaccinationStateUnknown()
        {
            // Arrange
            var notification = new Notification();
            var timeline = new ClinicalDetails() { BCGVaccinationState = State.Unknown, BCGVaccinationYear = 2000 };

            // Act
            service.UpdateTimelineAsync(notification, timeline);

            // Assert
            Assert.Null(timeline.BCGVaccinationYear);
        }

        public static IEnumerable<object[]> UkBornTestCases()
        {
            yield return new object[] { new Country() { CountryId = 1, IsoCode = Countries.UkCode}, true};
            yield return new object[] { new Country() { CountryId = 2, IsoCode = Countries.UnknownCode}, null};
            yield return new object[] { new Country() { CountryId = 3, IsoCode = "Other code"}, false};
        }

        [Theory, MemberData(nameof(UkBornTestCases))]
        public void UkBorn_IsSetToCorrectValueDependentOnBirthCountry(Country country, bool? expectedResult)
        {
            // Arrange
            mockContext.Setup(rep => rep.GetCountryByIdAsync(country.CountryId))
                                 .Returns(Task.FromResult(country));
            var notification = new Notification();
            var patient = new PatientDetails() { CountryId = country.CountryId };

            // Act
            service.UpdatePatientAsync(notification, patient);

            // Assert
            Assert.Equal(expectedResult, patient.UkBorn);
        }

        [Fact]
        public void UkBorn_IsSetToNullIfCountryNotSet()
        {
            // Arrange
            var notification = new Notification();
            var patient = new PatientDetails() { UkBorn = true };

            // Act
            service.UpdatePatientAsync(notification, patient);

            // Assert
            Assert.Null(patient.UkBorn);
        }

        [Fact]
        public void NhsNumber_IsSetToNullIfNhsNumberUnknownTrue()
        {
            // Arrange
            var notification = new Notification();
            var patient = new PatientDetails() { NhsNumber = "1534645612", NhsNumberNotKnown = true };

            // Act
            service.UpdatePatientAsync(notification, patient);

            // Assert
            Assert.Null(patient.NhsNumber);
        }

        [Fact]
        public void Postcode_IsSetToNullIfPostcodeUnknownTrue()
        {
            // Arrange
            var notification = new Notification();
            var patient = new PatientDetails() { Postcode = "NW5 1TL", NoFixedAbode = true };

            // Act
            service.UpdatePatientAsync(notification, patient);

            // Assert
            Assert.Null(patient.Postcode);
        }
    }
}