using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using Xunit;

namespace ntbs_service_unit_tests.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HomePageFixture : IAsyncLifetime
    {
        public NtbsContext Context;

        public async Task InitializeAsync()
        {
            var contextOptions = new DbContextOptionsBuilder<NtbsContext>()
                .UseInMemoryDatabase(nameof(HomePageTests))
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
