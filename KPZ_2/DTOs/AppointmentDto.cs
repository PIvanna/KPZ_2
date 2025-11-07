using System;
using MedTeleHelp.WPF.Models.Enums;

namespace MedTeleHelp.WPF.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorFullName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}