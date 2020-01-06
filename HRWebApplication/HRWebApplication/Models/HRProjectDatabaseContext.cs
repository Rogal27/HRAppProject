using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace HRWebApplication.Models
{
    public partial class HRProjectDatabaseContext : DbContext
    {
        public HRProjectDatabaseContext()
        {
        }

        public HRProjectDatabaseContext(DbContextOptions<HRProjectDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationAttachment> ApplicationAttachment { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<ApplicationStatus> ApplicationStatus { get; set; }
        public virtual DbSet<Attachments> Attachments { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<CV> Cv { get; set; }
        public virtual DbSet<JobOffers> JobOffers { get; set; }
        public virtual DbSet<JobOfferStatus> JobOfferStatus { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationAttachment>(entity =>
            {
                entity.HasIndex(e => new { e.ApplicationId, e.AttachmentId })
                    .HasName("idx_application_attachment");

                entity.Property(e => e.ApplicationAttachmentId).HasColumnName("ApplicationAttachmentID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationAttachment)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationAttachment_Applications");

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.ApplicationAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationAttachment_Attachments");
            });

            modelBuilder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);

                entity.HasIndex(e => e.ApplicationStatusId)
                    .HasName("idx_status");

                entity.HasIndex(e => e.JobOfferId)
                    .HasName("idx_joboffer");

                entity.HasIndex(e => new { e.JobOfferId, e.FirstName, e.LastName })
                    .HasName("idx_joboffer_names");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.ApplicationStatusId).HasColumnName("ApplicationStatusID");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Cvid).HasColumnName("CVID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobOfferId).HasColumnName("JobOfferID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.ApplicationStatus)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.ApplicationStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Applications_ApplicationStatus");

                entity.HasOne(d => d.Cv)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Cvid)
                    .HasConstraintName("FK_Applications_CV");

                entity.HasOne(d => d.JobOffer)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.JobOfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Applications_JobOffer");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Applications_Users");
            });

            modelBuilder.Entity<ApplicationStatus>(entity =>
            {
                entity.Property(e => e.ApplicationStatusId).HasColumnName("ApplicationStatusID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Attachments>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.HasIndex(e => e.CompanyName)
                    .HasName("idx_name");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("text");
            });

            modelBuilder.Entity<CV>(entity =>
            {
                entity.ToTable("CV");

                entity.Property(e => e.CVID).HasColumnName("CVID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobOffers>(entity =>
            {
                entity.HasKey(e => e.JobOfferId);

                entity.HasIndex(e => e.CreationDate)
                    .HasName("idx_date");

                entity.HasIndex(e => e.JobOfferStatusId)
                    .HasName("idx_status");

                entity.HasIndex(e => e.JobTitle)
                    .HasName("idx_title");

                entity.HasIndex(e => e.Location)
                    .HasName("idx_location");

                entity.HasIndex(e => e.ValidUntil)
                    .HasName("idx_expirationdate");

                entity.Property(e => e.JobOfferId).HasColumnName("JobOfferID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.JobOfferStatusId).HasColumnName("JobOfferStatusID");

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValidUntil).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobOffers_Companies");

                entity.HasOne(d => d.JobOfferStatus)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.JobOfferStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobOffers_JobOfferStatus");
            });

            modelBuilder.Entity<JobOfferStatus>(entity =>
            {
                entity.Property(e => e.JobOfferStatusId).HasColumnName("JobOfferStatusID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email)
                    .HasName("idx_email");

                entity.HasIndex(e => e.FirstName)
                    .HasName("idx_fname");

                entity.HasIndex(e => e.LastName)
                    .HasName("idx_lname");

                entity.HasIndex(e => new { e.FirstName, e.LastName })
                    .HasName("idx_names");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserRoles");
            });
        }
    }
}
