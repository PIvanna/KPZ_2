using System.ComponentModel.DataAnnotations;

namespace MedTeleHelp.API.ViewModels
{
    public class DoctorCreateVm
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Range(0, 5, ErrorMessage = "Рейтинг від 0 до 5")]
        public double Rating { get; set; }

        [Range(0, 10000)]
        public decimal ConsultationFee { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        
        public string? PhotoUrl { get; set; }
    }
}