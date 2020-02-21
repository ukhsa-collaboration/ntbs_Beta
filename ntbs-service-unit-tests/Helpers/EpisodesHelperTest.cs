using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class EpisodeHelperTest
    {
        readonly List<TreatmentEvent> MockTreatmentEvents;
        
        public EpisodeHelperTest()
        {
            MockTreatmentEvents = new List<TreatmentEvent>()
            {
                // Episode 1
                new TreatmentEvent
                {
                    EventDate = new DateTime(2011, 1, 1),
                    TreatmentEventType = TreatmentEventType.TreatmentStart
                },                
                new TreatmentEvent
                {
                    EventDate = new DateTime(2012, 1, 1),
                    TreatmentEventType = TreatmentEventType.TransferOut
                },
                // Episode 2               
                new TreatmentEvent
                {
                    EventDate = new DateTime(2014, 1, 1),
                    TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                    TreatmentOutcome = new TreatmentOutcome
                    {
                        TreatmentOutcomeType = TreatmentOutcomeType.Completed
                    }
                },            
                // Episode 3
                new TreatmentEvent
                {
                    EventDate = new DateTime(2016, 1, 1),
                    TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                    TreatmentOutcome = new TreatmentOutcome
                    {
                        TreatmentOutcomeType = TreatmentOutcomeType.Cured
                    }
                },
                new TreatmentEvent
                {
                    EventDate = new DateTime(2015, 1, 1),
                    TreatmentEventType = TreatmentEventType.TreatmentRestart
                },                
                // Episode 4
                new TreatmentEvent
                {
                    EventDate = new DateTime(2017, 1, 1),
                    TreatmentEventType = TreatmentEventType.TransferIn
                },
                new TreatmentEvent
                {
                    EventDate = new DateTime(2020, 1, 1),
                    TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                    TreatmentOutcome = new TreatmentOutcome
                    {
                        TreatmentOutcomeType = TreatmentOutcomeType.Died
                    }
                },
                new TreatmentEvent
                {
                    EventDate = new DateTime(2019, 1, 1),
                    TreatmentEventType = TreatmentEventType.TreatmentRestart
                },
            };
        }
        
        [Fact]
        public void GroupTreatmentEventsByEpisode_GroupsBasedOnEndingOutcomeType()
        {
            var groupedEpisodes = MockTreatmentEvents.GroupByEpisode();
            Assert.Equal(4, groupedEpisodes.Count());
            Assert.Equal(2, groupedEpisodes[1].Count());
            Assert.Single(groupedEpisodes[2]);
            Assert.Equal(2, groupedEpisodes[3].Count());
            Assert.Equal(3, groupedEpisodes[4].Count());
        }

        [Fact]
        public void GetTreatmentEvent_ReturnsLastTreatmentEventBeforeEndDate()
        {
            var startDate = new DateTime(2016, 12, 12);
            var endDate = new DateTime(2020, 2, 1);
            var treatmentEvent = MockTreatmentEvents.GetMostRecentTreatmentOutcomeInPeriod(startDate, endDate);
            
            Assert.Equal(treatmentEvent.EventDate, new DateTime(2020, 1, 1));
        }
        
        [Fact]
        public void GetTreatmentEvent_ReturnsEmptyList_WhenNoDateInRange()
        {
            var startDate = new DateTime(2020, 1, 10);
            var endDate = new DateTime(2020, 2, 10);
            var treatmentEvent = MockTreatmentEvents.GetMostRecentTreatmentOutcomeInPeriod(startDate, endDate);
            
            Assert.Null(treatmentEvent);
        }
    }
}
