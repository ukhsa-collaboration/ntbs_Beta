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
                if (IsEpisodeEndingType(treatmentEvent.TreatmentOutcome?.TreatmentOutcomeType) 
                    || (treatmentEvent.TreatmentEventType == TreatmentEventType.TransferOut ))
                {
                    episodeCount++;
                }
            }

            return groupedEpisodes;
        }

        private static bool IsEpisodeEndingType(TreatmentOutcomeType? outcomeType)
        {
            return outcomeType != null && episodeEndingOutcomeTypes.Contains((TreatmentOutcomeType) outcomeType);
        }
        
    }
}
