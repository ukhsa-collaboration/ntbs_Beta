using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    /**
     * The concept of outcomes at 12/24/36 months is important for the notification reporting.
     * The conversion of the NTBS event-based system to those values is non-trivial and this class holds the majority
     * of this knowledge.
     * See https://airelogic-nis.atlassian.net/wiki/spaces/R2/pages/599687169/Outcomes+logic for more context on it.
     */
    public static class TreatmentOutcomesHelper
    {
        public static TreatmentOutcome GetTreatmentOutcomeAtXYears(Notification notification, int yearsAfterTreatmentStartDate)
        {
            // If a treatment outcome is missing at X years then one must not already exist
            if (IsTreatmentOutcomeMissingAtXYears(notification, yearsAfterTreatmentStartDate))
            {
                return null;
            }
            
            // If a treatment outcome is not missing that is because one either exists as the last event of the 1 year period
            // or one is not needed and so is null
            return GetOrderedTreatmentEventsInWindowXtoXMinus1Years(notification, yearsAfterTreatmentStartDate)
                ?.LastOrDefault(x => x.TreatmentOutcome != null)
                ?.TreatmentOutcome;
        }

        public static bool IsTreatmentOutcomeMissingAtXYears(Notification notification, int yearsAfterTreatmentStartDate)
        {
            for (var i = yearsAfterTreatmentStartDate; i >= 1; i--)
            {
                var lastTreatmentEventsBetweenIAndIMinusOneYears = GetOrderedTreatmentEventsInWindowXtoXMinus1Years(notification, i)?.LastOrDefault();
                
                // Check if any events have happened in this year window, look back a year if none exist
                if (lastTreatmentEventsBetweenIAndIMinusOneYears == null)
                {
                    continue;
                }
                
                // If the last event was not a treatment outcome event a treatment outcome event is missing
                if (!lastTreatmentEventsBetweenIAndIMinusOneYears.TreatmentEventTypeIsOutcome)
                {
                    return true;
                }
                
                // If a previous year has a treatment outcome of not evaluated this is not an ending treatment outcome
                // so a new treatment outcome will be needed for this 12 month period
                if (i < yearsAfterTreatmentStartDate &&
                    lastTreatmentEventsBetweenIAndIMinusOneYears.TreatmentOutcome?.TreatmentOutcomeSubType ==
                    TreatmentOutcomeSubType.StillOnTreatment)
                {
                    return true;
                }
                
                // If a treatment outcome event exists then a new one is not needed
                if (lastTreatmentEventsBetweenIAndIMinusOneYears.TreatmentEventTypeIsOutcome)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsTreatmentOutcomeExpectedAtXYears(Notification notification, int yearsAfterTreatmentStartDate)
        {
            var startingDate = notification.ClinicalDetails.StartingDate;
            if (startingDate == null || DateTime.Now < startingDate.Value.AddYears(yearsAfterTreatmentStartDate))
            {
                return false;
            }
            
            if (GetOrderedTreatmentEventsInWindowXtoXMinus1Years(notification, yearsAfterTreatmentStartDate)
                    ?.LastOrDefault(x => x.TreatmentOutcome != null) != null)
            {
                return true;
            }
            return IsTreatmentOutcomeMissingAtXYears(notification, yearsAfterTreatmentStartDate);
        }
        
    private static IEnumerable<TreatmentEvent> GetOrderedTreatmentEventsInWindowXtoXMinus1Years(
        Notification notification,
        int numberOfYears)
    {
        return notification.TreatmentEvents?.Where(t =>
            {
                var startDate = notification.ClinicalDetails.StartingDate;
                if (numberOfYears == 1)
                {
                    // For outcome at 12 months we don't want to set a lower boundary, so that pre-notification date
                    // events can be taken into account (i.e. death events for post mortem cases)
                    return t.EventDate < startDate?.AddYears(numberOfYears);    
                }
                return startDate?.AddYears(numberOfYears - 1) <= t.EventDate 
                       && t.EventDate < startDate?.AddYears(numberOfYears);
            })
            .OrderBy(t => t.EventDate)
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
