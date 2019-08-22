using ntbs_service.Models;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_tests.UnitTests.ntbs_service_tests
{
    public class PatientServiceTest
    {
        private IPatientService service;
        public PatientServiceTest()
        {
            service = new PatientService();
        }

        [Theory]
        [InlineData(Countries.UkCode, true)]
        [InlineData(Countries.UnknownCode, null)]
        [InlineData("Other code", false)]
        public void UkBorn_IsSetToCorrectValueDependentOnBirthCountry(string countryCode, bool? expectedResult)
        {
            // Arrange
            var country = new Country() { IsoCode = countryCode };
            var patient = new Patient() { Country = country };

            // Act
            service.UpdateUkBorn(patient);

            // Assert
            Assert.Equal(expectedResult, patient.UkBorn);
        }


        public void UkBorn_IsSetToNullIfCountryNotSet()
        {
            // Arrange
            var patient = new Patient() { UkBorn = true };

            // Act
            service.UpdateUkBorn(patient);

            // Assert
            Assert.Null(patient.UkBorn);
        }
    }
}