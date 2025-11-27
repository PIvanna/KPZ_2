using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedTeleHelp.WPF.Models.Enums;

namespace MedTeleHelp.WPF.Data.Entities
{
    [Table("Appointments")]
    public class AppointmentEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        [MaxLength(200)]
        public string DoctorFullName { get; set; } = string.Empty;

        [Required]
        public DateTime AppointmentTime { get; set; }

        [Required]
        public int Status { get; set; } // Stored as int for enum

        // Navigation property
        [ForeignKey("DoctorId")]
        public virtual DoctorEntity? Doctor { get; set; }

        // Helper property for enum conversion
        [NotMapped]
        public AppointmentStatus AppointmentStatus
        {
            get => (AppointmentStatus)Status;
            set => Status = (int)value;
        }
    }
}
