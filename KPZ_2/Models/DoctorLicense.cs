using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedTeleHelp.WPF.Models
{
    public class DoctorLicense
    {
        [Key]
        public Guid Id { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}