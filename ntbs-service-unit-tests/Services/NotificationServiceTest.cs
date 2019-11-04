using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class NotificationServiceTest
    {
        private readonly INotificationService service;
        private readonly Mock<INotificationRepository> mockRepository;
        private readonly Mock<IUserService> mockUserService;
        private readonly Mock<NtbsContext> mockContext;

        public NotificationServiceTest()
        {
            mockRepository = new Mock<INotificationRepository>();
            mockUserService = new Mock<IUserService>();
            mockContext = new Mock<NtbsContext>();
            service = new NotificationService(mockRepository.Object, mockUserService.Object, mockContext.Object);
        }

        [Fact]
        public void SocialRiskFactorChecklist_AreSetToFalseIfStatusUnknown()
        {
            // Arrange
            var notification = new Notification();
            var socialRiskFactors = new SocialRiskFactors()
            {
                RiskFactorDrugs = new RiskFactorDetails(RiskFactorType.Drugs) { IsCurrent = true, MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.Unknown },
                RiskFactorHomelessness = new RiskFactorDetails(RiskFactorType.Homelessness) { IsCurrent = true, MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.Unknown },
                RiskFactorImprisonment = new RiskFactorDetails(RiskFactorType.Imprisonment) { IsCurrent = true, MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.Unknown },
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
        }

        [Fact]
        public void SocialRiskFactorChecklist_AreSetToFalseIfStatusNo()
        {
            // Arrange
            var notification = new Notification();
            var socialRiskFactors = new SocialRiskFactors()
            {
                RiskFactorDrugs = new RiskFactorDetails(RiskFactorType.Drugs) { IsCurrent = true, MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.No },
                RiskFactorHomelessness = new RiskFactorDetails(RiskFactorType.Homelessness) { IsCurrent = true, MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.No },
                RiskFactorImprisonment = new RiskFactorDetails(RiskFactorType.Imprisonment) { IsCurrent = true, MoreThanFiveYearsAgo = true, InPastFiveYears = true, Status = Status.No },
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
        }

        public static IEnumerable<object[]> UkBornTestCases()
        {
            yield return new object[] { new Country() { CountryId = 1, IsoCode = Countries.UkCode }, true };
            yield return new object[] { new Country() { CountryId = 2, IsoCode = Countries.UnknownCode }, null };
            yield return new object[] { new Country() { CountryId = 3, IsoCode = "Other code" }, false };
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
        public void SubmitNotification_ChangesDateAndStatus()
        {
            // Arrange
            var notification = new Notification();
            var expectedDate = DateTime.UtcNow;

            // Act
            service.SubmitNotification(notification);

            // Assert
            Assert.Equal(NotificationStatus.Notified, notification.NotificationStatus);
            Assert.True(notification.SubmissionDate.HasValue);
            var statusChangeDate = notification.SubmissionDate.Value;
            Assert.Equal(expectedDate.Date, statusChangeDate.Date);
        }

        [Fact]
        public void UpdateImmunosuppressionDetails_LeavesValuesUnchangedWhenStatusIsYes()
        {
            var reference = new ImmunosuppressionDetails
            {
                Status = Status.Yes,
                HasBioTherapy = true,
                HasTransplantation = true,
                HasOther = true,
                OtherDescription = "Test description"
            };
            var input = new ImmunosuppressionDetails
            {
                Status = reference.Status,
                HasBioTherapy = reference.HasBioTherapy,
                HasTransplantation = reference.HasTransplantation,
                HasOther = reference.HasOther,
                OtherDescription = reference.OtherDescription
            };
            var notification = new Notification();

            service.UpdateImmunosuppresionDetailsAsync(notification, input);

            Assert.Equal(reference.Status, input.Status);
            Assert.Equal(reference.HasBioTherapy, input.HasBioTherapy);
            Assert.Equal(reference.HasTransplantation, input.HasTransplantation);
            Assert.Equal(reference.HasOther, input.HasOther);
            Assert.Equal(reference.OtherDescription, input.OtherDescription);
        }

        [Theory]
        [InlineData((int)Status.No)]
        [InlineData((int)Status.Unknown)]
        public async Task UpdateImmunosuppressionDetails_StripsAllButStatusWhenStatusIsNotYes(int status)
        {
            // Hacky workaround to parameterise enum values seems to be required as passing enum directly resulted in console exception:
            // System.IO.FileNotFoundException : Could not load file or assembly 'Microsoft.Extensions.Configuration.UserSecrets...
            var parsedStatus = (Status)status;
            var reference = new ImmunosuppressionDetails
            {
                Status = parsedStatus,
                HasBioTherapy = true,
                HasTransplantation = true,
                HasOther = true,
                OtherDescription = "Test description"
            };
            var input = new ImmunosuppressionDetails
            {
                Status = reference.Status,
                HasBioTherapy = reference.HasBioTherapy,
                HasTransplantation = reference.HasTransplantation,
                HasOther = reference.HasOther,
                OtherDescription = reference.OtherDescription
            };
            var notification = new Notification();

            await service.UpdateImmunosuppresionDetailsAsync(notification, input);

            Assert.Equal(reference.Status, input.Status);
            Assert.NotEqual(reference.HasBioTherapy, input.HasBioTherapy);
            Assert.False(input.HasBioTherapy);
            Assert.NotEqual(reference.HasTransplantation, input.HasTransplantation);
            Assert.False(input.HasTransplantation);
            Assert.NotEqual(reference.HasOther, input.HasOther);
            Assert.False(input.HasOther);
            Assert.NotEqual(reference.OtherDescription, input.OtherDescription);
            Assert.Null(input.OtherDescription);
        }

        [Fact]
        public async Task UpdateImmunosuppressionDetails_StripsOtherDescriptionWhenHasOtherIsFalse()
        {
            var reference = new ImmunosuppressionDetails
            {
                Status = Status.Yes,
                HasOther = false,
                OtherDescription = "Test description"
            };
            var input = new ImmunosuppressionDetails
            {
                Status = reference.Status,
                HasOther = reference.HasOther,
                OtherDescription = reference.OtherDescription
            };
            var notification = new Notification();

            await service.UpdateImmunosuppresionDetailsAsync(notification, input);

            Assert.Equal(reference.Status, input.Status);
            Assert.Equal(reference.HasOther, input.HasOther);
            Assert.NotEqual(reference.OtherDescription, input.OtherDescription);
            Assert.Null(input.OtherDescription);
        }

        [Fact]
        public async Task UpdatePatientFlags_StripsNhsNumberIfNotKnownAsync()
        {
            var reference = new PatientDetails
            {
                NhsNumberNotKnown = true,
                NhsNumber = "12345"
            };
            var input = new PatientDetails
            {
                NhsNumberNotKnown = reference.NhsNumberNotKnown,
                NhsNumber = reference.NhsNumber
            };

            await service.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NhsNumberNotKnown, input.NhsNumberNotKnown);
            Assert.NotEqual(reference.NhsNumber, input.NhsNumber);
            Assert.Null(input.NhsNumber);
        }

        [Fact]
        public async Task UpdatePatientFlags_DoesNotStripNhsNumberIfKnown()
        {
            var reference = new PatientDetails
            {
                NhsNumberNotKnown = false,
                NhsNumber = "12345"
            };
            var input = new PatientDetails
            {
                NhsNumberNotKnown = reference.NhsNumberNotKnown,
                NhsNumber = reference.NhsNumber
            };

            await service.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NhsNumberNotKnown, input.NhsNumberNotKnown);
            Assert.Equal(reference.NhsNumber, input.NhsNumber);
        }

        [Fact]
        public async Task UpdatePatientFlags_StripsPostcodeIfNoFixedAbode()
        {
            var reference = new PatientDetails
            {
                NoFixedAbode = true,
                Postcode = "12345"
            };
            var input = new PatientDetails
            {
                NoFixedAbode = reference.NoFixedAbode,
                Postcode = reference.Postcode
            };

            await service.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NoFixedAbode, input.NoFixedAbode);
            Assert.NotEqual(reference.Postcode, input.Postcode);
            Assert.Null(input.Postcode);
        }

        [Fact]
        public async Task UpdatePatientFlags_DoesNotStripPostcodeIfFixedAbode()
        {
            var reference = new PatientDetails
            {
                NoFixedAbode = false,
                Postcode = "12345"
            };
            var input = new PatientDetails
            {
                NoFixedAbode = reference.NoFixedAbode,
                Postcode = reference.Postcode
            };

            await service.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NoFixedAbode, input.NoFixedAbode);
            Assert.Equal(reference.Postcode, input.Postcode);
        }

        [Fact]
        public async Task UpdatePatientFlags_StripsOccupationFreeTextIfNoFreeTextForOccupation()
        {
            var reference = new PatientDetails
            {
                OccupationId = 1,
                OccupationOther = "12345"
            };
            var input = new PatientDetails
            {
                OccupationId = reference.OccupationId,
                OccupationOther = reference.OccupationOther
            };
            mockContext.Setup(rep => rep.GetOccupationByIdAsync(input.OccupationId.Value))
                .Returns(Task.FromResult(new Occupation { HasFreeTextField = false }));

            await service.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.OccupationId, input.OccupationId);
            Assert.NotEqual(reference.OccupationOther, input.OccupationOther);
            Assert.Null(input.OccupationOther);
        }

        [Fact]
        public async Task UpdatePatientFlags_DoesNotStripOccupationFreeTextIfFreeTextForOccupation()
        {
            var reference = new PatientDetails
            {
                OccupationId = 1,
                OccupationOther = "12345"
            };
            var input = new PatientDetails
            {
                OccupationId = reference.OccupationId,
                OccupationOther = reference.OccupationOther
            };
            mockContext.Setup(rep => rep.GetOccupationByIdAsync(input.OccupationId.Value))
                .Returns(Task.FromResult(new Occupation { HasFreeTextField = true }));

            await service.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.OccupationId, input.OccupationId);
            Assert.Equal(reference.OccupationOther, input.OccupationOther);
        }
    }
}
