using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.DataAccess
{
    // This suite attempts to test EF queries, close to the context. It draws on this article for the setup inspiration
    // (albeit without the extensive multi-provider support):
    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/testing-sample
    public class DataQualityRepositoryTests
    {
        private const string NoAlertsName = "A";
        private const string BirthCountryAlertName = "B";
        private const string ClinicalDatesAlertName = "C";

        public DataQualityRepositoryTests()
        {
            ContextOptions = new DbContextOptionsBuilder<NtbsContext>()
                .UseInMemoryDatabase(nameof(DataQualityRepositoryTests))
                .Options;
        }

        private DbContextOptions<NtbsContext> ContextOptions { get; }

        [Fact]
        public async Task FiltersOutNotificationsWithTheAlertTypeAlreadyPresent()
        {
            // ARRANGE
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                
                // We will use the birth country alert as a proxy for testing the common path ways of the
                // data quality repository
                // All seeded notifications will be eligible for this alert
                
                var notification1 = NotificationEligibleForCountryOfBirthDqAlert(NoAlertsName);

                var notification2 = NotificationEligibleForCountryOfBirthDqAlert(BirthCountryAlertName);
                notification2.Alerts = new List<Alert> {new DataQualityBirthCountryAlert()};

                var notification3 = NotificationEligibleForCountryOfBirthDqAlert(ClinicalDatesAlertName);
                notification3.Alerts = new List<Alert> {new DataQualityClinicalDatesAlert()};

                await context.AddRangeAsync(notification1, notification2, notification3);
                
                await context.SaveChangesAsync();
            }
            
            using (var context = new NtbsContext(ContextOptions))
            {
                var repo = new DataQualityRepository(context);
                
                // ACT
                // Get all possible ones
                var notifications = await repo.GetNotificationsEligibleForDqBirthCountryAlertsAsync(100, 0);
                
                // ASSERT
                // Out of the 3 eligible notifications, 1 already has the alert
                Assert.True(notifications.Any(n => n.PatientDetails.FamilyName == NoAlertsName), 
                    "Eligible notification with no other alerts not selected");
                Assert.True(notifications.Any(n => n.PatientDetails.FamilyName == ClinicalDatesAlertName),
                    "Eligible notification with only other alerts types not selected");
                Assert.Equal(2, notifications.Count);
            }
        }
        
        private static Notification NotificationEligibleForCountryOfBirthDqAlert(string name)
        {
            return new Notification
            {
                NotificationStatus = NotificationStatus.Notified,
                NotificationDate = DateTime.Now
                    .AddDays(-DataQualityRepository.MinNumberDaysNotifiedForAlert)
                    .AddDays(-10), // Make sure we are well into the eligible time range
                PatientDetails = new PatientDetails
                {
                    FamilyName = name,
                    CountryId = Countries.UnknownId
                },
                Alerts = new List<Alert>()
            };
        }
    }
}
