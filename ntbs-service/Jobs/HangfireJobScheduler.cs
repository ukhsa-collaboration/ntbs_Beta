using System;
using Hangfire;
using ntbs_service.Models.Entities;

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
            
            RecurringJob.AddOrUpdate<UnmatchedLabResultAlertsJob>(
                "unmatched-lab-result-alerts",
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily(4),
                TimeZoneInfo.Local);
        }
    }
}
