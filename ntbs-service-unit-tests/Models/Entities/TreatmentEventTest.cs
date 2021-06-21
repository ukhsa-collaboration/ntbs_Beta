using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class TreatmentEventTest
    {
        [Fact]
        public void TreatmentEventWithNullEventDateIsInvalid()
        {
            // given
            var treatmentEvent = new TreatmentEvent { EventDate = null };

            // when
            var isValid = ValidateTreatmentEvent(treatmentEvent, out var results);

            // then
            Assert.False(isValid);
            var expectedErrorMessage = string.Format(ValidationMessages.RequiredEnter, "Event Date");
            Assert.Contains(results, vr => vr.ErrorMessage == expectedErrorMessage);
        }

        private static bool ValidateTreatmentEvent(TreatmentEvent treatmentEvent, out List<ValidationResult> results)
        {
            var context = new ValidationContext(treatmentEvent);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(treatmentEvent, context, results, true);
        }
    }
}
