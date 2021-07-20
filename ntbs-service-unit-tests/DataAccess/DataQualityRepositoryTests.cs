using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.QueryEntities;
using Xunit;

namespace ntbs_service_unit_tests.DataAccess
{
    public class DataQualityRepositoryTests : IClassFixture<DataQualityRepositoryFixture>, IDisposable
    {
        private readonly NtbsContext _context;

        private const string NoAlertsName = "A";
        private const string BirthCountryAlertName = "B";
        private const string ClinicalDatesAlertName = "C";

        public DataQualityRepositoryTests(DataQualityRepositoryFixture dataQualityRepositoryFixture)
        {
            _context = dataQualityRepositoryFixture.Context;
        }

        [Fact]
        public async Task FiltersOutNotificationsWithTheAlertTypeAlreadyPresent()
        {
            // ARRANGE
            // We will use the birth country alert as a proxy for testing the common path ways of the
            // data quality repository
            // All seeded notifications will be eligible for this alert

            var notification1 = NotificationEligibleForCountryOfBirthDqAlert(NoAlertsName);

            var notification2 = NotificationEligibleForCountryOfBirthDqAlert(BirthCountryAlertName);
            notification2.Alerts = new List<Alert> { new DataQualityBirthCountryAlert() };

            var notification3 = NotificationEligibleForCountryOfBirthDqAlert(ClinicalDatesAlertName);
            notification3.Alerts = new List<Alert> { new DataQualityClinicalDatesAlert() };

            await _context.AddRangeAsync(notification1, notification2, notification3);
            await _context.SaveChangesAsync();

            var repo = new DataQualityRepository(_context);

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

        [Theory]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "9620869346", "9620869346")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Closed, null, null,
            "9620869346", "9620869346")]
        [InlineData(NotificationStatus.Closed, NotificationStatus.Closed, null, null,
            "9620869346", "9620869346")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", null,
            "9620869346", "9620869346")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", "2",
            "9620869346", "9620869346")]
        public async Task NotificationsWithDuplicateNhsNumbers_CorrectlyFindsDuplicates(
            NotificationStatus status1, NotificationStatus status2, string groupName1, string groupName2,
            string nhsNumber1, string nhsNumber2)
        {
            // Arrange
            var group1 = new NotificationGroup();
            var group2 = new NotificationGroup();
            await _context.NotificationGroup.AddRangeAsync(group1, group2);

            var notification1 = new Notification
            {
                NotificationStatus = status1,
                NotificationDate = DateTime.Now,
                Group = groupName1 == "1" ? group1 : groupName1 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    NhsNumber = nhsNumber1,
                }
            };
            var notification2 = new Notification
            {
                NotificationStatus = status2,
                NotificationDate = DateTime.Now,
                Group = groupName2 == "1" ? group1 : groupName2 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    NhsNumber = nhsNumber2,
                }
            };

            await _context.AddRangeAsync(notification1, notification2);
            await _context.SaveChangesAsync();

            var repo = new DataQualityRepository(_context);

            // Act
            var notificationIds = await repo.GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync();

            // Assert
            AssertDuplicatePairIsFound(notificationIds, notification1.NotificationId, notification2.NotificationId);
        }

        [Theory]
        [InlineData(NotificationStatus.Deleted, NotificationStatus.Notified, null, null,
            "9620869346", "9620869346")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Denotified, null, null,
            "9620869346", "9620869346")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Draft, null, null,
            "9620869346", "9620869346")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "0000000000", "9620869346")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            null, null)]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", "1",
            "9620869346", "9620869346")]
        public async Task NotificationsWithDuplicateNhsNumbers_CorrectlyIgnoresNonDuplicates(
            NotificationStatus status1, NotificationStatus status2, string groupName1, string groupName2,
            string nhsNumber1, string nhsNumber2)
        {
            // Arrange
            var group1 = new NotificationGroup();
            var group2 = new NotificationGroup();
            await _context.NotificationGroup.AddRangeAsync(group1, group2);

            var notification1 = new Notification
            {
                NotificationStatus = status1,
                NotificationDate = DateTime.Now,
                Group = groupName1 == "1" ? group1 : groupName1 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    NhsNumber = nhsNumber1,
                }
            };
            var notification2 = new Notification
            {
                NotificationStatus = status2,
                NotificationDate = DateTime.Now,
                Group = groupName2 == "1" ? group1 : groupName2 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    NhsNumber = nhsNumber2,
                }
            };

            await _context.AddRangeAsync(notification1, notification2);
            await _context.SaveChangesAsync();

            var repo = new DataQualityRepository(_context);

            // Act
            var notificationIds = await repo.GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync();

            // Assert
            Assert.Empty(notificationIds);
        }

        [Theory]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Closed, null, null,
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Closed, NotificationStatus.Closed, null, null,
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", null,
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", "2",
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "John", "Smith", "Smith", "John", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Closed, null, null,
            "John", "Smith", "Smith", "John", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Closed, NotificationStatus.Closed, null, null,
            "John", "Smith", "Smith", "John", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", null,
            "John", "Smith", "Smith", "John", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", "2",
            "John", "Smith", "Smith", "John", "2020-01-01", "2020-01-01")]
        public async Task NotificationsWithDuplicateNamesAndDobs_CorrectlyFindsDuplicates(
            NotificationStatus status1, NotificationStatus status2, string groupName1, string groupName2,
            string givenName1, string givenName2, string familyName1, string familyName2, string dob1, string dob2)
        {
            // Arrange
            var group1 = new NotificationGroup();
            var group2 = new NotificationGroup();
            await _context.NotificationGroup.AddRangeAsync(group1, group2);

            var notification1 = new Notification
            {
                NotificationStatus = status1,
                NotificationDate = DateTime.Now,
                Group = groupName1 == "1" ? group1 : groupName1 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    GivenName = givenName1,
                    FamilyName = familyName1,
                    Dob = dob1 == null ? null : (DateTime?)DateTime.Parse(dob1)
                }
            };
            var notification2 = new Notification
            {
                NotificationStatus = status2,
                NotificationDate = DateTime.Now,
                Group = groupName2 == "1" ? group1 : groupName2 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    GivenName = givenName2,
                    FamilyName = familyName2,
                    Dob = dob1 == null ? null : (DateTime?)DateTime.Parse(dob2)
                }
            };

            await _context.AddRangeAsync(notification1, notification2);
            await _context.SaveChangesAsync();

            var repo = new DataQualityRepository(_context);

            // Act
            var notificationIds = await repo.GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync();

            // Assert
            AssertDuplicatePairIsFound(notificationIds, notification1.NotificationId, notification2.NotificationId);
        }

        [Theory]
        [InlineData(NotificationStatus.Deleted, NotificationStatus.Notified, null, null,
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Denotified, null, null,
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Draft, null, null,
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "John", "John", "Smith", "Smith", "1990-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "John", "John", "Smith", "Smith", null, null)]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "John", "John", "Johnson", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "John", "John", null, null, "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            "John", "James", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, null, null,
            null, null, "Smith", "Smith", "2020-01-01", "2020-01-01")]
        [InlineData(NotificationStatus.Notified, NotificationStatus.Notified, "1", "1",
            "John", "John", "Smith", "Smith", "2020-01-01", "2020-01-01")]
        public async Task NotificationsWithDuplicateNamesAndDobs_CorrectlyIgnoresNonDuplicates(
            NotificationStatus status1, NotificationStatus status2, string groupName1, string groupName2,
            string givenName1, string givenName2, string familyName1, string familyName2, string dob1, string dob2)
        {
            // Arrange
            var group1 = new NotificationGroup();
            var group2 = new NotificationGroup();
            await _context.NotificationGroup.AddRangeAsync(group1, group2);

            var notification1 = new Notification
            {
                NotificationStatus = status1,
                NotificationDate = DateTime.Now,
                Group = groupName1 == "1" ? group1 : groupName1 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    GivenName = givenName1,
                    FamilyName = familyName1,
                    Dob = dob1 == null ? null : (DateTime?)DateTime.Parse(dob1)
                }
            };
            var notification2 = new Notification
            {
                NotificationStatus = status2,
                NotificationDate = DateTime.Now,
                Group = groupName2 == "1" ? group1 : groupName2 == "2" ? group2 : null,
                PatientDetails = new PatientDetails
                {
                    GivenName = givenName2,
                    FamilyName = familyName2,
                    Dob = dob1 == null ? null : (DateTime?)DateTime.Parse(dob2)
                }
            };

            await _context.AddRangeAsync(notification1, notification2);
            await _context.SaveChangesAsync();

            var repo = new DataQualityRepository(_context);

            // Act
            var notificationIds = await repo.GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync();

            // Assert
            Assert.Empty(notificationIds);
        }

        private static void AssertDuplicatePairIsFound(IList<NotificationAndDuplicateIds> notificationIds,
            int notificationId1, int notificationId2)
        {
            Assert.Contains(notificationIds, pair =>
                pair.NotificationId == notificationId1
                && pair.DuplicateId == notificationId2);
            Assert.Contains(notificationIds, pair =>
                pair.NotificationId == notificationId2
                && pair.DuplicateId == notificationId1);
            Assert.Equal(2, notificationIds.Count);
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

        public void Dispose()
        {
            _context.Notification.RemoveRange(_context.Notification);
            _context.NotificationGroup.RemoveRange(_context.NotificationGroup);
            _context.SaveChanges();
        }
    }
}
