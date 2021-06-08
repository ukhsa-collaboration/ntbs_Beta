using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public static class EpisodesExtensionMethods
    {
        private static readonly List<TreatmentOutcomeType> EpisodeEndingOutcomeTypes = new List<TreatmentOutcomeType>
        {
            TreatmentOutcomeType.Completed,
            TreatmentOutcomeType.Cured,
            TreatmentOutcomeType.Died,
            TreatmentOutcomeType.Lost,
            TreatmentOutcomeType.Failed,
            TreatmentOutcomeType.TreatmentStopped
        };

        public static List<TreatmentPeriod> GroupEpisodesIntoPeriods(this IEnumerable<TreatmentEvent> treatmentEvents)
        {
            var treatmentPeriods = new List<TreatmentPeriod>();
            var periodNumber = 1;

            // The treatment period to append to; if it is null then a new period should be created
            TreatmentPeriod currentTreatmentPeriod = null;

            foreach (var treatmentEvent in treatmentEvents.OrderForEpisodes())
            {
                // If at the start of a new treatment period, make it and add the event
                if (currentTreatmentPeriod == null)
                {
                    // If a transfer out event, make a special period just for this event
                    if (treatmentEvent.TreatmentEventType == TreatmentEventType.TransferOut)
                    {
                        currentTreatmentPeriod = TreatmentPeriod.CreateTransferPeriod(treatmentEvent);
                    }
                    else
                    {
                        currentTreatmentPeriod = TreatmentPeriod.CreateTreatmentPeriod(periodNumber, treatmentEvent);
                        periodNumber++;
                    }

                    treatmentPeriods.Add(currentTreatmentPeriod);
                }
                // Otherwise append this event to the existing period
                else
                {
                    currentTreatmentPeriod.TreatmentEvents.Add(treatmentEvent);
                }

                if (treatmentEvent.IsEpisodeEndingTreatmentEvent())
                {
                    currentTreatmentPeriod = null;
                }
            }

            return treatmentPeriods;
        }

        public static TreatmentEvent GetMostRecentTreatmentEvent(this IEnumerable<TreatmentEvent> treatmentEvents)
        {
            return treatmentEvents.OrderForEpisodes().LastOrDefault();
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
                    // to be interpreted deterministically and with "natural" results. More info at:
                    // https://airelogic-nis.atlassian.net/wiki/spaces/R2/pages/599687169/Outcomes+logic
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
