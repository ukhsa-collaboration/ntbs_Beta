using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
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

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("ntbsContext");
            
            var options = new DbContextOptionsBuilder<NtbsContext>()
                .UseSqlServer(connectionString)
                .Options;
            
            using (var context = new NtbsContext(options))
            {
                if (!context.Database.GetService<IRelationalDatabaseCreator>().Exists())
                {
                    Console.WriteLine("Database does not exist, aborting bulk insert");
                    return;
                }
                Console.WriteLine("Starting generation of notifications");
                await GenerateNotifications(context);
                Console.WriteLine("Finished generation of notifications");
            }
        }


        static async Task GenerateNotifications(NtbsContext context)
        {
            var numberOfNotifications = 3000;
            var notificationsOperable = Builder<Notification>.CreateListOfSize(numberOfNotifications)
                .All()
                .With(n => n.NotificationId = 0)
                .With(n => n.NotificationStatus = NotificationStatus.Notified)
                .With(n => n.PatientDetails.GivenName = Faker.Name.First())
                .With(n => n.PatientDetails.FamilyName = Faker.Name.Last())
                .With(n => n.ClinicalDetails.Notes = "UniqueBulkInsert")
                .With(n => n.GroupId = null);

            // Comment out the line below to cause Treatment Outcome Alerts to be generated
            // notificationsOperable = await AddDataQualityTreatmentEvents(notificationsOperable, context);
            
            var notifications= notificationsOperable.Build();
            
            // Add random fields that Faker is not capable of
            foreach (var notification in notifications)
            {
                var rand = new Random();
                var days = rand.Next(1, 1000);
                notification.NotificationDate = new DateTime(2014, 1, 1).AddDays(days);
                var tbServices = (await context.TbService.ToListAsync()).Select(t => t.Code).ToList();
                var tbServiceIndex = rand.Next(0, tbServices.Count() - 1);
                notification.HospitalDetails.TBServiceCode = tbServices[tbServiceIndex];
                var nhsNumberString = "9";
                for (var i = 0; i < 9; i++)
                {
                    nhsNumberString = String.Concat(nhsNumberString, rand.Next(1, 9).ToString());
                }
                notification.PatientDetails.NhsNumber = nhsNumberString;

            }
            
            context.AddRange(notifications);
            await context.SaveChangesAsync();
        }

        private static async Task<IOperable<Notification>> AddDataQualityTreatmentEvents(IOperable<Notification> notificationsOperable, NtbsContext context)
        {
            var completedOutcome = await context.TreatmentOutcome.Where(t => t.TreatmentOutcomeId == 1).FirstOrDefaultAsync();
            var notificationsOperableWithTreatmentEvents = notificationsOperable
                .With(n => n.TreatmentEvents = new List<TreatmentEvent>
                {
                    new TreatmentEvent
                    {
                        TreatmentEventType = TreatmentEventType.TreatmentStart,
                        EventDate = new DateTime(2017, 1, 1)
                    },
                    new TreatmentEvent
                    {
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        EventDate = new DateTime(2017, 2, 1),
                        TreatmentOutcomeId = completedOutcome.TreatmentOutcomeId,
                        TreatmentOutcome = completedOutcome
                    }
                });
            return notificationsOperableWithTreatmentEvents;
        }
        
    }
}
