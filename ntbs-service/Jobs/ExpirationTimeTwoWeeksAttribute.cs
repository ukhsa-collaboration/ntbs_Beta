using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace ntbs_service.Jobs
{
    public class ExpirationTimeTwoWeeksAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(14);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // no op
        }
    }
}
