using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HCP.SMS.DL.Entities
{
    public partial class eSyaEnterprise : DbContext
    {
        public static string _connString = "";

        public eSyaEnterprise()
        {
        }

        public eSyaEnterprise(DbContextOptions<eSyaEnterprise> options)
            : base(options)
        {
        }

        public virtual DbSet<GtEcbsln> GtEcbsln { get; set; }
        public virtual DbSet<GtEcfmfd> GtEcfmfd { get; set; }
        public virtual DbSet<GtEcsmsd> GtEcsmsd { get; set; }
        public virtual DbSet<GtEcsmsh> GtEcsmsh { get; set; }
        public virtual DbSet<GtEcsmsr> GtEcsmsr { get; set; }
        public virtual DbSet<GtEcsmst> GtEcsmst { get; set; }
        public virtual DbSet<GtEcsmsv> GtEcsmsv { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(_connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<GtEcbsln>(entity =>
            {
                entity.HasKey(e => new { e.BusinessId, e.SegmentId, e.LocationId });

                entity.ToTable("GT_ECBSLN");

                entity.HasIndex(e => e.BusinessKey)
                    .HasName("IX_GT_ECBSLN")
                    .IsUnique();

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.SegmentId).HasColumnName("SegmentID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EActiveUsers)
                    .IsRequired()
                    .HasColumnName("eActiveUsers");

                entity.Property(e => e.EBusinessKey)
                    .IsRequired()
                    .HasColumnName("eBusinessKey");

                entity.Property(e => e.ENoOfBeds).HasColumnName("eNoOfBeds");

                entity.Property(e => e.ESyaLicenseType)
                    .IsRequired()
                    .HasColumnName("eSyaLicenseType")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EUserLicenses)
                    .IsRequired()
                    .HasColumnName("eUserLicenses");

                entity.Property(e => e.FormId)
                    .IsRequired()
                    .HasColumnName("FormID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.LocationDescription)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.TocurrConversion).HasColumnName("TOCurrConversion");

                entity.Property(e => e.TolocalCurrency)
                    .IsRequired()
                    .HasColumnName("TOLocalCurrency")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TorealCurrency).HasColumnName("TORealCurrency");
            });

            modelBuilder.Entity<GtEcfmfd>(entity =>
            {
                entity.HasKey(e => e.FormId);

                entity.ToTable("GT_ECFMFD");

                entity.Property(e => e.FormId)
                    .HasColumnName("FormID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ControllerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FormCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FormName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ToolTip).HasMaxLength(100);
            });

            modelBuilder.Entity<GtEcsmsd>(entity =>
            {
                entity.HasKey(e => new { e.Smsid, e.ParameterId });

                entity.ToTable("GT_ECSMSD");

                entity.Property(e => e.Smsid)
                    .HasColumnName("SMSID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.HasOne(d => d.Sms)
                    .WithMany(p => p.GtEcsmsd)
                    .HasForeignKey(d => d.Smsid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECSMSD_GT_ECSMSH");
            });

            modelBuilder.Entity<GtEcsmsh>(entity =>
            {
                entity.HasKey(e => e.Smsid);

                entity.ToTable("GT_ECSMSH");

                entity.Property(e => e.Smsid)
                    .HasColumnName("SMSID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.Smsdescription)
                    .IsRequired()
                    .HasColumnName("SMSDescription")
                    .HasMaxLength(100);

                entity.Property(e => e.Smsstatement)
                    .IsRequired()
                    .HasColumnName("SMSStatement")
                    .HasMaxLength(500);

                entity.Property(e => e.TeventId).HasColumnName("TEventID");
            });

            modelBuilder.Entity<GtEcsmsr>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.Smsid, e.MobileNumber });

                entity.ToTable("GT_ECSMSR");

                entity.Property(e => e.Smsid)
                    .HasColumnName("SMSID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.RecipientName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Remarks).HasMaxLength(25);

                entity.HasOne(d => d.Sms)
                    .WithMany(p => p.GtEcsmsr)
                    .HasForeignKey(d => d.Smsid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECSMSR_GT_ECSMSH");
            });

            modelBuilder.Entity<GtEcsmst>(entity =>
            {
                entity.HasKey(e => e.TeventId);

                entity.ToTable("GT_ECSMST");

                entity.Property(e => e.TeventId)
                    .HasColumnName("TEventID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.TeventDesc)
                    .IsRequired()
                    .HasColumnName("TEventDesc")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<GtEcsmsv>(entity =>
            {
                entity.HasKey(e => e.Smsvariable);

                entity.ToTable("GT_ECSMSV");

                entity.Property(e => e.Smsvariable)
                    .HasColumnName("SMSVariable")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.Smscomponent)
                    .IsRequired()
                    .HasColumnName("SMSComponent")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
