using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;
using ntbs_service.Models.Enums;

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
        public void SocialRiskFactorChecklist_AreSetToFalseIfStatusUnknown()
        {
            // Arrange
            var notification = new Notification();
            var socialRiskFactors = new SocialRiskFactors() { 
                RiskFactorDrugs = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.Unknown },
                RiskFactorHomelessness   = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.Unknown},
                RiskFactorImprisonment = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.Unknown},
                RiskFactorMentalHealth = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.Unknown},
            };

            // Act
            service.UpdateSocialRiskFactorsAsync(notification, socialRiskFactors);

            // Assert
            Assert.False(socialRiskFactors.RiskFactorDrugs.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorDrugs.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorDrugs.IsCurrent);
         
            Assert.False(socialRiskFactors.RiskFactorHomelessness.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorHomelessness.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorHomelessness.IsCurrent);
            
            Assert.False(socialRiskFactors.RiskFactorImprisonment.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorImprisonment.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorImprisonment.IsCurrent);
            
            Assert.False(socialRiskFactors.RiskFactorMentalHealth.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorMentalHealth.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorMentalHealth.IsCurrent);   
        }

        [Fact]
        public void SocialRiskFactorChecklist_AreSetToFalseIfStatusNo()
        {
            // Arrange
            var notification = new Notification();
            var socialRiskFactors = new SocialRiskFactors() { 
                RiskFactorDrugs = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.No },
                RiskFactorHomelessness   = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.No},
                RiskFactorImprisonment = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.No},
                RiskFactorMentalHealth = new RiskFactorBase { IsCurrent = true,  MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.No},
            };

            // Act
            service.UpdateSocialRiskFactorsAsync(notification, socialRiskFactors);

            // Assert
            Assert.False(socialRiskFactors.RiskFactorDrugs.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorDrugs.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorDrugs.IsCurrent);
         
            Assert.False(socialRiskFactors.RiskFactorHomelessness.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorHomelessness.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorHomelessness.IsCurrent);
            
            Assert.False(socialRiskFactors.RiskFactorImprisonment.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorImprisonment.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorImprisonment.IsCurrent);
            
            Assert.False(socialRiskFactors.RiskFactorMentalHealth.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorMentalHealth.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorMentalHealth.IsCurrent);   
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