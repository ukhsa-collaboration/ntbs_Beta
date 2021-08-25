using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class EpisodesExtensionMethodsTest
    {
        private static DateTime earlierDate = new DateTime(2011, 1, 25);
        private static DateTime laterDate = new DateTime(2012, 12, 25);
        private static DateTime earlierDateEarlierTime = new DateTime(2011, 1, 25, 14, 35, 12);
        private static DateTime earlierDateLaterTime = new DateTime(2011, 1, 25, 14, 35, 13);

        private List<TreatmentEvent> treatmentEvents = new List<TreatmentEvent>
        {
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.DiagnosisMade},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TreatmentStart},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferIn},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferOut},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TreatmentRestart},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TreatmentOutcome},

            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.DiagnosisMade},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentStart},
            new TreatmentEvent {EventDate = earlierDateEarlierTime, TreatmentEventType = TreatmentEventType.TransferIn},
            new TreatmentEvent {EventDate = earlierDateEarlierTime, TreatmentEventType = TreatmentEventType.TransferOut},
            new TreatmentEvent {EventDate = earlierDateLaterTime, TreatmentEventType = TreatmentEventType.TransferIn},
            new TreatmentEvent {EventDate = earlierDateLaterTime, TreatmentEventType = TreatmentEventType.TransferOut},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentRestart},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentOutcome},
        };

        [Fact]
        public void OrderForEpisodesCorrectlyOrdersEvents()
        {
            // Act
            var orderedList = treatmentEvents.OrderForEpisodes().ToList();

            // Assert
            AssertTreatmentEventOrder(orderedList);
        }

        [Fact]
        public void OrderForEpisodesCorrectlyOrdersEventsWithReversedList()
        {
            // Arrange
            treatmentEvents.Reverse();

            // Act
            var orderedList = treatmentEvents.OrderForEpisodes().ToList();

            // Assert
            AssertTreatmentEventOrder(orderedList);
        }

        private void AssertTreatmentEventOrder(List<TreatmentEvent> orderedList)
        {
            Assert.Equal(TreatmentEventType.DiagnosisMade, orderedList[0].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentStart, orderedList[1].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[2].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[3].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[4].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[5].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentRestart, orderedList[6].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentOutcome, orderedList[7].TreatmentEventType);
            Assert.All(orderedList.GetRange(0, 2), ev => Assert.Equal(ev.EventDate, earlierDate));
            Assert.All(orderedList.GetRange(2, 2), ev => Assert.Equal(ev.EventDate, earlierDateEarlierTime));
            Assert.All(orderedList.GetRange(4, 2), ev => Assert.Equal(ev.EventDate, earlierDateLaterTime));
            Assert.All(orderedList.GetRange(6, 2), ev => Assert.Equal(ev.EventDate, earlierDate));

            Assert.Equal(TreatmentEventType.DiagnosisMade, orderedList[8].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentStart, orderedList[9].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[10].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[11].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentRestart, orderedList[12].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentOutcome, orderedList[13].TreatmentEventType);
            Assert.All(orderedList.GetRange(8, 6), ev => Assert.Equal(ev.EventDate, laterDate));
        }
    }
}
