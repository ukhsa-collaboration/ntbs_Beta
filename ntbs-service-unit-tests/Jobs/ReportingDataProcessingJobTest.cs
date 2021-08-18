using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Jobs;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Xunit;

namespace ntbs_service_unit_tests.Jobs
{
    public class ReportingDataProcessingJobTest
    {
        private readonly Mock<IExternalStoredProcedureRepository> _externalStoredProcedureRepository;
        private readonly ReportingDataProcessingJob _reportingDataProcessingJob;

        public ReportingDataProcessingJobTest()
        {
            _externalStoredProcedureRepository = new Mock<IExternalStoredProcedureRepository>();
            _reportingDataProcessingJob = new ReportingDataProcessingJob(_externalStoredProcedureRepository.Object);
        }

        [Fact]
        public async Task Run_CallsPopulateStoredProcedure()
        {
            // Arrange
            _externalStoredProcedureRepository
                .Setup(r => r.ExecutePopulateForestExtractStoredProcedure())
                .Returns(Task.FromResult(Enumerable.Empty<dynamic>()));

            // Act
            await _reportingDataProcessingJob.Run(null);

            // Assert
            _externalStoredProcedureRepository
                .Verify(r => r.ExecutePopulateForestExtractStoredProcedure(), Times.Once);
            _externalStoredProcedureRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Run_LogsSuccess()
        {
            // Arrange
            _externalStoredProcedureRepository
                .Setup(r => r.ExecutePopulateForestExtractStoredProcedure())
                .Returns(Task.FromResult(Enumerable.Empty<dynamic>()));

            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            using (TestCorrelator.CreateContext())
            {
                // Act
                await _reportingDataProcessingJob.Run(null);

                // Assert
                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToList();

                Assert.DoesNotContain(logEvents, logEvent => logEvent.Level > LogEventLevel.Information);
                Assert.Contains(logEvents,
                    logEvent => logEvent.Level == LogEventLevel.Information
                                && logEvent.RenderMessage() == "Starting reporting data processing job");
                Assert.Contains(logEvents,
                    logEvent => logEvent.Level == LogEventLevel.Information
                                && logEvent.RenderMessage() == "Finishing reporting data processing job");
            }
        }

        [Fact]
        public async Task Run_ThrowsOnStoredProcedureMessages()
        {
            // Arrange
            List<dynamic> sqlResult = new List<object> { new { ErrorMessage = "Divide by zero error encountered." } };
            const string serialisedResult = "[{\"ErrorMessage\":\"Divide by zero error encountered.\"}]";

            _externalStoredProcedureRepository
                .Setup(r => r.ExecutePopulateForestExtractStoredProcedure())
                .Returns(Task.FromResult(sqlResult.AsEnumerable()));

            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            using (TestCorrelator.CreateContext())
            {
                // Act
                var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
                    await _reportingDataProcessingJob.Run(null));

                // Assert
                Assert.Equal("Stored procedure did not execute successfully as result has messages, check the logs",
                    exception.Message);

                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToList();

                Assert.Contains(logEvents,
                    logEvent => logEvent.Level == LogEventLevel.Information
                                && logEvent.RenderMessage() == $"Result: {serialisedResult}");
                Assert.Contains(logEvents,
                    logEvent => logEvent.Level == LogEventLevel.Error
                                && logEvent.RenderMessage() == "Error occured during reporting data processing job");
            }
        }

        [Fact]
        public async Task Run_ThrowsOnRepositoryException()
        {
            // Arrange
            var expectedException = new ApplicationException("Connection was unsuccessful");
            _externalStoredProcedureRepository
                .Setup(r => r.ExecutePopulateForestExtractStoredProcedure())
                .Throws(expectedException);

            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            using (TestCorrelator.CreateContext())
            {
                // Act
                var actualException = await Assert.ThrowsAsync<ApplicationException>(async () =>
                    await _reportingDataProcessingJob.Run(null));

                // Assert
                Assert.Equal(expectedException, actualException);

                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToList();

                Assert.Contains(logEvents,
                    logEvent => logEvent.Level == LogEventLevel.Error
                                && logEvent.RenderMessage() == "Error occured during reporting data processing job"
                                && logEvent.Exception == expectedException);
            }
        }
    }
}
