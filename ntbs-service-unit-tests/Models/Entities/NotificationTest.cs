using System;
using System.Collections.Generic;
using System.Data;
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

        public static TheoryData<DateTime, DateTime?, NotificationStatus, bool> TestData => new TheoryData<DateTime, DateTime?, NotificationStatus, bool>
        {
            { DateTime.Now, new DateTime(2003, 4, 15), NotificationStatus.Notified, false },
            { new DateTime(2003, 4, 15), new DateTime(2003, 4, 15), NotificationStatus.Notified, true },
            { new DateTime(2003, 4, 12), new DateTime(2003, 4, 15), NotificationStatus.Notified, false },
            { new DateTime(2003, 4, 25), new DateTime(2003, 4, 15), NotificationStatus.Notified, true },
            { new DateTime(2003, 4, 25), new DateTime(2003, 4, 15), NotificationStatus.Denotified, false },
            // This case is important to check that the calculation succeeds even when there is a treatment event without a date.
            { new DateTime(2003, 4, 12), null, NotificationStatus.Notified, true }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void NotificationSetsShouldBeClosedCorrectly(DateTime treatmentOutcomeDate, DateTime? treatmentOtherDate, NotificationStatus status, bool expectedValue)
        {
            // Arrange
            var notification = new Notification
            {
                NotificationStatus = status,
                TreatmentEvents = new List<TreatmentEvent> {
                    new TreatmentEvent
                    {
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        TreatmentOutcomeId = 7,
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

        [Fact]
        public void ShouldBeClosedThrowsExceptionWhenOutcomeNotLoaded()
        {
            // Arrange
            var notification = new Notification
            {
                NotificationStatus = NotificationStatus.Notified,
                TreatmentEvents = new List<TreatmentEvent> {
                    new TreatmentEvent
                    {
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        TreatmentOutcomeId = 7,
                        EventDate = new DateTime(2003, 4, 15)
                    }
                }
            };

            // Assert
            Assert.Throws<ApplicationException>(() => notification.ShouldBeClosed());
        }
    }
}
