using AutoMapper;
using MedTeleHelp.API.Models;
using MedTeleHelp.API.ViewModels;

namespace MedTeleHelp.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DoctorCreateVm, Doctor>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())); // Генеруємо ID
            
            CreateMap<Doctor, DoctorCreateVm>();
            
            
            CreateMap<AppointmentCreateVm, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 0));
            
        }
    }
}