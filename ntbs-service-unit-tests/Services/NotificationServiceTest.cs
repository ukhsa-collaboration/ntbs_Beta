using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class NotificationServiceTest
    {
        private readonly INotificationService _notificationService;
        private readonly Mock<INotificationRepository> _mockNotificationRepository = new Mock<INotificationRepository>();
        private readonly Mock<IReferenceDataRepository> _mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
        private readonly Mock<IUserService> _mockUserService = new Mock<IUserService>();
        private readonly Mock<ISpecimenService> _mockSpecimenService = new Mock<ISpecimenService>();
        private readonly Mock<NtbsContext> _mockContext = new Mock<NtbsContext>();
        private readonly Mock<IItemRepository<TreatmentEvent>> _mockTreatmentEventRepository = new Mock<IItemRepository<TreatmentEvent>>();
        private readonly Mock<IAlertService> _mockAlertService = new Mock<IAlertService>();

        public NotificationServiceTest()
        {
            _mockContext.Setup(context => context.SetValues(It.IsAny<Object>(), It.IsAny<Object>()));

            _notificationService = new NotificationService(
                _mockNotificationRepository.Object,
                _mockReferenceDataRepository.Object,
                _mockUserService.Object,
                _mockTreatmentEventRepository.Object,
                _mockContext.Object,
                _mockSpecimenService.Object, 
                _mockAlertService.Object);
        }

        [Theory]
        [InlineData((int)Status.No)]
        [InlineData((int)Status.Unknown)]
        public async Task SocialRiskFactorChecklist_AreSetToNullIfStatusNoOrUnknown(int status)
        {
            // Arrange
            var parsedStatus = (Status)status;
            var socialRiskFactors = new SocialRiskFactors()
            {
                RiskFactorDrugs =
                    new RiskFactorDetails(RiskFactorType.Drugs)
                    {
                        IsCurrent = true,
                        MoreThanFiveYearsAgo = true,
                        InPastFiveYears = true,
                        Status = parsedStatus
                    },
                RiskFactorHomelessness =
                    new RiskFactorDetails(RiskFactorType.Homelessness)
                    {
                        IsCurrent = true,
                        MoreThanFiveYearsAgo = true,
                        InPastFiveYears = true,
                        Status = parsedStatus
                    },
                RiskFactorImprisonment = 
                    new RiskFactorDetails(RiskFactorType.Imprisonment)
                {
                    IsCurrent = true, 
                    MoreThanFiveYearsAgo = true, 
                    InPastFiveYears = true, 
                    Status = parsedStatus
                },
            };
            var notification = new Notification {SocialRiskFactors = socialRiskFactors};

            // Act
            await _notificationService.UpdateSocialRiskFactorsAsync(notification, socialRiskFactors);

            // Assert
            _mockContext.Verify(context => context.SetValues(notification.SocialRiskFactors, socialRiskFactors));

            Assert.Null(socialRiskFactors.RiskFactorDrugs.InPastFiveYears);
            Assert.Null(socialRiskFactors.RiskFactorDrugs.MoreThanFiveYearsAgo);
            Assert.Null(socialRiskFactors.RiskFactorDrugs.IsCurrent);
            _mockContext.Verify(context =>
                context.SetValues(notification.SocialRiskFactors.RiskFactorDrugs, socialRiskFactors.RiskFactorDrugs));

            Assert.Null(socialRiskFactors.RiskFactorHomelessness.InPastFiveYears);
            Assert.Null(socialRiskFactors.RiskFactorHomelessness.MoreThanFiveYearsAgo);
            Assert.Null(socialRiskFactors.RiskFactorHomelessness.IsCurrent);
            _mockContext.Verify(context => context.SetValues(notification.SocialRiskFactors.RiskFactorHomelessness,
                socialRiskFactors.RiskFactorHomelessness));

            Assert.Null(socialRiskFactors.RiskFactorImprisonment.InPastFiveYears);
            Assert.Null(socialRiskFactors.RiskFactorImprisonment.MoreThanFiveYearsAgo);
            Assert.Null(socialRiskFactors.RiskFactorImprisonment.IsCurrent);
            _mockContext.Verify(context => context.SetValues(notification.SocialRiskFactors.RiskFactorImprisonment,
                socialRiskFactors.RiskFactorImprisonment));

            VerifyUpdateDatabaseCalled();
        }
        
        [Fact]
        public async Task SocialRiskFactorChecklist_AreSetToFalseIfNullAndStatusYes()
        {
            // Arrange
            const Status yes = Status.Yes;
            var socialRiskFactors = new SocialRiskFactors()
            {
                RiskFactorDrugs =
                    new RiskFactorDetails(RiskFactorType.Drugs)
                    {
                        IsCurrent = null,
                        MoreThanFiveYearsAgo = null,
                        InPastFiveYears = null,
                        Status = yes
                    },
                RiskFactorHomelessness =
                    new RiskFactorDetails(RiskFactorType.Homelessness)
                    {
                        IsCurrent = null,
                        MoreThanFiveYearsAgo = null,
                        InPastFiveYears = null,
                        Status = yes
                    },
                RiskFactorImprisonment = 
                    new RiskFactorDetails(RiskFactorType.Imprisonment)
                {
                    IsCurrent = null, 
                    MoreThanFiveYearsAgo = null, 
                    InPastFiveYears = null, 
                    Status = yes
                },
            };
            var notification = new Notification {SocialRiskFactors = socialRiskFactors};

            // Act
            await _notificationService.UpdateSocialRiskFactorsAsync(notification, socialRiskFactors);

            // Assert
            _mockContext.Verify(context => context.SetValues(notification.SocialRiskFactors, socialRiskFactors));

            Assert.False(socialRiskFactors.RiskFactorDrugs.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorDrugs.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorDrugs.IsCurrent);
            _mockContext.Verify(context =>
                context.SetValues(notification.SocialRiskFactors.RiskFactorDrugs, socialRiskFactors.RiskFactorDrugs));

            Assert.False(socialRiskFactors.RiskFactorHomelessness.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorHomelessness.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorHomelessness.IsCurrent);
            _mockContext.Verify(context => context.SetValues(notification.SocialRiskFactors.RiskFactorHomelessness,
                socialRiskFactors.RiskFactorHomelessness));

            Assert.False(socialRiskFactors.RiskFactorImprisonment.InPastFiveYears);
            Assert.False(socialRiskFactors.RiskFactorImprisonment.MoreThanFiveYearsAgo);
            Assert.False(socialRiskFactors.RiskFactorImprisonment.IsCurrent);
            _mockContext.Verify(context => context.SetValues(notification.SocialRiskFactors.RiskFactorImprisonment,
                socialRiskFactors.RiskFactorImprisonment));

            VerifyUpdateDatabaseCalled();
        }

        public static IEnumerable<object[]> UkBornTestCases()
        {
            yield return new object[] {new Country() {CountryId = 1, IsoCode = Countries.UkCode}, true};
            yield return new object[] {new Country() {CountryId = 2, IsoCode = Countries.UnknownCode}, null};
            yield return new object[] {new Country() {CountryId = 3, IsoCode = "Other code"}, false};
        }

        [Theory, MemberData(nameof(UkBornTestCases))]
        public async Task UkBorn_IsSetToCorrectValueDependentOnBirthCountry(Country country, bool? expectedResult)
        {
            // Arrange
            _mockReferenceDataRepository.Setup(rep => rep.GetCountryByIdAsync(country.CountryId))
                .Returns(Task.FromResult(country));
            var notification = new Notification();
            var patient = new PatientDetails() {CountryId = country.CountryId};

            // Act
            await _notificationService.UpdatePatientDetailsAsync(notification, patient);

            // Assert
            Assert.Equal(expectedResult, patient.UkBorn);
            _mockContext.Verify(context => context.SetValues(notification.PatientDetails, patient));
            VerifyUpdateDatabaseCalled();
        }

        [Fact]
        public async Task UkBorn_IsSetToNullIfCountryNotSet()
        {
            // Arrange
            var notification = new Notification();
            var patient = new PatientDetails() {UkBorn = true};

            // Act
            await _notificationService.UpdatePatientDetailsAsync(notification, patient);

            // Assert
            Assert.Null(patient.UkBorn);
            _mockContext.Verify(context => context.SetValues(notification.PatientDetails, patient));
            VerifyUpdateDatabaseCalled();
        }

        [Fact]
        public async Task SubmitNotification_ChangesDateAndStatus()
        {
            // Arrange
            var notification = new Notification();
            var expectedDate = DateTime.UtcNow;

            // Act
            await _notificationService.SubmitNotificationAsync(notification);

            // Assert
            Assert.Equal(NotificationStatus.Notified, notification.NotificationStatus);
            Assert.True(notification.SubmissionDate.HasValue);
            var statusChangeDate = notification.SubmissionDate.Value;
            Assert.Equal(expectedDate.Date, statusChangeDate.Date);
            VerifyUpdateDatabaseCalled();
        }

        [Fact]
        public async Task UpdateImmunosuppressionDetails_LeavesValuesUnchangedWhenStatusIsYes()
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

            await _notificationService.UpdateImmunosuppresionDetailsAsync(notification, input);

            Assert.Equal(reference.Status, input.Status);
            Assert.Equal(reference.HasBioTherapy, input.HasBioTherapy);
            Assert.Equal(reference.HasTransplantation, input.HasTransplantation);
            Assert.Equal(reference.HasOther, input.HasOther);
            Assert.Equal(reference.OtherDescription, input.OtherDescription);
            VerifyUpdateDatabaseCalled();
        }

        [Theory]
        [InlineData((int)Status.No)]
        [InlineData((int)Status.Unknown)]
        public async Task UpdateImmunosuppressionDetails_StripsAllButStatusWhenStatusIsNotYes(int status)
        {
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

            await _notificationService.UpdateImmunosuppresionDetailsAsync(notification, input);

            Assert.Equal(reference.Status, input.Status);
            Assert.NotEqual(reference.HasBioTherapy, input.HasBioTherapy);
            Assert.Null(input.HasBioTherapy);
            Assert.NotEqual(reference.HasTransplantation, input.HasTransplantation);
            Assert.Null(input.HasTransplantation);
            Assert.NotEqual(reference.HasOther, input.HasOther);
            Assert.Null(input.HasOther);
            Assert.NotEqual(reference.OtherDescription, input.OtherDescription);
            Assert.Null(input.OtherDescription);

            _mockContext.Verify(context => context.SetValues(notification.ImmunosuppressionDetails, input));
            VerifyUpdateDatabaseCalled();
        }

        [Fact]
        public async Task UpdateImmunosuppressionDetails_StripsOtherDescriptionWhenHasOtherIsFalse()
        {
            var reference = new ImmunosuppressionDetails
            {
                Status = Status.Yes, HasOther = false, OtherDescription = "Test description"
            };
            var input = new ImmunosuppressionDetails
            {
                Status = reference.Status,
                HasOther = reference.HasOther,
                OtherDescription = reference.OtherDescription
            };
            var notification = new Notification();

            await _notificationService.UpdateImmunosuppresionDetailsAsync(notification, input);

            Assert.Equal(reference.Status, input.Status);
            Assert.Equal(reference.HasOther, input.HasOther);
            Assert.NotEqual(reference.OtherDescription, input.OtherDescription);
            Assert.Null(input.OtherDescription);

            _mockContext.Verify(context => context.SetValues(notification.ImmunosuppressionDetails, input));
            VerifyUpdateDatabaseCalled();
        }

        [Fact]
        public async Task UpdatePatientFlags_StripsNhsNumberIfNotKnownAsync()
        {
            var reference = new PatientDetails {NhsNumberNotKnown = true, NhsNumber = "12345"};
            var input = new PatientDetails
            {
                NhsNumberNotKnown = reference.NhsNumberNotKnown, NhsNumber = reference.NhsNumber
            };

            await _notificationService.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NhsNumberNotKnown, input.NhsNumberNotKnown);
            Assert.NotEqual(reference.NhsNumber, input.NhsNumber);
            Assert.Null(input.NhsNumber);
        }

        [Fact]
        public async Task UpdatePatientFlags_DoesNotStripNhsNumberIfKnown()
        {
            var reference = new PatientDetails {NhsNumberNotKnown = false, NhsNumber = "12345"};
            var input = new PatientDetails
            {
                NhsNumberNotKnown = reference.NhsNumberNotKnown, NhsNumber = reference.NhsNumber
            };

            await _notificationService.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NhsNumberNotKnown, input.NhsNumberNotKnown);
            Assert.Equal(reference.NhsNumber, input.NhsNumber);
        }

        [Fact]
        public async Task UpdatePatientFlags_StripsPostcodeIfNoFixedAbode()
        {
            var reference = new PatientDetails {NoFixedAbode = true, Postcode = "12345"};
            var input = new PatientDetails {NoFixedAbode = reference.NoFixedAbode, Postcode = reference.Postcode};

            await _notificationService.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NoFixedAbode, input.NoFixedAbode);
            Assert.NotEqual(reference.Postcode, input.Postcode);
            Assert.Null(input.Postcode);
        }

        [Fact]
        public async Task UpdatePatientFlags_DoesNotStripPostcodeIfFixedAbode()
        {
            var reference = new PatientDetails {NoFixedAbode = false, Postcode = "12345"};
            var input = new PatientDetails {NoFixedAbode = reference.NoFixedAbode, Postcode = reference.Postcode};

            await _notificationService.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.NoFixedAbode, input.NoFixedAbode);
            Assert.Equal(reference.Postcode, input.Postcode);
        }

        [Fact]
        public async Task UpdatePatientFlags_StripsOccupationFreeTextIfNoFreeTextForOccupation()
        {
            var reference = new PatientDetails {OccupationId = 1, OccupationOther = "12345"};
            var input = new PatientDetails
            {
                OccupationId = reference.OccupationId, OccupationOther = reference.OccupationOther
            };
            _mockReferenceDataRepository.Setup(rep => rep.GetOccupationByIdAsync(input.OccupationId.Value))
                .Returns(Task.FromResult(new Occupation {HasFreeTextField = false}));

            await _notificationService.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.OccupationId, input.OccupationId);
            Assert.NotEqual(reference.OccupationOther, input.OccupationOther);
            Assert.Null(input.OccupationOther);
        }

        [Fact]
        public async Task UpdatePatientFlags_DoesNotStripOccupationFreeTextIfFreeTextForOccupation()
        {
            var reference = new PatientDetails {OccupationId = 1, OccupationOther = "12345"};
            var input = new PatientDetails
            {
                OccupationId = reference.OccupationId, OccupationOther = reference.OccupationOther
            };
            _mockReferenceDataRepository.Setup(rep => rep.GetOccupationByIdAsync(input.OccupationId.Value))
                .Returns(Task.FromResult(new Occupation {HasFreeTextField = true}));

            await _notificationService.UpdatePatientFlagsAsync(input);

            Assert.Equal(reference.OccupationId, input.OccupationId);
            Assert.Equal(reference.OccupationOther, input.OccupationOther);
        }

        private void VerifyUpdateDatabaseCalled()
        {
            _mockNotificationRepository.Verify(mock =>
                mock.SaveChangesAsync(It.IsAny<NotificationAuditType>(), It.IsAny<String>()));
        }
        
        [Fact]
        public async Task UpdateNotificationClustersAsync_DoesNotThrowIfNotificationExists()
        {
            const int existingNotification = 1;
            _mockNotificationRepository
                .Setup(r => r.GetNotificationAsync(existingNotification))
                .Returns(Task.FromResult(new Notification()));
            
            // If action throws, then the test fails - used frameworks do not explicitly expose a DoesNotThrow
            await _notificationService.UpdateNotificationClustersAsync(
                new List<NotificationClusterValue>
                {
                    new NotificationClusterValue {NotificationId = existingNotification, ClusterId = null}
                });
        }

        [Fact]
        public async Task UpdateNotificationClustersAsync_ThrowsIfNotificationDoesNotExist()
        {
            const int notExistingNotification = 1;
            _mockNotificationRepository
                .Setup(r => r.GetNotificationAsync(notExistingNotification))
                .Returns(Task.FromResult<Notification>(null));
            
            await Assert.ThrowsAnyAsync<DataException>(() =>
                _notificationService.UpdateNotificationClustersAsync(
                    new List<NotificationClusterValue>
                    {
                        new NotificationClusterValue {NotificationId = notExistingNotification, ClusterId = null}
                    })
            );
        }

        [Fact]
        public async Task AddNotificationStartEventType_WithTreatmentDate_WhenNotificationSubmittedHasTreatmentDate()
        {
            // Arrange
            var treatmentDate = new DateTime(2015, 1, 1);
            const int notificationId = 1;
            var notification = new Notification
            {
                NotificationId = notificationId,
                ClinicalDetails = new ClinicalDetails
                {
                    TreatmentStartDate = treatmentDate
                }
            };

            // Act
            await _notificationService.SubmitNotificationAsync(notification);

            // Assert
            _mockTreatmentEventRepository.Verify(x => 
                x.AddAsync(It.Is<TreatmentEvent>(e => e.EventDate == treatmentDate)), Times.Once);
        }
        
        [Fact]
        public async Task AddNotificationStartEventType_WithNotificationDate_WhenNotificationSubmittedWithoutTreatmentDate()
        {
            // Arrange
            var notificationDate = new DateTime(2015, 1, 1);
            const int notificationId = 1;
            var notification = new Notification
            {
                NotificationId = notificationId,
                NotificationDate = notificationDate
            };

            // Act
            await _notificationService.SubmitNotificationAsync(notification);

            // Assert
            _mockTreatmentEventRepository.Verify(x => 
                x.AddAsync(It.Is<TreatmentEvent>(e => e.EventDate == notificationDate && 
                                                      e.TreatmentEventType == TreatmentEventType.TreatmentStart))
                , Times.Once);
        }
    }
}
