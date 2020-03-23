using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("ntbsContext");
            
            var options = new DbContextOptionsBuilder<NtbsContext>()
                .UseSqlServer(connectionString)
                .Options;
            
            using (var context = new NtbsContext(options))
            {
                context.Database.Migrate();
                Console.WriteLine("Starting generation of notifications");
                await generateNotifications(context);
                Console.WriteLine("Finished generation of notifications");
            }
        }


        static async Task generateNotifications(NtbsContext context)
        {
            var numberOfNotifications = 30000;
            var notificationsOperable = Builder<Notification>.CreateListOfSize(numberOfNotifications)
                .All()
                .With(n => n.NotificationId = 0)
                .With(n => n.NotificationStatus = NotificationStatus.Notified)
                .With(n => n.PatientDetails.GivenName = Faker.Name.First())
                .With(n => n.PatientDetails.FamilyName = Faker.Name.Last())
                .With(n => n.PatientDetails.NhsNumberNotKnown = true)
                .With(n => n.HospitalDetails.TBServiceCode = "TBS0008")
                .With(n => n.HospitalDetails.Consultant = "UniqueBulkInsert")
                .With(n => n.NotificationDate = new DateTime(2017, 1, 1))
                .With(n => n.GroupId = null);

            // Comment out line below to not add the Treatment event which trigger Treatment Outcome DQ alerts
            notificationsOperable = AddDataQualityTreatmentEvents(notificationsOperable);

            var notifications= notificationsOperable.Build();
            context.AddRange(notifications);
            await context.SaveChangesAsync();
        }

        private static IOperable<Notification> AddDataQualityTreatmentEvents(IOperable<Notification> notificationsOperable)
        {
            var x = new Random().Next(1, 6);
            if (x == 1)
            {
                var notificationsOperableWithTreatmentEvents = notificationsOperable
                    .With(n => n.TreatmentEvents = new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            TreatmentEventType = TreatmentEventType.TreatmentStart,
                            EventDate = new DateTime(2017, 1, 1)
                        }
                    });
                return notificationsOperableWithTreatmentEvents;
            }

            return notificationsOperable;
        }
        
    }
}
