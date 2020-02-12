using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public static class EpisodesHelper
    {
        static readonly List<TreatmentOutcomeType> episodeEndingOutcomeTypes = new List<TreatmentOutcomeType>
        {
            TreatmentOutcomeType.Completed,
            TreatmentOutcomeType.Cured,
            TreatmentOutcomeType.Died,
            TreatmentOutcomeType.Lost,
            TreatmentOutcomeType.Failed,
            TreatmentOutcomeType.TreatmentStopped
        };
        
        public static Dictionary<int, List<TreatmentEvent>> GroupTreatmentEventsByEpisode(IEnumerable<TreatmentEvent> treatmentEvents)
        {
            var orderedTreatmentEvents = treatmentEvents.OrderBy(t => t.EventDate);
            var groupedEpisodes = new Dictionary<int, List<TreatmentEvent>>();
            var episodeCount = 1;
            
            foreach (var treatmentEvent in orderedTreatmentEvents)
            {
                if (!groupedEpisodes.ContainsKey(episodeCount))
                {
                    groupedEpisodes.Add(episodeCount, new List<TreatmentEvent>() {treatmentEvent});
                }
                else
                {
                    groupedEpisodes[episodeCount].Add(treatmentEvent);
                }
                if (IsEpisodeEndingTreatmentEvent(treatmentEvent))
                {
                    episodeCount++;
                }
            }

            return groupedEpisodes;
        }

        public static TreatmentEvent GetMostRecentEventInPeriod(IEnumerable<TreatmentEvent> treatmentEvents, DateTime startTime, DateTime endTime)
        {
            return treatmentEvents.Where(t => t.TreatmentEventTypeIsOutcome
                                                && t.EventDate > startTime
                                                && t.EventDate <= endTime)
                .OrderBy(t => t.EventDate)
                .LastOrDefault();
        }

        private static bool IsEpisodeEndingTreatmentEvent(TreatmentEvent treatmentEvent)
        {
            return (treatmentEvent.TreatmentOutcome != null 
                   && episodeEndingOutcomeTypes.Contains(treatmentEvent.TreatmentOutcome.TreatmentOutcomeType))
                   || treatmentEvent.TreatmentEventType == TreatmentEventType.TransferOut;
        }
        
    }
}
