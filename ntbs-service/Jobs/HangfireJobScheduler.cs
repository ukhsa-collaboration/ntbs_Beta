using System;
using Hangfire;
using ntbs_service.Properties;

namespace ntbs_service.Jobs
{
    static class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs(ScheduledJobConfig scheduledJobConfig)
        {
            if (scheduledJobConfig.UserSyncEnabled)
            {
                RecurringJob.AddOrUpdate<UserSyncJob>(
                    "user-sync",
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobConfig.UserSyncTiming,
                    TimeZoneInfo.Local);
            }

            if (scheduledJobConfig.DrugResistanceProfileUpdateEnabled)
            {
                RecurringJob.AddOrUpdate<DrugResistanceProfileUpdateJob>(
                    "drug-resistance-profile-update",
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobConfig.DrugResistanceProfileUpdateTiming,
                    TimeZoneInfo.Local);
            }

            if (scheduledJobConfig.UnmatchedLabResultAlertsEnabled)
            {
                RecurringJob.AddOrUpdate<UnmatchedLabResultAlertsJob>(
                    "unmatched-lab-result-alerts",
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobConfig.UnmatchedLabResultAlertsTiming,
                    TimeZoneInfo.Local);
            }

            if (scheduledJobConfig.DataQualityAlertsEnabled)
            {
                RecurringJob.AddOrUpdate<DataQualityAlertsJob>(
                    "data-quality-alerts",
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobConfig.DataQualityAlertsTiming,
                    TimeZoneInfo.Local);
            }

            if (scheduledJobConfig.NotificationClusterUpdateEnabled)
            {
                RecurringJob.AddOrUpdate<NotificationClusterUpdateJob>(
                    "notification-cluster-update",
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobConfig.NotificationClusterUpdateTiming,
                    TimeZoneInfo.Local);
            }
        }
    }
}
