using System;
using System.Collections.Generic;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class EpisodeHelperTest
    {
        // Arrange
        readonly List<TreatmentEvent> _testTreatmentEvents = new List<TreatmentEvent>
        {
            // Episode 1
            new TreatmentEvent
            {
                EventDate = new DateTime(2011, 1, 1), TreatmentEventType = TreatmentEventType.TreatmentStart
            },
            new TreatmentEvent
            {
                EventDate = new DateTime(2012, 1, 1), TreatmentEventType = TreatmentEventType.TransferOut
            },
            // Episode 2               
            new TreatmentEvent
            {
                EventDate = new DateTime(2014, 1, 1),
                TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                TreatmentOutcome = new TreatmentOutcome {TreatmentOutcomeType = TreatmentOutcomeType.Completed}
            },
            // Episode 3
            new TreatmentEvent
            {
                EventDate = new DateTime(2016, 1, 1),
                TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                TreatmentOutcome = new TreatmentOutcome {TreatmentOutcomeType = TreatmentOutcomeType.Cured}
            },
            new TreatmentEvent
            {
                EventDate = new DateTime(2015, 1, 1), TreatmentEventType = TreatmentEventType.TreatmentRestart
            },
            // Episode 4
            new TreatmentEvent
            {
                EventDate = new DateTime(2017, 1, 1), TreatmentEventType = TreatmentEventType.TransferIn
            },
            new TreatmentEvent
            {
                EventDate = new DateTime(2020, 1, 1),
                TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                TreatmentOutcome = new TreatmentOutcome {TreatmentOutcomeType = TreatmentOutcomeType.Died}
            },
            new TreatmentEvent
            {
                EventDate = new DateTime(2019, 1, 1), TreatmentEventType = TreatmentEventType.TreatmentRestart
            },
        };

        [Fact]
        public void GroupTreatmentEventsByEpisode_GroupsBasedOnEndingOutcomeType()
        {
            // Act
            var groupedEpisodes = _testTreatmentEvents.GroupByEpisode();

            // Assert
            Assert.Equal(4, groupedEpisodes.Count);
            Assert.Equal(2, groupedEpisodes[1].Count);
            Assert.Single(groupedEpisodes[2]);
            Assert.Equal(2, groupedEpisodes[3].Count);
            Assert.Equal(3, groupedEpisodes[4].Count);
        }

        [Fact]
        public void GetMostRecentTreatmentEvent_ReturnsLastTreatmentEvent()
        {
            // Act
            var treatmentEvent = _testTreatmentEvents.GetMostRecentTreatmentEvent();

            // Assert
            Assert.Equal(treatmentEvent.EventDate, new DateTime(2020, 1, 1));
        }

        [Fact]
        public void CorrectlySortThroughEventsOnTheSameDay()
        {
            // Arrange
            var treatmentEvents = new List<TreatmentEvent>
            {
                new TreatmentEvent
                {
                    EventDate = new DateTime(2020, 04, 16), TreatmentEventType = TreatmentEventType.TreatmentStart,
                },
                new TreatmentEvent
                {
                    // The treatment events tend to be with time and not just date!
                    EventDate = new DateTime(2020, 07, 1, 10, 43, 10),
                    TreatmentEventType = TreatmentEventType.TransferOut,
                },
                new TreatmentEvent
                {
                    EventDate = new DateTime(2020, 07, 1, 10, 43, 10),
                    TreatmentEventType = TreatmentEventType.TransferIn,
                },
                new TreatmentEvent
                {
                    EventDate = new DateTime(2020, 07, 01),
                    TreatmentEventType = TreatmentEventType.TreatmentRestart,
                },
                new TreatmentEvent
                {
                    EventDate = new DateTime(2020, 07, 03),
                    TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                    TreatmentOutcome = new TreatmentOutcome
                    {
                        TreatmentOutcomeType = TreatmentOutcomeType.Completed,
                        TreatmentOutcomeSubType = TreatmentOutcomeSubType.Other
                    }
                }
            };

            // Act
            var episodes = treatmentEvents.GroupByEpisode();

            // Assert
            Assert.Collection(episodes,
                ep => Assert.Collection(ep.Value,
                    ev => Assert.Equal(TreatmentEventType.TreatmentStart, ev.TreatmentEventType),
                    ev => Assert.Equal(TreatmentEventType.TransferOut, ev.TreatmentEventType)
                ),
                ep => Assert.Collection(ep.Value,
                    ev => Assert.Equal(TreatmentEventType.TransferIn, ev.TreatmentEventType),
                    ev => Assert.Equal(TreatmentEventType.TreatmentRestart, ev.TreatmentEventType),
                    ev => Assert.Equal(TreatmentEventType.TreatmentOutcome, ev.TreatmentEventType)
                )
            );
        }
    }
}
