using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Text.Json;
using System.Threading.Tasks;
using MedTeleHelp.WPF.DTOs;
using MedTeleHelp.WPF.Mappers;
using MedTeleHelp.WPF.Models;

namespace MedTeleHelp.WPF.Services
{
    public class FileDataService
    {
        private readonly string _filePath;

        public FileDataService(string filePath) { _filePath = filePath; }

        public async Task<(List<Doctor> Doctors, List<Appointment> Appointments)> LoadDataAsync()
        {
            if (!File.Exists(_filePath))
            {
                return (new List<Doctor>(), new List<Appointment>());
            }

            var json = await File.ReadAllTextAsync(_filePath);
            var dto = JsonSerializer.Deserialize<DataDto>(json);

            var doctors = dto?.Doctors?.Select(ModelMapper.ToModel).ToList() ?? new List<Doctor>();
            var appointments = dto?.Appointments?.Select(ModelMapper.ToModel).ToList() ?? new List<Appointment>();

            return (doctors, appointments);
        }

        public async Task SaveDataAsync(IEnumerable<Doctor> doctors, IEnumerable<Appointment> appointments)
        {
            var doctorsDto = doctors.Select(ModelMapper.ToDto).ToList();
            var appointmentsDto = appointments.Select(ModelMapper.ToDto).ToList();

            var dataToSave = new DataDto
            {
                Doctors = doctorsDto,
                Appointments = appointmentsDto
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(dataToSave, options);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}