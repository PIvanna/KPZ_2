using System.ComponentModel.DataAnnotations;

namespace MedTeleHelp.API.ViewModels
{
    public class AppointmentCreateVm
    {
        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public DateTime AppointmentTime { get; set; }
    }
}