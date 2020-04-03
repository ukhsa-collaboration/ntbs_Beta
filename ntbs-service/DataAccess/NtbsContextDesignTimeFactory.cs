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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            var optionsBuilder = new DbContextOptionsBuilder<NtbsContext>();
            var connectionString = configuration.GetConnectionString("ntbsMigratorContext");
            Console.WriteLine(connectionString);
            optionsBuilder.UseSqlServer(connectionString);

            return new NtbsContext(optionsBuilder.Options);
        }
    }
}
