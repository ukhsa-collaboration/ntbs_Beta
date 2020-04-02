using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.DataAccess
{
    public class NtbsContextDesignTimeFactory : IDesignTimeDbContextFactory<NtbsContext>
    {
        private readonly IConfiguration Configuration;

        public NtbsContextDesignTimeFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public NtbsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NtbsContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ntbsMigratorContext"));

            return new NtbsContext(optionsBuilder.Options);
        }
    }
}
