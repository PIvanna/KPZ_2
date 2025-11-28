using MedTeleHelp.API.Enums;

namespace MedTeleHelp.API.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorFullName { get; set; } = string.Empty;
        public DateTime AppointmentTime { get; set; }
        public int Status { get; set; } 
    }
}