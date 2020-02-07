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
        
        public static Dictionary<int, List<TreatmentEvent>> CalculateEpisodes(IEnumerable<TreatmentEvent> treatmentEvents)
        {
            var orderedTreatmentEvents = treatmentEvents.OrderBy(t => t.EventDate);
            var groupedEpisodes = new Dictionary<int, List<TreatmentEvent>>();
            var episodeCount = 1;
            groupedEpisodes.Add(episodeCount, new List<TreatmentEvent>());
            foreach (var treatmentEvent in orderedTreatmentEvents)
            {
                groupedEpisodes[episodeCount].Add(treatmentEvent);
                if (IsEpisodeEndingType(treatmentEvent.TreatmentOutcome?.TreatmentOutcomeType))
                {
                    episodeCount++;
                    groupedEpisodes.Add(episodeCount, new List<TreatmentEvent>());
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
