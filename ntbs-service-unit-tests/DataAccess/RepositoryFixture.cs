using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using Xunit;

namespace ntbs_service_unit_tests.DataAccess
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RepositoryFixture<T> : IAsyncLifetime
    {
        public NtbsContext Context;

        public async Task InitializeAsync()
        {
            var contextOptions = new DbContextOptionsBuilder<NtbsContext>()
                .UseSqlite($"Filename={typeof(T)}.db")
                .Options;

            Context = new NtbsContext(contextOptions);
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await Context.DisposeAsync();
        }
    }
}
