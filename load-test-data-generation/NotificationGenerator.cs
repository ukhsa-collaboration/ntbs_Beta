using System;
using System.Linq;
using Bogus;
using load_test_data_generation.Notifications;
using MoreLinq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace load_test_data_generation
{
    internal class NotificationGenerator
    {
        private readonly IContextProvider contextProvider;
        private PatientDetailsGenerator patientDetailsGenerator;
        private HospitalDetailsGenerator hospitalDetailsGenerator;
        private TestResultGenerator testResultGenerator;

        private const int BatchSize = 100;

        private static readonly DateTime StartOf2015 = new DateTime(2014, 01, 01, 00, 00, 00);
        private static readonly DateTime EndOf2020 = new DateTime(2020, 12, 31, 23, 59, 59);

        private readonly Faker<Notification> testNotifications = new Faker<Notification>()
            .RuleFor(n => n.NotificationDate, f => f.Date.Between(StartOf2015, EndOf2020))
            .RuleFor(n => n.NotificationStatus, f => NotificationStatus.Notified)
            .RuleFor(n => n.SubmissionDate, (f, u) => u.NotificationDate.Value.Add(f.Date.Timespan(TimeSpan.FromDays(2))))
            .RuleFor(n => n.ETSID, f => null)
            .RuleFor(n => n.LTBRID, f => null)
            .RuleFor(n => n.LTBRPatientId, f => null)
            .RuleFor(n => n.GroupId, f => null)
            .RuleFor(n => n.ClusterId, f => null)
            .RuleFor(n => n.CreationDate, f => DateTime.UtcNow);

        public NotificationGenerator(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public void GenerateNotifications(int numberOfNotificationsToGenerate)
        {
            Console.WriteLine("Initialising services...");
            patientDetailsGenerator = new PatientDetailsGenerator(contextProvider);
            patientDetailsGenerator.Initialise();

            hospitalDetailsGenerator = new HospitalDetailsGenerator(contextProvider);
            hospitalDetailsGenerator.Initialise();

            testResultGenerator = new TestResultGenerator(contextProvider);
            testResultGenerator.Initialise();

            // Split into batches to avoid performance degradation of having too much data in context.
            foreach (var batch in Enumerable.Range(0, numberOfNotificationsToGenerate).Batch(BatchSize))
            {
                var notifications = batch.Select(_ => GenerateNotification()).ToList();
                contextProvider.WithContext(context =>
                {
                    Console.WriteLine("Saving batch of notifications in database...");
                    context.AddRange(notifications);
                    context.SaveChanges();
                });
            }
        }

        private Notification GenerateNotification()
        {
            Console.WriteLine("Generating notification...");
            var notification = testNotifications.Generate();

            notification.PatientDetails = patientDetailsGenerator.GeneratePatientDetails();
            notification.HospitalDetails = hospitalDetailsGenerator.GenerateHospitalDetails();
            notification.NotificationSites = NotificationSiteGenerator.GenerateNotificationSites();
            notification.ClinicalDetails = ClinicalDetailsGenerator.GenerateClinicalDetails(notification);
            notification.TestData = testResultGenerator.GenerateTestData();
            notification.TreatmentEvents = TreatmentEventsGenerator.GenerateTreatmentEvents(notification);

            EnsureConsistentStatus(notification);

            return notification;
        }

        private static void EnsureConsistentStatus(Notification notification)
        {
            if (notification.ShouldBeClosed())
            {
                notification.NotificationStatus = NotificationStatus.Closed;
            }
        }
    }
}
