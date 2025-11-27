using Microsoft.EntityFrameworkCore;
using MedTeleHelp.WPF.Data.Entities;

namespace MedTeleHelp.WPF.Data.DbFirst
{
    /// <summary>
    /// DbContext для DB First підходу
    /// Цей контекст створюється з існуючої бази даних
    /// Для генерації використовуйте команду:
    /// dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=TelemedicineDbFirst;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Data/DbFirst -c TelemedicineDbFirstContext
    /// </summary>
    public class TelemedicineDbFirstContext : DbContext
    {
        public DbSet<DoctorEntity> Doctors { get; set; }
        public DbSet<AppointmentEntity> Appointments { get; set; }

        public TelemedicineDbFirstContext()
        {
        }

        public TelemedicineDbFirstContext(DbContextOptions<TelemedicineDbFirstContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Connection string для DB First підходу
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=TelemedicineDbFirst;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                    options => options.EnableSensitiveDataLogging()); // Для SQL Profiler
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфігурація для DB First (якщо потрібно додати додаткові налаштування)
            // За замовчуванням EF використовує структуру з БД
        }
    }
}
