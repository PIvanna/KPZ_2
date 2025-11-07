using KPZ_2.DTOs;
using KPZ_2.Models;
using Riok.Mapperly.Abstractions;

namespace KPZ_2.Mappers
{
    [Mapper]
    public static partial class ModelMapper
    {
        public static partial DoctorDto ToDto(Doctor doctor);

        public static partial AppointmentDto ToDto(Appointment appointment);

        public static partial Doctor ToModel(DoctorDto doctorDto);
        
        public static partial Appointment ToModel(AppointmentDto appointmentDto);
    }
}