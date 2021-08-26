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
        private static readonly DateTime earlierDate = new DateTime(2011, 1, 25);
        private static readonly DateTime laterDate = new DateTime(2012, 12, 25);
        private static readonly DateTime earlierDateEarlierTime = new DateTime(2011, 1, 25, 14, 35, 12);
        private static readonly DateTime earlierDateLaterTime = new DateTime(2011, 1, 25, 14, 35, 13);

        private List<TreatmentEvent> sameDayTreatmentEvents = new List<TreatmentEvent>
        {
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.DiagnosisMade},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentStart},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TransferIn},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TransferOut},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentRestart},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentOutcome},
        };

        private List<TreatmentEvent> differentDayTreatmentEvents = new List<TreatmentEvent>
        {
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.DiagnosisMade},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentStart},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferIn},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferOut},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentRestart},
            new TreatmentEvent {EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentOutcome}
        };

        private List<TreatmentEvent> transferEvents = new List<TreatmentEvent>
        {
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferIn},
            new TreatmentEvent {EventDate = earlierDateLaterTime, TreatmentEventType = TreatmentEventType.TransferIn},
            new TreatmentEvent {EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferOut},
            new TreatmentEvent {EventDate = earlierDateEarlierTime, TreatmentEventType = TreatmentEventType.TransferOut},
        };

        [Fact]
        public void OrderForEpisodesCorrectlyOrdersSameDayEvents()
        {
            // Act
            var orderedList = sameDayTreatmentEvents.OrderForEpisodes().ToList();

            // Assert
            Assert.Equal(TreatmentEventType.DiagnosisMade, orderedList[0].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentStart, orderedList[1].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[2].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[3].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentRestart, orderedList[4].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentOutcome, orderedList[5].TreatmentEventType);
            Assert.All(orderedList.GetRange(0, 6), ev => Assert.Equal(ev.EventDate, earlierDate));
        }

        [Fact]
        public void OrderForEpisodesCorrectlyOrdersDifferentDayEvents()
        {
            // Act
            var orderedList = differentDayTreatmentEvents.OrderForEpisodes().ToList();

            // Assert
            Assert.Equal(TreatmentEventType.TreatmentStart, orderedList[0].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentRestart, orderedList[1].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentOutcome, orderedList[2].TreatmentEventType);
            Assert.Equal(TreatmentEventType.DiagnosisMade, orderedList[3].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[4].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[5].TreatmentEventType);
            Assert.All(orderedList.GetRange(0, 3), ev => Assert.Equal(ev.EventDate, earlierDate));
            Assert.All(orderedList.GetRange(3, 3), ev => Assert.Equal(ev.EventDate, laterDate));
        }

        [Fact]
        public void OrderForEpisodesCorrectlyOrdersTransferEvents()
        {
            // Act
            var orderedList = transferEvents.OrderForEpisodes().ToList();
            
            // Assert
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[0].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[1].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[2].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[3].TreatmentEventType);
            Assert.Equal(orderedList[0].EventDate, earlierDateEarlierTime);
            Assert.Equal(orderedList[1].EventDate, earlierDateLaterTime);
            Assert.All(orderedList.GetRange(2, 2), ev => Assert.Equal(ev.EventDate, laterDate));
        }
    }
}
