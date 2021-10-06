using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.DataAccess
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RepositoryFixture<T> : IAsyncLifetime
    {
        public NtbsContext Context;
        public DbContextOptions<NtbsContext> ContextOptions;

        public async Task InitializeAsync()
        {
            ContextOptions = new DbContextOptionsBuilder<NtbsContext>()
                .UseSqlite($"Filename={typeof(T)}.db")
                .Options;

            Context = new NtbsContext(ContextOptions);
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await Context.DisposeAsync();
        }
    }
}
