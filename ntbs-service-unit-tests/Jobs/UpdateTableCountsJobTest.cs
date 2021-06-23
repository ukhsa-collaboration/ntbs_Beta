using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.Jobs;
using ntbs_service.Models.QueryEntities;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Xunit;

namespace ntbs_service_unit_tests.Jobs
{
    public class UpdateTableCountsJobTest
    {
        private readonly Mock<ITableCountsRepository> _tableCountsRepository;

        private readonly UpdateTableCountsJob _updateTableCountsJob;

        public UpdateTableCountsJobTest()
        {
            _tableCountsRepository = new Mock<ITableCountsRepository>();

            _updateTableCountsJob = new UpdateTableCountsJob(_tableCountsRepository.Object);
        }

        [Fact]
        public async Task Run_CallsUpdateStoredProcedure()
        {
            // Arrange
            _tableCountsRepository
                .Setup(r => r.GetRecentTableCounts())
                .Returns(Task.FromResult(Enumerable.Repeat(CreateBasicTableCounts(), 2)));

            // Act
            await _updateTableCountsJob.Run(null);

            // Assert
            _tableCountsRepository.Verify(r => r.ExecuteUpdateTableCountsStoredProcedure());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Run_LogsInfo_WhenNotEnoughRecords(int numberOfRecords)
        {
            // Arrange
            _tableCountsRepository
                .Setup(r => r.GetRecentTableCounts())
                .Returns(Task.FromResult(Enumerable.Repeat(CreateBasicTableCounts(), numberOfRecords)));

            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            using (TestCorrelator.CreateContext())
            {
                // Act
                await _updateTableCountsJob.Run(null);
                const string expectedMessage =
                    "Not enough table counts available. Try re-running the update-table-counts job.";

                // Assert
                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToList();

                Assert.DoesNotContain(logEvents, logEvent => logEvent.Level > LogEventLevel.Information);
                Assert.Contains(logEvents, logEvent => logEvent.Level == LogEventLevel.Information
                                                       && logEvent.RenderMessage() == expectedMessage);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Run_LogsInfo_WhenNoFieldsReduced(int count)
        {
            // Arrange
            _tableCountsRepository
                .Setup(r => r.GetRecentTableCounts())
                .Returns(Task.FromResult(Enumerable.Repeat(CreateBasicTableCounts(count), 2)));

            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            using (TestCorrelator.CreateContext())
            {
                // Act
                await _updateTableCountsJob.Run(null);

                // Assert
                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToList();

                Assert.DoesNotContain(logEvents, logEvent => logEvent.Level > LogEventLevel.Information);
                Assert.Contains(logEvents, logEvent => logEvent.Level == LogEventLevel.Information
                                                       && logEvent.RenderMessage() == "Table counts look normal");
            }
        }

        [Theory]
        [MemberData(nameof(CountNameObjects))]
        public async Task Run_LogsWarnings_WhenSingleFieldIsReduced(string fieldName)
        {
            // Arrange
            var previousTableCounts = CreateBasicTableCounts(0);
            typeof(TableCounts)
                .GetProperty(fieldName)
                .SetValue(previousTableCounts, 1);

            var tableCounts = new[] { CreateBasicTableCounts(0), previousTableCounts };
            _tableCountsRepository
                .Setup(r => r.GetRecentTableCounts())
                .Returns(Task.FromResult(tableCounts.AsEnumerable()));

            // Act and assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _updateTableCountsJob.Run(null));
            Assert.Contains("Some table counts have decreased", exception.Message);
            Assert.Contains($"Property: {fieldName} has decreased from 1 to 0", exception.Message);
        }

        [Fact]
        public async Task Run_LogsWarnings_WhenAllFieldsAreReduced()
        {
            // Arrange
            var tableCounts = new[] { CreateBasicTableCounts(0), CreateBasicTableCounts(1) };
            _tableCountsRepository
                .Setup(r => r.GetRecentTableCounts())
                .Returns(Task.FromResult(tableCounts.AsEnumerable()));

            // Act and assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _updateTableCountsJob.Run(null));
            Assert.Contains("Some table counts have decreased", exception.Message);
            CountNames.ForEach(fieldName =>
                Assert.Contains($"Property: {fieldName} has decreased from 1 to 0", exception.Message)
            );
        }

        private static IEnumerable<string> CountNames => new[]
        {
            nameof(TableCounts.MigrationNotificationsViewCount),
            nameof(TableCounts.MigrationMBovisAnimalExposureViewCount),
            nameof(TableCounts.MigrationMBovisExposureToKnownCaseViewCount),
            nameof(TableCounts.MigrationMBovisOccupationExposuresViewCount),
            nameof(TableCounts.MigrationMBovisUnpasteurisedMilkConsumptionViewCount),
            nameof(TableCounts.MigrationSocialContextAddressViewCount),
            nameof(TableCounts.MigrationSocialContextVenueViewCount),
            nameof(TableCounts.TransfersViewCount),
            nameof(TableCounts.TreatmentOutcomesCount),
            nameof(TableCounts.EtsNotificationsCount),
            nameof(TableCounts.LtbrNotificationsCount),
            nameof(TableCounts.ETS_NotificationCount),
            nameof(TableCounts.LTBR_DiseasePeriodCount),
            nameof(TableCounts.LTBR_PatientEpisodeCount),
            nameof(TableCounts.NotificationClusterMatchCount),
            nameof(TableCounts.NotificationSpecimenMatchCount),
            nameof(TableCounts.EtsSpecimenMatchCount)
        };

        public static IEnumerable<object[]> CountNameObjects =>
            CountNames.Select(name => new object[] { name });

        private static TableCounts CreateBasicTableCounts(int count = 0) =>
            new TableCounts
            {
                CountTime = DateTime.Now,
                MigrationNotificationsViewCount = count,
                MigrationMBovisAnimalExposureViewCount = count,
                MigrationMBovisExposureToKnownCaseViewCount = count,
                MigrationMBovisOccupationExposuresViewCount = count,
                MigrationMBovisUnpasteurisedMilkConsumptionViewCount = count,
                MigrationSocialContextAddressViewCount = count,
                MigrationSocialContextVenueViewCount = count,
                TransfersViewCount = count,
                TreatmentOutcomesCount = count,
                EtsNotificationsCount = count,
                LtbrNotificationsCount = count,
                ETS_NotificationCount = count,
                LTBR_DiseasePeriodCount = count,
                LTBR_PatientEpisodeCount = count,
                NotificationClusterMatchCount = count,
                NotificationSpecimenMatchCount = count,
                EtsSpecimenMatchCount = count
            };
    }
}
