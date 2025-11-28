using Microsoft.EntityFrameworkCore;
using MedTeleHelp.WPF.Models;
using MedTeleHelp.WPF.Models.Enums;
using System;

namespace MedTeleHelp.WPF.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorLicense> DoctorLicenses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
        public DbSet<AppointmentStatusLookup> AppointmentStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=MedTeleHelpDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.License)
                .WithOne(l => l.Doctor)
                .HasForeignKey<DoctorLicense>(l => l.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);
            
            modelBuilder.Entity<DoctorPatient>()
                .HasKey(dp => new { dp.DoctorId, dp.PatientId });

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Doctor)
                .WithMany(d => d.DoctorPatients)
                .HasForeignKey(dp => dp.DoctorId);

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Patient)
                .WithMany(p => p.DoctorPatients)
                .HasForeignKey(dp => dp.PatientId);

            modelBuilder.Entity<AppointmentStatusLookup>().HasData(
                new AppointmentStatusLookup { Id = (int)AppointmentStatus.Scheduled, StatusName = "Scheduled" },
                new AppointmentStatusLookup { Id = (int)AppointmentStatus.Completed, StatusName = "Completed" },
                new AppointmentStatusLookup { Id = (int)AppointmentStatus.Cancelled, StatusName = "Cancelled" }
            );
        }
    }
}