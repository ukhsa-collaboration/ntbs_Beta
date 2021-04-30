using System;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;

namespace load_test_data_generation
{
    internal interface IContextProvider
    {
        void WithContext(Action<NtbsContext> action);
        T WithContext<T>(Func<NtbsContext, T> action);
    }

    internal class ContextProvider : IContextProvider
    {
        private readonly string connectionString;

        public ContextProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void WithContext(Action<NtbsContext> action)
        {
            using var context = CreateContext();
            action(context);
        }

        public T WithContext<T>(Func<NtbsContext, T> action)
        {
            using var context = CreateContext();
            return action(context);
        }

        private NtbsContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<NtbsContext>();
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
            return new NtbsContext(options.Options);
        }
    }
}
