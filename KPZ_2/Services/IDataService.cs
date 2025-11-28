using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedTeleHelp.WPF.Models;

namespace MedTeleHelp.WPF.Services
{
    public interface IDataService
    {
        Task<List<Doctor>> GetAllDoctorsAsync();
        Task AddDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(Guid id);

        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task AddAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Guid id);

        // НОВИЙ МЕТОД: Виклик збереженої процедури
        Task<List<Doctor>> GetDoctorsByRatingProcedure(double minRating);
    }
}