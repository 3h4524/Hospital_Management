using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class HospitalManagementContext : DbContext
{
    public HospitalManagementContext()
    {
    }

    public HospitalManagementContext(DbContextOptions<HospitalManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<RewardPenalty> RewardPenalties { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }

    public virtual DbSet<Timekeeping> Timekeepings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=3H;Initial Catalog=Hospital_Management;User ID=sa;Password=3H452004hh@;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2702A82D4");

            entity.HasIndex(e => new { e.DoctorId, e.AppointmentDate, e.TimeSlot }, "UQ_Doctor_AppointmentSlot").IsUnique();

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Scheduled");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Patient");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C037C1D817043");

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.ReceiverRole).HasMaxLength(20);
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SenderRole).HasMaxLength(20);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDFBCBE923A");

            entity.HasIndex(e => e.Email, "UQ__Doctors__A9D105342B3E0EC2").IsUnique();

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.HashPassword).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Specialization).HasMaxLength(100);
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__MedicalR__FBDF78C9D1618AA7");

            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Diagnosis).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Prescription).HasMaxLength(500);

            entity.HasOne(d => d.Appointment).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Record_Appointment");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC346EF6A02F7");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Patients__85FB4E38AC6E894C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Patients__A9D105346B5C23B2").IsUnique();

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.HashPassword).HasMaxLength(255);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RewardPenalty>(entity =>
        {
            entity.HasKey(e => e.RpId).HasName("PK__RewardPe__850C7721D17070B8");

            entity.ToTable("RewardPenalty");

            entity.Property(e => e.RpId).HasColumnName("RP_ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.Rpdate).HasColumnName("RPDate");
            entity.Property(e => e.SystemUserId).HasColumnName("SystemUserID");
            entity.Property(e => e.Type).HasMaxLength(10);

            entity.HasOne(d => d.Doctor).WithMany(p => p.RewardPenalties)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK_RewardPenalty_Doctor");

            entity.HasOne(d => d.SystemUser).WithMany(p => p.RewardPenalties)
                .HasForeignKey(d => d.SystemUserId)
                .HasConstraintName("FK_RewardPenalty_SystemUsers");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.SalaryId).HasName("PK__Salary__4BE204B7FDE04EAA");

            entity.ToTable("Salary");

            entity.HasIndex(e => new { e.DoctorId, e.Month, e.Year }, "UQ_Salary_PerMonth").IsUnique();

            entity.Property(e => e.SalaryId).HasColumnName("SalaryID");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.FinalSalary)
                .HasComputedColumnSql("(([BaseSalary]+[TotalReward])-[TotalPenalty])", true)
                .HasColumnType("decimal(20, 2)");
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalPenalty)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalReward)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WorkingDays).HasDefaultValue(0);

            entity.HasOne(d => d.Doctor).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_Doctor");
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.HasKey(e => e.SystemUserId).HasName("PK__SystemUs__8788C275FA44094A");

            entity.HasIndex(e => e.PhoneNumber, "UQ__SystemUs__85FB4E384B3A48B6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__SystemUs__A9D1053402803ABF").IsUnique();

            entity.Property(e => e.SystemUserId).HasColumnName("SystemUserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.HashPassword).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Timekeeping>(entity =>
        {
            entity.HasKey(e => e.TimeKeepingId).HasName("PK__Timekeep__1F0DBAF1BD2A10C0");

            entity.ToTable("Timekeeping");

            entity.Property(e => e.TimeKeepingId).HasColumnName("TimeKeepingID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.SystemUserId).HasColumnName("SystemUserID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Timekeepings)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK_Timekeeping_Doctor");

            entity.HasOne(d => d.SystemUser).WithMany(p => p.Timekeepings)
                .HasForeignKey(d => d.SystemUserId)
                .HasConstraintName("FK_Timekeeping_SystemUsers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
