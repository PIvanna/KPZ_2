using Microsoft.EntityFrameworkCore;
using MedTeleHelp.WPF.Models;

namespace MedTeleHelp.WPF.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=MedTeleHelpDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasColumnType("decimal(18,2)");
        }
    }
}