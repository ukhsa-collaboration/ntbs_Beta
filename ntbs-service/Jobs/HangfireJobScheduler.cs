using System;
using Hangfire;
using ntbs_service.Properties;

namespace ntbs_service.Jobs
{
    static class HangfireJobScheduler
    {
        private const string UserSyncJobId = "user-sync";
        private const string CloseInactiveNotificationsJobId = "close-inactive-notifications";
        private const string DrugResistanceProfileUpdateJobId = "drug-resistance-profile-update";
        private const string UnmatchedLabResultAlertsJobId = "unmatched-lab-result-alerts";
        private const string DataQualityAlertsJobId = "data-quality-alerts";
        private const string NotificationClusterUpdateJobId = "notification-cluster-update";
        
        public static void ScheduleRecurringJobs(ScheduledJobsConfig scheduledJobsConfig)
        {
            if (scheduledJobsConfig.UserSyncEnabled)
            {
                RecurringJob.AddOrUpdate<UserSyncJob>(
                    UserSyncJobId,
                    job => job.Run(JobCancellationToken.Null),
                    scheduledJobsConfig.UserSyncCron,
                    TimeZoneInfo.Local);
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
                    TimeZoneInfo.Local);
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
                    TimeZoneInfo.Local);
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
                    TimeZoneInfo.Local);
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
                    TimeZoneInfo.Local);
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
                    TimeZoneInfo.Local);
            }
            else
            {
                RecurringJob.RemoveIfExists(NotificationClusterUpdateJobId);
            }
        }
    }
}
