using System;
using System.Collections.Generic;
using Bogus;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace load_test_data_generation.Notifications
{
    internal static class TreatmentEventsGenerator
    {
        // This corresponds to "Completed - Standard Therapy". If we were randomizing this then we would get
        // it from the database, in the same way that we do in other generators. However, since we're hard-coding
        // it for now, this doesn't seem worthwhile.
        private const int CompletedOutcomeId = 1;

        public static List<TreatmentEvent> GenerateTreatmentEvents(Notification notification)
        {
            var startEventGenerator = new Faker<TreatmentEvent>()
                .RuleFor(te => te.TreatmentEventType, f => TreatmentEventType.TreatmentStart)
                .RuleFor(te => te.EventDate, f => notification.NotificationDate.Value.Add(f.Date.Timespan(TimeSpan.FromDays(14))));
            var startEvent = startEventGenerator.Generate();

            var endEventGenerator = new Faker<TreatmentEvent>()
                .RuleFor(te => te.TreatmentEventType, f => TreatmentEventType.TreatmentOutcome)
                .RuleFor(te => te.EventDate, f => startEvent.EventDate.Value.Add(f.Date.Timespan(TimeSpan.FromDays(120))))
                .RuleFor(te => te.TreatmentOutcomeId, f => CompletedOutcomeId);
            var endEvent = endEventGenerator.Generate();

            return new List<TreatmentEvent> { startEvent, endEvent };
        }
    }
}
