using Microsoft.EntityFrameworkCore;
using MedTeleHelp.WPF.Data.Entities;

namespace MedTeleHelp.WPF.Data.CodeFirst
{
    public class TelemedicineDbContext : DbContext
    {
        public DbSet<DoctorEntity> Doctors { get; set; }
        public DbSet<AppointmentEntity> Appointments { get; set; }

        public TelemedicineDbContext()
        {
        }

        public TelemedicineDbContext(DbContextOptions<TelemedicineDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Connection string - можна винести в конфігурацію
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=TelemedicineCodeFirst;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                    options => options.EnableSensitiveDataLogging()); // Для SQL Profiler
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Doctor configuration
            modelBuilder.Entity<DoctorEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Specialization).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.ConsultationFee).HasColumnType("decimal(10,2)");
                entity.Property(e => e.PhotoUrl).HasMaxLength(500);
                entity.Property(e => e.Bio).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2");

                // Relationships
                entity.HasMany(e => e.Appointments)
                      .WithOne(e => e.Doctor)
                      .HasForeignKey(e => e.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Appointment configuration
            modelBuilder.Entity<AppointmentEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DoctorFullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AppointmentTime).IsRequired();
                entity.Property(e => e.Status).IsRequired();

                // Index for better query performance
                entity.HasIndex(e => e.DoctorId);
                entity.HasIndex(e => e.AppointmentTime);
            });
        }
    }
}
