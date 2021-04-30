using System;
using Bogus;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace load_test_data_generation.Notifications
{
    internal static class ClinicalDetailsGenerator
    {
        public static ClinicalDetails GenerateClinicalDetails(Notification notification)
        {
            var testClinicalDetails = new Faker<ClinicalDetails>()
               .RuleFor(cd => cd.BCGVaccinationState, f => Status.Yes)
               .RuleFor(cd => cd.HIVTestState, f => HIVTestStatus.HIVStatusKnown)
               .RuleFor(cd => cd.IsSymptomatic, f => true)
               .RuleFor(cd => cd.DiagnosisDate, f => notification.NotificationDate.Value.Subtract(f.Date.Timespan(TimeSpan.FromDays(30))))
               .RuleFor(cd => cd.TBServicePresentationDate, (f, cd) => cd.DiagnosisDate.Value.Subtract(f.Date.Timespan(TimeSpan.FromDays(30))))
               .RuleFor(cd => cd.FirstPresentationDate, (f, cd) => cd.TBServicePresentationDate.Value.Subtract(f.Date.Timespan(TimeSpan.FromDays(30))));
            return testClinicalDetails.Generate();
        }
    }
}
