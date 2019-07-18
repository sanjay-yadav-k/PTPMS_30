using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Datalayer.Models
{
    public partial class OnlineclassContext : DbContext
    {
        public OnlineclassContext()
        {
        }

        public OnlineclassContext(DbContextOptions<OnlineclassContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblBatch> TblBatch { get; set; }
        public virtual DbSet<Tblchepter> Tblchepter { get; set; }
        public virtual DbSet<TblClass> TblClass { get; set; }
        public virtual DbSet<TblClassSubjectMap> TblClassSubjectMap { get; set; }
        public virtual DbSet<TblDaytype> TblDaytype { get; set; }
        public virtual DbSet<TblDuration> TblDuration { get; set; }
        public virtual DbSet<TblEmailSend> TblEmailSend { get; set; }
        public virtual DbSet<Tbllogin> Tbllogin { get; set; }
        public virtual DbSet<Tblmapbatchtime> Tblmapbatchtime { get; set; }
        public virtual DbSet<Tblmapteacherammount> Tblmapteacherammount { get; set; }
        public virtual DbSet<Tblfeessetting> Tblfeessetting { get; set; }
        public virtual DbSet<Tblmapstudentduration> Tblmapstudentduration { get; set; }
        public virtual DbSet<Tblmapsubjecttime> Tblmapsubjecttime { get; set; }
        public virtual DbSet<Tblscheduleclassbatch> Tblscheduleclassbatch { get; set; }
        public virtual DbSet<TblStudent> TblStudent { get; set; }
        public virtual DbSet<TblstudentSubjectBatch> TblstudentSubjectBatch { get; set; }
        public virtual DbSet<Tblstudentteacher> Tblstudentteacher { get; set; }
        public virtual DbSet<Tblsubject> Tblsubject { get; set; }
        public virtual DbSet<Tblsubjectbatchmap> Tblsubjectbatchmap { get; set; }
        public virtual DbSet<Tblteacher> Tblteacher { get; set; }
        public virtual DbSet<Tblteacherclassbatchmap> Tblteacherclassbatchmap { get; set; }
        public virtual DbSet<Tblteachertopay> Tblteachertopay { get; set; }
        public virtual DbSet<TblTempTransaction> TblTempTransaction { get; set; }
        public virtual DbSet<TblTransaction> TblTransaction { get; set; }
        public virtual DbSet<Tbltype> Tbltype { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=WINSERVER;Initial Catalog=Onlineclass;User ID=sa;Password=karmick@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblBatch>(entity =>
            {
                entity.HasKey(e => e.BatchId);

                entity.ToTable("tblBatch");

                entity.Property(e => e.BatchName).HasMaxLength(50);

                entity.Property(e => e.Dateofcommencement).HasColumnType("datetime");

                entity.Property(e => e.DayId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<TblClass>(entity =>
            {
                entity.HasKey(e => e.ClassId);

                entity.ToTable("tblClass");

                entity.Property(e => e.ClassName).HasMaxLength(50);
            });
            modelBuilder.Entity<Tblchepter>(entity =>
            {
                entity.HasKey(e => e.ChepterId);

                entity.ToTable("tblchepter");

                entity.Property(e => e.ChepterId).HasColumnName("Chepter_id");

                entity.Property(e => e.ChepterName).IsUnicode(false);

                entity.Property(e => e.Syllabus)
                    .HasColumnName("syllabus")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblClassSubjectMap>(entity =>
            {
                entity.HasKey(e => e.ClassSubjectId);

                entity.ToTable("tblClassSubjectMap");

                entity.Property(e => e.ClassSubjectId).HasColumnName("classSubjectId");
            });

         
            modelBuilder.Entity<TblDaytype>(entity =>
            {
                entity.HasKey(e => e.DaytypeId);

                entity.ToTable("tblDaytype");

                entity.Property(e => e.Day)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDuration>(entity =>
            {
                entity.HasKey(e => e.Durationid);

                entity.Property(e => e.Durationname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEmailSend>(entity =>
            {
                entity.HasKey(e => e.MailId);

                entity.ToTable("tblEmailSend");

                entity.Property(e => e.RecieverMailId)
                    .HasColumnName("RecieverMail_id")
                    .HasMaxLength(100);

                entity.Property(e => e.SendDateTime).HasColumnType("datetime");

                entity.Property(e => e.SenderMailId)
                    .HasColumnName("SenderMail_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Subject).HasMaxLength(100);
            });
            modelBuilder.Entity<Tblfeessetting>(entity =>
            {
                entity.HasKey(e => e.Feeid);

                entity.ToTable("tblfeessetting");

                entity.Property(e => e.Ammount).HasColumnType("decimal(18, 0)");
            });
            modelBuilder.Entity<Tbllogin>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserName).IsUnicode(false);
            });

            modelBuilder.Entity<Tblmapbatchtime>(entity =>
            {
                entity.HasKey(e => e.Batchtimemapid);

                entity.ToTable("tblmapbatchtime");
            });
            modelBuilder.Entity<Tblmapteacherammount>(entity =>
            {
                entity.HasKey(e => e.TecherammountId);

                entity.ToTable("tblmapteacherammount");

                entity.Property(e => e.AmountTopay).HasColumnType("decimal(18, 0)");
            });
            modelBuilder.Entity<Tblmapstudentduration>(entity =>
            {
                entity.ToTable("tblmapstudentduration");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Period).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Tblmapsubjecttime>(entity =>
            {
                entity.HasKey(e => e.Subjecttimemapid);

                entity.ToTable("tblmapsubjecttime");

                entity.Property(e => e.Subjecttimemapid).HasColumnName("subjecttimemapid");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            });

            modelBuilder.Entity<Tblscheduleclassbatch>(entity =>
            {
                entity.HasKey(e => e.Classdatemapid);

                entity.ToTable("tblscheduleclassbatch");

                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.IsCopletedClass).HasColumnName("Is_CopletedClass");

                entity.Property(e => e.IsStartClass).HasColumnName("Is_StartClass");
            });

            modelBuilder.Entity<TblStudent>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.ToTable("tblStudent");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");
                entity.Property(e => e.Email)
                   .HasMaxLength(500)
                   .IsUnicode(false);

                entity.Property(e => e.Fname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo).HasColumnType("numeric(10, 0)");

                entity.Property(e => e.ParentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Skype)
                   .HasMaxLength(500)
                   .IsUnicode(false);
                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zipcode).HasColumnType("numeric(10, 0)");
            });

            modelBuilder.Entity<TblstudentSubjectBatch>(entity =>
            {
                entity.ToTable("tblstudent_subject_batch");
            });

            modelBuilder.Entity<Tblstudentteacher>(entity =>
            {
                entity.HasKey(e => e.Teacherstudentmapid);

                entity.ToTable("tblstudentteacher");

                entity.Property(e => e.Ammount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Date).HasColumnType("datetime");

            });

            modelBuilder.Entity<Tblsubject>(entity =>
            {
                entity.HasKey(e => e.Subjectid);

                entity.ToTable("tblsubject");
                entity.Property(e => e.DayId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subjectname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Tblsubjectbatchmap>(entity =>
            {
                entity.HasKey(e => e.BatchmapId);

                entity.ToTable("tblsubjectbatchmap");
            });

            modelBuilder.Entity<Tblteacher>(entity =>
            {
                entity.HasKey(e => e.TeacherId);

                entity.ToTable("tblteacher");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Fname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tblteacherclassbatchmap>(entity =>
            {
                entity.HasKey(e => e.Techermapid);

                entity.ToTable("tblteacherclassbatchmap");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
               
            });
            modelBuilder.Entity<Tblteachertopay>(entity =>
            {
                entity.HasKey(e => e.TeacherpayId);

                entity.ToTable("tblteachertopay");

                entity.Property(e => e.Ammount).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<TblTempTransaction>(entity =>
            {
                entity.HasKey(e => e.TempTransactionId);

                entity.ToTable("tblTempTransaction");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DateOfTransaction).HasColumnType("datetime");

                entity.Property(e => e.IsSubject).HasColumnName("IsSubject");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("Total_Amount")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<TblTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("tblTransaction");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
            });
            modelBuilder.Entity<Tbltype>(entity =>
            {
                entity.HasKey(e => e.Typeid);

                entity.ToTable("tbltype");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
