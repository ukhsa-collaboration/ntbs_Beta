using System;
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

        public virtual DbSet<DrugResistence> DrugResistence { get; set; }
        public virtual DbSet<Episode> Episode { get; set; }
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<ResistentDrug> ResistentDrug { get; set; }
        public virtual DbSet<Sex> Sex { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=ntbsContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<DrugResistence>(entity =>
            {
                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.DrugResistence)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DrugResistence_Notification");

                entity.HasOne(d => d.ResistentDrug)
                    .WithMany(p => p.DrugResistence)
                    .HasForeignKey(d => d.ResistentDrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResistentDrug_Notification");
            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.Episode)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Episode_Notification");
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.HospitalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Hospital");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Patient");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.Forename).HasMaxLength(200);

                entity.Property(e => e.NhsNumber).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(200);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Region");

                entity.HasOne(d => d.Sex)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.SexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Sex");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionId).ValueGeneratedOnAdd();

                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<ResistentDrug>(entity =>
            {
                entity.Property(e => e.ResistentDrugId).ValueGeneratedOnAdd();

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.Property(e => e.SexId).ValueGeneratedOnAdd();

                entity.Property(e => e.Label).HasMaxLength(200);
            });
        }
    }
}
