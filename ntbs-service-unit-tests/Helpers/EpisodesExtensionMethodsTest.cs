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
        [Fact]
        public void OrderForEpisodesCorrectlyOrdersEvents()
        {
            // Arrange
            var earlierDate = new DateTime(2011, 1, 25);
            var laterDate = new DateTime(2012, 12, 25);
            var treatmentEvents = new List<TreatmentEvent>
            {
                new TreatmentEvent { EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferIn },
                new TreatmentEvent { EventDate = laterDate, TreatmentEventType = TreatmentEventType.TransferOut },
                new TreatmentEvent { EventDate = laterDate, TreatmentEventType = TreatmentEventType.DiagnosisMade },
                new TreatmentEvent { EventDate = laterDate, TreatmentEventType = TreatmentEventType.TreatmentStart },
                new TreatmentEvent { EventDate = laterDate, TreatmentEventType = TreatmentEventType.TreatmentOutcome },
                new TreatmentEvent { EventDate = laterDate, TreatmentEventType = TreatmentEventType.TreatmentRestart },

                new TreatmentEvent { EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TransferIn },
                new TreatmentEvent { EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TransferOut },
                new TreatmentEvent { EventDate = earlierDate, TreatmentEventType = TreatmentEventType.DiagnosisMade },
                new TreatmentEvent { EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentStart },
                new TreatmentEvent { EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentRestart },
                new TreatmentEvent { EventDate = earlierDate, TreatmentEventType = TreatmentEventType.TreatmentOutcome },
            };
            Random rng = new Random();
            var randomisedOrderList = treatmentEvents.OrderBy(ev => rng.Next());

            // Act
            var orderedList = randomisedOrderList.OrderForEpisodes().ToList();

            // Assert
            Assert.Equal(TreatmentEventType.DiagnosisMade, orderedList[0].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentStart, orderedList[1].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[2].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[3].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentRestart, orderedList[4].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentOutcome, orderedList[5].TreatmentEventType);
            Assert.All(orderedList.GetRange(0, 6), ev => Assert.Equal(ev.EventDate, earlierDate));

            Assert.Equal(TreatmentEventType.DiagnosisMade, orderedList[6].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentStart, orderedList[7].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferOut, orderedList[8].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TransferIn, orderedList[9].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentRestart, orderedList[10].TreatmentEventType);
            Assert.Equal(TreatmentEventType.TreatmentOutcome, orderedList[11].TreatmentEventType);
            Assert.All(orderedList.GetRange(6, 6), ev => Assert.Equal(ev.EventDate, laterDate));
        }
    }
}
