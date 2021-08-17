using System;
using Hangfire;
using ntbs_service.Properties;
using TimeZoneConverter;

namespace ntbs_service.Jobs
{
    internal static class HangfireJobScheduler
    {
        private const string UserSyncJobId = "user-sync";
        private const string CloseInactiveNotificationsJobId = "close-inactive-notifications";
        private const string DrugResistanceProfileUpdateJobId = "drug-resistance-profile-update";
        private const string UnmatchedLabResultAlertsJobId = "unmatched-lab-result-alerts";
        private const string DataQualityAlertsJobId = "data-quality-alerts";
        private const string NotificationClusterUpdateJobId = "notification-cluster-update";
        private const string MarkImportedNotificationsAsImportedJobId = "mark-notifications-as-imported";
        private const string GenerateReportingDataJobId = "generate-report-data";
        private const string ReportingDataRefreshJobId = "reporting-data-refresh";
        private const string ReportingDataProcessingJobId = "reporting-data-processing";
        private const string UpdateTableCountsJobId = "update-table-counts";

        // The window in which our overnight jobs must run has become so narrow that we now need to adjust for local time
        // to be able to reliably fit them in before the start of the working day.
        // We're using TZConvert rather than TimeZoneInfo.FindSystemTimeZoneById because it is cross-platform. Note, however,
        // that this will not be necessary in .NET 6, so when the project updates to .NET 6 we can remove the dependency.
        // See https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#time-zone-conversion-apis
        private static readonly TimeZoneInfo GmtStandardTime = TZConvert.GetTimeZoneInfo("GMT Standard Time");

        public static void ScheduleRecurringJobs(ScheduledJobsConfig scheduledJobsConfig)
        {
            if (scheduledJobsConfig.UserSyncEnabled)
            {
                RecurringJob.AddOrUpdate<UserSyncJob>(
                    UserSyncJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.UserSyncCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(UserSyncJobId);
            }

            if (scheduledJobsConfig.CloseInactiveNotificationsEnabled)
            {
                RecurringJob.AddOrUpdate<CloseInactiveNotificationsJob>(
                    CloseInactiveNotificationsJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.CloseInactiveNotificationsCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(CloseInactiveNotificationsJobId);
            }

            if (scheduledJobsConfig.DrugResistanceProfileUpdateEnabled)
            {
                RecurringJob.AddOrUpdate<DrugResistanceProfileUpdateJob>(
                    DrugResistanceProfileUpdateJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.DrugResistanceProfileUpdateCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(DrugResistanceProfileUpdateJobId);
            }

            if (scheduledJobsConfig.UnmatchedLabResultAlertsEnabled)
            {
                RecurringJob.AddOrUpdate<UnmatchedLabResultAlertsJob>(
                    UnmatchedLabResultAlertsJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.UnmatchedLabResultAlertsCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(UnmatchedLabResultAlertsJobId);
            }

            if (scheduledJobsConfig.DataQualityAlertsEnabled)
            {
                RecurringJob.AddOrUpdate<DataQualityAlertsJob>(
                    DataQualityAlertsJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.DataQualityAlertsCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(DataQualityAlertsJobId);
            }

            if (scheduledJobsConfig.NotificationClusterUpdateEnabled)
            {
                RecurringJob.AddOrUpdate<NotificationClusterUpdateJob>(
                    NotificationClusterUpdateJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.NotificationClusterUpdateCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(NotificationClusterUpdateJobId);
            }

            if (scheduledJobsConfig.MarkImportedNotificationsAsImportedEnabled)
            {
                RecurringJob.AddOrUpdate<MarkImportedNotificationsAsImportedJob>(
                    MarkImportedNotificationsAsImportedJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.MarkImportedNotificationsAsImportedCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(MarkImportedNotificationsAsImportedJobId);
            }

            if (scheduledJobsConfig.GenerateReportingDataJobEnabled)
            {
                // PerformContext context is passed in via Hangfire Server
                RecurringJob.AddOrUpdate<GenerateReportingDataJob>(
                    GenerateReportingDataJobId,
                    job => job.Run(null),
                    scheduledJobsConfig.GenerateReportingDataJobCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(GenerateReportingDataJobId);
            }

            if (scheduledJobsConfig.ReportingDataRefreshJobEnabled)
            {
                // PerformContext context is passed in via Hangfire Server
                RecurringJob.AddOrUpdate<ReportingDataRefreshJob>(
                    ReportingDataRefreshJobId,
                    job => job.Run(null),
                    scheduledJobsConfig.ReportingDataRefreshJobCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(ReportingDataRefreshJobId);
            }

            if (scheduledJobsConfig.ReportingDataProcessingJobEnabled)
            {
                // PerformContext context is passed in via Hangfire Server
                RecurringJob.AddOrUpdate<ReportingDataProcessingJob>(
                    ReportingDataProcessingJobId,
                    job => job.Run(null),
                    scheduledJobsConfig.ReportingDataProcessingJobCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(ReportingDataProcessingJobId);
            }

            if (scheduledJobsConfig.UpdateTableCountsJobEnabled)
            {
                // PerformContext context is passed in via Hangfire Server
                RecurringJob.AddOrUpdate<UpdateTableCountsJob>(
                    UpdateTableCountsJobId,
                    job => job.Run(null),
                    scheduledJobsConfig.UpdateTableCountsJobCron,
                    GmtStandardTime);
            }
            else
            {
                RecurringJob.RemoveIfExists(UpdateTableCountsJobId);
            }
        }
    }
}
