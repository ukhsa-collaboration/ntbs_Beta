using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public static class EpisodesExtensionMethods
    {
        static readonly List<TreatmentOutcomeType> EpisodeEndingOutcomeTypes = new List<TreatmentOutcomeType>
        {
            TreatmentOutcomeType.Completed,
            TreatmentOutcomeType.Cured,
            TreatmentOutcomeType.Died,
            TreatmentOutcomeType.Lost,
            TreatmentOutcomeType.Failed,
            TreatmentOutcomeType.TreatmentStopped
        };

        public static Dictionary<int, List<TreatmentEvent>> GroupByEpisode(
            this IEnumerable<TreatmentEvent> treatmentEvents)
        {
            var groupedEpisodes = new Dictionary<int, List<TreatmentEvent>>();
            var episodeCount = 1;

            foreach (var treatmentEvent in treatmentEvents.OrderForEpisodes())
            {
                if (!groupedEpisodes.ContainsKey(episodeCount))
                {
                    groupedEpisodes.Add(episodeCount, new List<TreatmentEvent> { treatmentEvent });
                }
                else
                {
                    groupedEpisodes[episodeCount].Add(treatmentEvent);
                }

                if (treatmentEvent.IsEpisodeEndingTreatmentEvent())
                {
                    episodeCount++;
                }
            }

            return groupedEpisodes;
        }

        public static TreatmentEvent GetMostRecentTreatmentEvent(this IEnumerable<TreatmentEvent> treatmentEvents)
        {
            return treatmentEvents
                .OrderByDescending(t => t.EventDate)
                .FirstOrDefault();
        }

        public static bool IsEpisodeEndingTreatmentEvent(this TreatmentEvent treatmentEvent)
        {
            return (treatmentEvent.TreatmentOutcome != null
                    && EpisodeEndingOutcomeTypes.Contains(treatmentEvent.TreatmentOutcome.TreatmentOutcomeType))
                   || treatmentEvent.TreatmentEventType == TreatmentEventType.TransferOut;
        }

        public static IEnumerable<TreatmentEvent> OrderForEpisodes(this IEnumerable<TreatmentEvent> treatmentEvents)
        {
            return treatmentEvents
                // ReSharper disable once PossibleInvalidOperationException - we know EventDate is not null
                .OrderBy(t => t.EventDate.Value.Date)
                .ThenBy(treatmentEvent =>
                {
                    // We want the order of treatment events that happened on the same day
                    // to be interpreted deterministically and with "natural" results.
                    switch (treatmentEvent.TreatmentEventType)
                    {
                        case TreatmentEventType.DiagnosisMade:
                            return 1;
                        case TreatmentEventType.TreatmentStart:
                            return 2;
                        case TreatmentEventType.TransferOut:
                            return 3;
                        case TreatmentEventType.TransferIn:
                            return 4;
                        case TreatmentEventType.TreatmentRestart:
                            return 5;
                        case TreatmentEventType.TreatmentOutcome:
                            return 6;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
        }
    }
}
