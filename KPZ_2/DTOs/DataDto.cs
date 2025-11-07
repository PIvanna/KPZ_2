using System.Collections.Generic;

namespace MedTeleHelp.WPF.DTOs
{
    public class DataDto
    {
        public List<DoctorDto> Doctors { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}