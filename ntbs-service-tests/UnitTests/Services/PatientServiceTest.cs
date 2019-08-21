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
        [InlineData((int)CountryCode.UK, true)]
        [InlineData((int)CountryCode.UNKNOWN, null)]
        [InlineData(null, null)]
        [InlineData(101, false)]
        public void UkBorn_IsSetToCorrectValueDependentOnBirthCountry(int? countryId, bool? expectedResult)
        {
            // Arrange
            var patient = new Patient() { CountryId = countryId };

            // Act
            service.UpdateUkBorn(patient);

            // Assert
            Assert.Equal(expectedResult, patient.UkBorn);
        }
    }
}