using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.DataAccess
{
    public class NtbsContextDesignTimeFactory : IDesignTimeDbContextFactory<NtbsContext>
    {
        public NtbsContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: false);

            if (environment == "Development") {
                builder.AddUserSecrets<Program>(true);
            }

            builder.AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<NtbsContext>();
            var connectionString = configuration.GetConnectionString("ntbsMigratorContext");
            optionsBuilder.UseSqlServer(connectionString);

            return new NtbsContext(optionsBuilder.Options);
        }
    }
}
