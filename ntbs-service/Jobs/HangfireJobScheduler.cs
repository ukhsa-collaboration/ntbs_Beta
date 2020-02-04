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
        }
    }
}
