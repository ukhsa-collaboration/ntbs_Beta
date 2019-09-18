using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ntbs_service.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ntbs_service.Models.Enums;
using Audit.EntityFramework;

namespace ntbs_service.Models
{
    public partial class NtbsContext : AuditDbContext
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
        public virtual DbSet<TBService> TBService { get; set;}
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationSite> NotificationSite { get; set; }
        public virtual DbSet<Site> Site { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Sex> Sex { get; set; }
        public virtual DbSet<Episode> Episode { get; set; }
        public virtual DbSet<SocialRiskFactors> SocialRiskFactors { get; set; }

        public virtual async Task<IList<Country>> GetAllCountriesAsync()
        {
            return await Country.ToListAsync();
        }

        public virtual async Task<Country> GetCountryByIdAsync(int? countryId)
        {
            return await Country.FindAsync(countryId);
        }
        
        public virtual async Task<IList<TBService>> GetAllTbServicesAsync()
        {
            return await TBService.ToListAsync();
        }
        
        public virtual async Task<IList<Hospital>> GetAllHospitalsAsync()
        {
            return await Hospital.ToListAsync();
        }

        public virtual async Task<List<Hospital>> GetHospitalsByTBService(string tbServiceCode)
        {
            return await Hospital
                        .Where(x => x.TBServiceCode == tbServiceCode)
                        .ToListAsync();
        }

        public virtual async Task<IList<Sex>> GetAllSexesAsync()
        {
            return await Sex.ToListAsync();
        }

        public virtual async Task<IList<Ethnicity>> GetAllEthnicitiesAsync()
        {
            return await Ethnicity.ToListAsync();
        }

        public virtual async Task<IList<Site>> GetAllSitesAsync()
        {
            return await Site.ToListAsync();
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

            modelBuilder.Entity<TBService>().HasData(GetTBServicesList());

            modelBuilder.Entity<Hospital>().HasData(GetHospitalsList());

            var converter = new EnumToStringConverter<Status>();

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.OwnsOne(e => e.Episode).ToTable("Episode");

                entity.OwnsOne(e => e.PatientDetails).ToTable("Patients");

                entity.OwnsOne(e => e.ClinicalDetails, e => {
                     e.Property(cd => cd.BCGVaccinationState)
                        .HasConversion(converter);
                    e.ToTable("ClinicalDetails");
                });

                entity.OwnsOne(e => e.PatientTBHistory).ToTable("PatientTBHistories");

                entity.OwnsOne(e => e.ContactTracing).ToTable("ContactTracing");

                entity.OwnsOne(e => e.SocialRiskFactors, x => {
                    x.OwnsOne(c => c.RiskFactorDrugs , rf => {
                        rf.Property(e => e.Status).HasConversion(converter);
                        rf.ToTable("RiskFactorDrugs");
                    });

                    x.OwnsOne(c => c.RiskFactorHomelessness, rh => {
                        rh.Property(e => e.Status).HasConversion(converter);
                        rh.ToTable("RiskFactorHomelessness");
                    });

                    x.OwnsOne(c => c.RiskFactorImprisonment, rh => {
                        rh.Property(e => e.Status).HasConversion(converter);
                        rh.ToTable("RiskFactorImprisonment");
                    });

                    x.Property(e => e.AlcoholMisuseStatus).HasConversion(converter);
                    x.Property(e => e.SmokingStatus).HasConversion(converter);
                    x.Property(e => e.MentalHealthStatus).HasConversion(converter);

                    x.ToTable("SocialRiskFactors");
                });
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

            modelBuilder.Entity<NotificationSite>(entity =>
            {
                entity.HasKey(e => new {e.NotificationId, e.SiteId });

                entity.HasOne(e => e.Notification)
                    .WithMany(n => n.NotificationSites)
                    .HasForeignKey(ns => ns.NotificationId);

                entity.HasOne(e => e.Site)
                    .WithMany(s => s.NotificationSites)
                    .HasForeignKey(ns => ns.SiteId);
            });

            modelBuilder.Entity<Site>().HasData(
                new Site { SiteId = (int)SiteId.PULMONARY, Description = "Pulmonary" },
                new Site { SiteId = (int)SiteId.BONE_SPINE, Description = "Bone/joint: spine" },
                new Site { SiteId = (int)SiteId.BONE_OTHER, Description = "Bone/joint: other" },
                new Site { SiteId = (int)SiteId.CNS_MENINGITIS, Description = "meningitis" },
                new Site { SiteId = (int)SiteId.CNS_OTHER, Description = "other" },
                new Site { SiteId = (int)SiteId.OCULAR, Description = "Ocular" },
                new Site { SiteId = (int)SiteId.CRYPTIC, Description = "Cryptic disseminated" },
                new Site { SiteId = (int)SiteId.GASTROINTESTINAL, Description = "Gastrointestinal/peritoneal" },
                new Site { SiteId = (int)SiteId.GENITOURINARY, Description = "Genitourinary" },
                new Site { SiteId = (int)SiteId.LYMPH_INTRA, Description = "Intra-thoracic" },
                new Site { SiteId = (int)SiteId.LYMPH_EXTRA, Description = "Extra-thoracic" },
                new Site { SiteId = (int)SiteId.LARYNGEAL, Description = "Laryngeal" },
                new Site { SiteId = (int)SiteId.MILIARY, Description = "Miliary" },
                new Site { SiteId = (int)SiteId.PLEURAL, Description = "Pleural" },
                new Site { SiteId = (int)SiteId.PERICARDIAL, Description = "Pericardial" },
                new Site { SiteId = (int)SiteId.SKIN, Description = "Soft tissue/Skin" },
                new Site { SiteId = (int)SiteId.OTHER, Description = "Other extra-pulmonary" }
            );
        }

        private List<Object> GetHospitalsList()
        {
            return SeedingHelper.GetHospitalsList("Models\\SeedData\\hospitals.csv");
        }

        private List<TBService> GetTBServicesList()
        {
            return SeedingHelper.GetRecordsFromCSV<TBService>("Models\\SeedData\\tbservices.csv");
        }
    }
}
