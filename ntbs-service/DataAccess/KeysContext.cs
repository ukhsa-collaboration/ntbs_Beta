using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Implemented following
// https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers?view=aspnetcore-2.2&tabs=netcore-cli#entity-framework-core
namespace ntbs_service.DataAccess
{
    public class KeysContext : DbContext, IDataProtectionKeyContext
    {
        public KeysContext(DbContextOptions<KeysContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("DataProtectionKeys");
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}
