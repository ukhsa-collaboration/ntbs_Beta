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

        public virtual DbSet<CohortReview> CohortReview { get; set; }
        public virtual DbSet<DrugResistence> DrugResistence { get; set; }
        public virtual DbSet<Episode> Episode { get; set; }
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<LabObversation> LabObversation { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<ResistentDrug> ResistentDrug { get; set; }
        public virtual DbSet<Sex> Sex { get; set; }
        public virtual DbSet<TreatmentOutcome> TreatmentOutcome { get; set; }

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

            modelBuilder.Entity<CohortReview>(entity =>
            {
                entity.Property(e => e.CscassessedAdult).HasColumnName("CSCAssessedAdult");

                entity.Property(e => e.CscassessedChild).HasColumnName("CSCAssessedChild");

                entity.Property(e => e.CscdiscontinuedLtbiadverseReactionAdult).HasColumnName("CSCDiscontinuedLTBIAdverseReactionAdult");

                entity.Property(e => e.CscdiscontinuedLtbiadverseReactionChild).HasColumnName("CSCDiscontinuedLTBIAdverseReactionChild");

                entity.Property(e => e.CscdiscontinuedLtbideathAdult).HasColumnName("CSCDiscontinuedLTBIDeathAdult");

                entity.Property(e => e.CscdiscontinuedLtbideathChild).HasColumnName("CSCDiscontinuedLTBIDeathChild");

                entity.Property(e => e.CscdiscontinuedLtbimovedAdult).HasColumnName("CSCDiscontinuedLTBIMovedAdult");

                entity.Property(e => e.CscdiscontinuedLtbimovedChild).HasColumnName("CSCDiscontinuedLTBIMovedChild");

                entity.Property(e => e.CscdiscontinuedLtbirefusedAdult).HasColumnName("CSCDiscontinuedLTBIRefusedAdult");

                entity.Property(e => e.CscdiscontinuedLtbirefusedChild).HasColumnName("CSCDiscontinuedLTBIRefusedChild");

                entity.Property(e => e.CscidentifiedAdult).HasColumnName("CSCIdentifiedAdult");

                entity.Property(e => e.CscidentifiedChild).HasColumnName("CSCIdentifiedChild");

                entity.Property(e => e.CscnumCompletedLtbitreamentAdult).HasColumnName("CSCNumCompletedLTBITreamentAdult");

                entity.Property(e => e.CscnumCompletedLtbitreamentChild).HasColumnName("CSCNumCompletedLTBITreamentChild");

                entity.Property(e => e.CscnumStartedLtbitreatmentAdult).HasColumnName("CSCNumStartedLTBITreatmentAdult");

                entity.Property(e => e.CscnumStartedLtbitreatmentChild).HasColumnName("CSCNumStartedLTBITreatmentChild");

                entity.Property(e => e.CscnumWithActiveDiseaseAdult).HasColumnName("CSCNumWithActiveDiseaseAdult");

                entity.Property(e => e.CscnumWithActiveDiseaseChild).HasColumnName("CSCNumWithActiveDiseaseChild");

                entity.Property(e => e.CscnumWithLtbiadult).HasColumnName("CSCNumWithLTBIAdult");

                entity.Property(e => e.CscnumWithLtbichild).HasColumnName("CSCNumWithLTBIChild");

                entity.Property(e => e.CscunderInvestigationAdult).HasColumnName("CSCUnderInvestigationAdult");

                entity.Property(e => e.CscunderInvestigationChild).HasColumnName("CSCUnderInvestigationChild");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.CohortReview)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CohortReview_Notification");
            });

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

            modelBuilder.Entity<LabObversation>(entity =>
            {
                entity.Property(e => e.CxrctatDiagnosis).HasColumnName("CXRCTAtDiagnosis");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.LabObversation)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LabObversation_Notification");
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

            modelBuilder.Entity<TreatmentOutcome>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.TreatmentOutcome)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TreatmentOutcome_Notification");
            });
        }
    }
}
