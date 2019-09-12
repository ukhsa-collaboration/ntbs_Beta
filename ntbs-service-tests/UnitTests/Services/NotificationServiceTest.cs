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
    }
}