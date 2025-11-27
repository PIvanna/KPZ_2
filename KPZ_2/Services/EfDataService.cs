using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedTeleHelp.WPF.Data;
using MedTeleHelp.WPF.Models;

namespace MedTeleHelp.WPF.Services
{
    public class EfDataService : IDataService
    {
        private readonly AppDbContext _context;

        public EfDataService()
        {
            _context = new AppDbContext();
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync() => 
            await _context.Doctors.AsNoTracking().ToListAsync();

        public async Task AddDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(Guid id)
        {
            var doc = await _context.Doctors.FindAsync(id);
            if (doc != null)
            {
                _context.Doctors.Remove(doc);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync() => 
            await _context.Appointments.AsNoTracking().ToListAsync();

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(Guid id)
        {
            var app = await _context.Appointments.FindAsync(id);
            if (app != null)
            {
                _context.Appointments.Remove(app);
                await _context.SaveChangesAsync();
            }
        }
    }
}