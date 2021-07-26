using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public partial class ImportValidatorTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_AdultsIdentified(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                AdultsIdentified = valueEntered,
                AdultsScreened = 3,
                AdultsActiveTB = 3,
                AdultsLatentTB = 3,
                AdultsStartedTreatment = 3,
                AdultsFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                AdultsIdentified = valueEntered,
                AdultsScreened = null,
                AdultsActiveTB = null,
                AdultsLatentTB = null,
                AdultsStartedTreatment = null,
                AdultsFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            // Validation errors
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingAdultsScreened,
                ValidationMessages.ContactTracingAdultsActiveTB,
                ValidationMessages.ContactTracingAdultsLatentTB,
                ValidationMessages.ContactTracingAdultsStartedTreatment,
                ValidationMessages.ContactTracingAdultsFinishedTreatment
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_ChildrenIdentified(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                ChildrenIdentified = valueEntered,
                ChildrenScreened = 3,
                ChildrenActiveTB = 3,
                ChildrenLatentTB = 3,
                ChildrenStartedTreatment = 3,
                ChildrenFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                ChildrenIdentified = valueEntered,
                ChildrenScreened = null,
                ChildrenActiveTB = null,
                ChildrenLatentTB = null,
                ChildrenStartedTreatment = null,
                ChildrenFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            // Validation errors
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingChildrenScreened,
                ValidationMessages.ContactTracingChildrenActiveTB,
                ValidationMessages.ContactTracingChildrenLatentTB,
                ValidationMessages.ContactTracingChildrenStartedTreatment,
                ValidationMessages.ContactTracingChildrenFinishedTreatment
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_AdultsScreened(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = valueEntered,
                AdultsActiveTB = 3,
                AdultsLatentTB = 3,
                AdultsStartedTreatment = 3,
                AdultsFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = valueEntered,
                AdultsActiveTB = null,
                AdultsLatentTB = null,
                AdultsStartedTreatment = null,
                AdultsFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            // Validation errors
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingAdultsActiveTB,
                ValidationMessages.ContactTracingAdultsLatentTB,
                ValidationMessages.ContactTracingAdultsStartedTreatment,
                ValidationMessages.ContactTracingAdultsFinishedTreatment
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_ChildrenScreened(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = valueEntered,
                ChildrenActiveTB = 3,
                ChildrenLatentTB = 3,
                ChildrenStartedTreatment = 3,
                ChildrenFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = valueEntered,
                ChildrenActiveTB = null,
                ChildrenLatentTB = null,
                ChildrenStartedTreatment = null,
                ChildrenFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            // Validation errors
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingChildrenActiveTB,
                ValidationMessages.ContactTracingChildrenLatentTB,
                ValidationMessages.ContactTracingChildrenStartedTreatment,
                ValidationMessages.ContactTracingChildrenFinishedTreatment
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_AdultsLatentTB(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = 3,
                AdultsActiveTB = 1,
                AdultsLatentTB = valueEntered,
                AdultsStartedTreatment = 3,
                AdultsFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = 3,
                AdultsActiveTB = 1,
                AdultsLatentTB = valueEntered,
                AdultsStartedTreatment = null,
                AdultsFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingAdultsStartedTreatment,
                ValidationMessages.ContactTracingAdultsFinishedTreatment
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_ChildrenLatentTB(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = 3,
                ChildrenActiveTB = 1,
                ChildrenLatentTB = valueEntered,
                ChildrenStartedTreatment = 3,
                ChildrenFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = 3,
                ChildrenActiveTB = 1,
                ChildrenLatentTB = valueEntered,
                ChildrenStartedTreatment = null,
                ChildrenFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingChildrenStartedTreatment,
                ValidationMessages.ContactTracingChildrenFinishedTreatment
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_AdultsStartedTreatment(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = 3,
                AdultsActiveTB = 1,
                AdultsLatentTB = 1,
                AdultsStartedTreatment = valueEntered,
                AdultsFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = 3,
                AdultsActiveTB = 1,
                AdultsLatentTB = 1,
                AdultsStartedTreatment = valueEntered,
                AdultsFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[] { ValidationMessages.ContactTracingAdultsFinishedTreatment });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async Task ImportValidatorCleansContactTracingValuesBasedOn_ChildrenStartedTreatment(int? valueEntered)
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = 3,
                ChildrenActiveTB = 1,
                ChildrenLatentTB = 1,
                ChildrenStartedTreatment = valueEntered,
                ChildrenFinishedTreatment = 3,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = 3,
                ChildrenActiveTB = 1,
                ChildrenLatentTB = 1,
                ChildrenStartedTreatment = valueEntered,
                ChildrenFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[] { ValidationMessages.ContactTracingChildrenFinishedTreatment });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Fact]
        public async Task ImportValidatorCleansContactTracingValues_WhenTooManyOfEachAdultsCaseType()
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = 3,
                AdultsActiveTB = 2,
                AdultsLatentTB = 2,
                AdultsStartedTreatment = 1,
                AdultsFinishedTreatment = 1,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                AdultsIdentified = 3,
                AdultsScreened = 3,
                AdultsActiveTB = null,
                AdultsLatentTB = null,
                AdultsStartedTreatment = null,
                AdultsFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            // Validation errors
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingAdultsActiveTB,
                ValidationMessages.ContactTracingAdultsLatentTB,
                ValidationMessages.ContactTracingAdultsStartedTreatment,
                ValidationMessages.ContactTracingAdultsFinishedTreatment,
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Fact]
        public async Task ImportValidatorCleansContactTracingValues_WhenTooManyOfEachChildrenCaseType()
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = 3,
                ChildrenActiveTB = 2,
                ChildrenLatentTB = 2,
                ChildrenStartedTreatment = 1,
                ChildrenFinishedTreatment = 1,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            var expectedContactTracing = new ContactTracing
            {
                ChildrenIdentified = 3,
                ChildrenScreened = 3,
                ChildrenActiveTB = null,
                ChildrenLatentTB = null,
                ChildrenStartedTreatment = null,
                ChildrenFinishedTreatment = null,
            };

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, new[]
            {
                ValidationMessages.ContactTracingChildrenActiveTB,
                ValidationMessages.ContactTracingChildrenLatentTB,
                ValidationMessages.ContactTracingChildrenStartedTreatment,
                ValidationMessages.ContactTracingChildrenFinishedTreatment,
            });

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        [Fact]
        public async Task ImportValidator_PreservesValidContactTracingValues()
        {
            // Arrange
            var actualNotification = CreateBasicNotification();
            actualNotification.ContactTracing = new ContactTracing
            {
                AdultsIdentified = 5,
                AdultsScreened = 5,
                AdultsActiveTB = 0,
                AdultsLatentTB = 5,
                AdultsStartedTreatment = 5,
                AdultsFinishedTreatment = 5,

                ChildrenIdentified = 5,
                ChildrenScreened = 4,
                ChildrenActiveTB = 1,
                ChildrenLatentTB = 3,
                ChildrenStartedTreatment = 2,
                ChildrenFinishedTreatment = 1,
            };
            var oldContactTracing = CloneContactTracing(actualNotification.ContactTracing);
            var expectedContactTracing = CloneContactTracing(actualNotification.ContactTracing);

            // Act
            var validationResults = await _importValidator.CleanAndValidateNotification(null, RunId, actualNotification);

            // Assert
            var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
            Assert.Empty(errorMessages);

            AssertWarningsGiven(actualNotification, Enumerable.Empty<string>());

            AssertContractTracingObjectsMatch(actualNotification.ContactTracing, expectedContactTracing,
                oldContactTracing, actualNotification.ClinicalDetails.Notes);
        }

        private void AssertWarningsGiven(Notification notification, IEnumerable<string> warnings)
        {
            foreach (var warning in warnings)
            {
                _importLoggerMock.Verify(s => s.LogNotificationWarning(null, RunId, notification.LegacyId, warning));
            }
            _importLoggerMock.VerifyNoOtherCalls();
        }

        private static void AssertContractTracingObjectsMatch(ContactTracing actual, ContactTracing expected,
            ContactTracing old, string actualClinicalNotes)
        {
            actualClinicalNotes = actualClinicalNotes ?? "";

            var propertyNames = new[]
            {
                nameof(ContactTracing.AdultsIdentified), nameof(ContactTracing.ChildrenIdentified),
                nameof(ContactTracing.AdultsScreened), nameof(ContactTracing.ChildrenScreened),
                nameof(ContactTracing.AdultsActiveTB), nameof(ContactTracing.ChildrenActiveTB),
                nameof(ContactTracing.AdultsLatentTB), nameof(ContactTracing.ChildrenLatentTB),
                nameof(ContactTracing.AdultsStartedTreatment), nameof(ContactTracing.ChildrenStartedTreatment),
                nameof(ContactTracing.AdultsFinishedTreatment), nameof(ContactTracing.ChildrenFinishedTreatment)
            };

            foreach (var propertyName in propertyNames)
            {
                AssertContactTracingPropertyMatches(actual, expected, old, actualClinicalNotes, propertyName);
            }
        }

        private static void AssertContactTracingPropertyMatches(ContactTracing actualContactTracing,
            ContactTracing expectedContactTracing, ContactTracing oldContactTracing, string actualClinicalNotes,
            string propertyName)
        {
            var property = typeof(ContactTracing).GetProperty(propertyName);

            var actualValue = (int?) property?.GetValue(actualContactTracing);
            var expectedValue = (int?) property?.GetValue(expectedContactTracing);
            var oldValue = (int?) property?.GetValue(oldContactTracing);

            Assert.Equal(expectedValue, actualValue);
            if (oldValue == expectedValue)
            {
                Assert.DoesNotContain(propertyName, actualClinicalNotes);
            }
            else
            {
                Assert.Contains($"{propertyName}: {oldValue}", actualClinicalNotes);
            }
        }

        private static ContactTracing CloneContactTracing(ContactTracing original) =>
            new ContactTracing
            {
                AdultsIdentified = original.AdultsIdentified,
                AdultsScreened = original.AdultsScreened,
                AdultsActiveTB = original.AdultsActiveTB,
                AdultsLatentTB = original.AdultsLatentTB,
                AdultsStartedTreatment = original.AdultsStartedTreatment,
                AdultsFinishedTreatment = original.AdultsFinishedTreatment,
                ChildrenIdentified = original.ChildrenIdentified,
                ChildrenScreened = original.ChildrenScreened,
                ChildrenActiveTB = original.ChildrenActiveTB,
                ChildrenLatentTB = original.ChildrenLatentTB,
                ChildrenStartedTreatment = original.ChildrenStartedTreatment,
                ChildrenFinishedTreatment = original.ChildrenFinishedTreatment
            };
    }
}
