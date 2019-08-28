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
    public class PatientServiceTest
    {
        private IPatientService service;
        private Mock<IPatientRepository> mockRepository;
        private Mock<NtbsContext> mockContext;

        private const int UkId = 1;
        public PatientServiceTest()
        {
            mockRepository = new Mock<IPatientRepository>();
            mockContext = new Mock<NtbsContext>();
            service = new PatientService(mockRepository.Object, mockContext.Object);
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
            var patient = new Patient() { CountryId = country.CountryId };

            // Act
            service.UpdatePatientAsync(patient);

            // Assert
            Assert.Equal(expectedResult, patient.UkBorn);
        }

        [Fact]
        public void UkBorn_IsSetToNullIfCountryNotSet()
        {
            // Arrange
            var patient = new Patient() { UkBorn = true };

            // Act
            service.UpdatePatientAsync(patient);

            // Assert
            Assert.Null(patient.UkBorn);
        }

        [Fact]
        public void NhsNumber_IsSetToNullIfNhsNumberUnknownTrue()
        {
            // Arrange
            var patient = new Patient() { NhsNumber = "1534645612", IsNhsNumberUnknown = true };

            // Act
            service.UpdatePatientAsync(patient);

            // Assert
            Assert.Null(patient.NhsNumber);
        }

        [Fact]
        public void Postcode_IsSetToNullIfPostcodeUnknownTrue()
        {
            // Arrange
            var patient = new Patient() { Postcode = "NW5 1TL", IsPostcodeUnknown = true };

            // Act
            service.UpdatePatientAsync(patient);

            // Assert
            Assert.Null(patient.Postcode);
        }
    }
}