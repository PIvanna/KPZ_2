using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedTeleHelp.WPF.Data.Entities
{
    [Table("Doctors")]
    public class DoctorEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Column(TypeName = "decimal(3,2)")]
        public double Rating { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ConsultationFee { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; } // Додано для демонстрації міграції

        public DateTime? CreatedAt { get; set; } // Додано для демонстрації міграції

        // Navigation property
        public virtual ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
    }
}
