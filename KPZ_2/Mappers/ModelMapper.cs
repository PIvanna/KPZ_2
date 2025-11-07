using Riok.Mapperly.Abstractions;
using MedTeleHelp.WPF.DTOs;
using MedTeleHelp.WPF.Models;
using System.Collections.Generic;

namespace MedTeleHelp.WPF.Mappers
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