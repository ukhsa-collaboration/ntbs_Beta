using System;
using System.Collections.Generic;
using System.IO;
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
            // generate some of dat csv shit
            // insert of some dat csv shit

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("ntbsContext");

            var options = new DbContextOptionsBuilder<NtbsContext>()
                .UseSqlServer("data source=localhost;initial catalog=ntbsDev;trusted_connection=true;MultipleActiveResultSets=true")
                .Options;
            
            using (var context = new NtbsContext(options))
            {
                Console.WriteLine("Hello World!");
                Console.ReadLine();
                await generateNotifications(context);
                Console.WriteLine("Cya");
            }
        }


        static async Task generateNotifications(NtbsContext context)
        {
            var notificationsOperable = Builder<Notification>.CreateListOfSize(100)
                .All()
                .With(n => n.PatientDetails.GivenName = Faker.Name.First())
                .With(n => n.PatientDetails.FamilyName = Faker.Name.Last())
                .With(n => n.PatientDetails.NhsNumberNotKnown = true)
                .With(n => n.HospitalDetails.TBServiceCode = "TBS0008")
                .With(n => n.HospitalDetails.Consultant == "UniqueBulkInsert")
                .With(n => n.NotificationDate = new DateTime(2017, 1, 1));

            var notifications= notificationsOperable.Build();
            context.BulkInsert(notifications);
            await context.SaveChangesAsync();
        }

        private IOperable<Notification> AddDataQualityTreatmentEvents(IOperable<Notification> notificationsOperable)
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
        
    }
}
