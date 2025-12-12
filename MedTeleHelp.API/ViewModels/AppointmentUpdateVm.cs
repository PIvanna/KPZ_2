using System.ComponentModel.DataAnnotations;
using MedTeleHelp.API.Enums;

namespace MedTeleHelp.API.ViewModels
{
    public class AppointmentUpdateVm
    {
        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public DateTime AppointmentTime { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }
    }
}
