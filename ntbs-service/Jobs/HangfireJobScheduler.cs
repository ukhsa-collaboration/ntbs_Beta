using System;
using Hangfire;

namespace ntbs_service.Jobs
{
    static class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<UserSyncJob>(
                "user-sync",
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily(3),
                TimeZoneInfo.Local);            
            
            RecurringJob.AddOrUpdate<DrugResistanceProfileUpdateJob>(
                "drug-resistance-profile-update",
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily(4),
                TimeZoneInfo.Local);
            
            RecurringJob.AddOrUpdate<UnmatchedLabResultAlertsJob>(
                "unmatched-lab-result-alerts",
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily(4),
                TimeZoneInfo.Local);
            
            RecurringJob.AddOrUpdate<DataQualityAlertsJob>(
                "data-quality-alerts",
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily(4),
                TimeZoneInfo.Local);
            
            RecurringJob.AddOrUpdate<NotificationClusterUpdateJob>(
                "notification-cluster-update",
                job => job.Run(JobCancellationToken.Null),
                Cron.Weekly(DayOfWeek.Sunday, 5),
                TimeZoneInfo.Local);
        }
    }
}
