using System.Collections.Generic;

namespace KPZ_2.DTOs
{
    public class DataDto
    {
        public List<DoctorDto> Doctors { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}