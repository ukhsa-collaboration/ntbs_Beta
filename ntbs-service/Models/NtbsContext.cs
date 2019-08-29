using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ntbs_service.Models
{
    public partial class NtbsContext : DbContext
    {
        public NtbsContext()
        {
        }

        public NtbsContext(DbContextOptions<NtbsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Ethnicity> Ethnicity { get; set; }
        public virtual DbSet<ClinicalTimeline> ClinicalTimelines { get; set; }
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Sex> Sex { get; set; }

        public virtual async Task<IList<Country>> GetAllCountriesAsync()
        {
            return await Country.ToListAsync();
        }

        public virtual async Task<Country> GetCountryByIdAsync(int? countryId)
        {
            return await Country.FindAsync(countryId);
        }

        public virtual async Task<IList<Sex>> GetAllSexesAsync()
        {
            return await Sex.ToListAsync();
        }

        public virtual async Task<IList<Ethnicity>> GetAllEthnicitiesAsync()
        {
            return await Ethnicity.ToListAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("name=ntbsContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Country>().HasData(
                Countries.GetCountriesArray()
            );

            modelBuilder.Entity<Ethnicity>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<Ethnicity>().HasData(
                new Ethnicity { EthnicityId = 1, Code = "A", Label = "White British", Order = 16 },
                new Ethnicity { EthnicityId = 2, Code = "B", Label = "White Irish", Order = 17 },
                new Ethnicity { EthnicityId = 3, Code = "C", Label = "Any other White background", Order = 3 },
                new Ethnicity { EthnicityId = 4, Code = "D", Label = "Mixed - White and Black Caribbean", Order = 14 },
                new Ethnicity { EthnicityId = 5, Code = "E", Label = "Mixed - White and Black African", Order = 13 },
                new Ethnicity { EthnicityId = 6, Code = "F", Label = "Mixed - White and Asian", Order = 12 },
                new Ethnicity { EthnicityId = 7, Code = "G", Label = "Any other mixed background", Order = 9 },
                new Ethnicity { EthnicityId = 8, Code = "H", Label = "Indian", Order = 1 },
                new Ethnicity { EthnicityId = 9, Code = "J", Label = "Pakistani", Order = 2 },
                new Ethnicity { EthnicityId = 10, Code = "K", Label = "Bangladeshi", Order = 10 },
                new Ethnicity { EthnicityId = 11, Code = "L", Label = "Any other Asian background", Order = 6 },
                new Ethnicity { EthnicityId = 12, Code = "M", Label = "Black Caribbean", Order = 11 },
                new Ethnicity { EthnicityId = 13, Code = "N", Label = "Black African", Order = 5 },
                new Ethnicity { EthnicityId = 14, Code = "P", Label = "Any other Black Background", Order = 7 },
                new Ethnicity { EthnicityId = 15, Code = "S", Label = "Any other ethnic group", Order = 8 },
                new Ethnicity { EthnicityId = 16, Code = "R", Label = "Chinese", Order = 4 },
                new Ethnicity { EthnicityId = 17, Code = "Z", Label = "Not stated", Order = 15 }
            );

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.Hospital)
                    .WithMany()
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Hospital");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Notifications)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Patient");

                entity.OwnsOne(e => e.ClinicalTimeline).ToTable("ClinicalTimelines");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.GivenName).HasMaxLength(35);

                entity.Property(e => e.NhsNumber).HasMaxLength(10);

                entity.Property(e => e.FamilyName).HasMaxLength(35);

                entity.Property(e => e.Postcode).HasMaxLength(50);

                entity.HasOne(d => d.Ethnicity)
                    .WithMany()
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Ethnicity");

                entity.HasOne(d => d.Country)
                    .WithMany()
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Country");

                entity.HasOne(d => d.Sex)
                    .WithMany()
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Sex");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.Label).HasMaxLength(200);
            });


            modelBuilder.Entity<Sex>(entity =>
            {
                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<Sex>().HasData(
                new Sex { SexId = 1, Label = "Male" },
                new Sex { SexId = 2, Label = "Female" },
                new Sex { SexId = 3, Label = "Unknown" }
            );
        }
    }
}
