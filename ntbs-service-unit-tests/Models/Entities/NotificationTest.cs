using System;
using System.Collections.Generic;
using System.Globalization;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class NotificationTest
    {
        [Theory]
        [InlineData("LTBR", "165-9", "16590", "165-9")]
        [InlineData("ETS", "165-9", "16590", "16590")]
        [InlineData(null, "165-9", "16590", "165-9")]
        [InlineData(null, null, "16590", "16590")]
        [InlineData(null, null, null, null)]
        public void NotificationCorrectlySetsLegacyId(string legacySource, string ltbrId, string etsId,
            string expectedLegacyId)
        {
            // Arrange
            var notification = new Notification
            {
                LTBRID = ltbrId,
                ETSID = etsId,
                LegacySource = legacySource
            };
            
            // Assert
            Assert.Equal(expectedLegacyId, notification.LegacyId);
        }

        public static TheoryData<DateTime, DateTime, bool> TestData => new TheoryData<DateTime, DateTime, bool>
        {
            { DateTime.Now, new DateTime(2003, 4, 15), false },
            { new DateTime(2003, 4, 15), new DateTime(2003, 4, 15), true },
            { new DateTime(2003, 4, 12), new DateTime(2003, 4, 15), false },
            { new DateTime(2003, 4, 25), new DateTime(2003, 4, 15), true }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void NotificationSetsShouldBeClosedCorrectly(DateTime treatmentOutcomeDate, DateTime treatmentOtherDate, bool expectedValue)
        {
            // Arrange
            var notification = new Notification
            {
                TreatmentEvents = new List<TreatmentEvent> {
                    new TreatmentEvent
                    {
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        TreatmentOutcome = new TreatmentOutcome{ TreatmentOutcomeSubType = TreatmentOutcomeSubType.TbCausedDeath },
                        EventDate = treatmentOutcomeDate
                    },
                    new TreatmentEvent
                    {
                        TreatmentEventType = TreatmentEventType.TreatmentStart,
                        EventDate = treatmentOtherDate
                    }
                }
            };

            // Assert
            Assert.Equal(expectedValue, notification.ShouldBeClosed());
        }
    }
}
