using System;
using Microsoft.EntityFrameworkCore;

namespace EFAuditer
{
    public class AuditDatabaseContext : DbContext
    {
        public AuditDatabaseContext() {}
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options) : base(options) {}

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO NTBS-218: Read in connection string from some kind of settings file
                optionsBuilder.UseSqlServer("data source=localhost;initial catalog=ntbsAudit;trusted_connection=true");
            }
        }
    }
}
