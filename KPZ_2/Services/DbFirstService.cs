using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedTeleHelp.WPF.Data.DbFirst;
using MedTeleHelp.WPF.Data.Entities;
using MedTeleHelp.WPF.Models;
using MedTeleHelp.WPF.Models.Enums;

namespace MedTeleHelp.WPF.Services
{
    /// <summary>
    /// Сервіс для роботи з даними через DB First підхід
    /// </summary>
    public class DbFirstService
    {
        private readonly TelemedicineDbFirstContext _context;

        public DbFirstService()
        {
            _context = new TelemedicineDbFirstContext();
        }

        // CREATE операції
        public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
        {
            var entity = new DoctorEntity
            {
                Id = doctor.Id == Guid.Empty ? Guid.NewGuid() : doctor.Id,
                FullName = doctor.FullName,
                Specialization = doctor.Specialization,
                Rating = doctor.Rating,
                ConsultationFee = doctor.ConsultationFee,
                PhotoUrl = doctor.PhotoUrl
            };

            _context.Doctors.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDoctor(entity);
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            var entity = new AppointmentEntity
            {
                Id = appointment.Id == Guid.Empty ? Guid.NewGuid() : appointment.Id,
                DoctorId = appointment.DoctorId,
                DoctorFullName = appointment.DoctorFullName,
                AppointmentTime = appointment.AppointmentTime,
                AppointmentStatus = appointment.Status
            };

            _context.Appointments.Add(entity);
            await _context.SaveChangesAsync();

            return MapToAppointment(entity);
        }

        // READ операції
        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            var entities = await _context.Doctors.ToListAsync();
            return entities.Select(MapToDoctor).ToList();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(Guid id)
        {
            var entity = await _context.Doctors.FindAsync(id);
            return entity != null ? MapToDoctor(entity) : null;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            var entities = await _context.Appointments
                .Include(a => a.Doctor)
                .ToListAsync();
            return entities.Select(MapToAppointment).ToList();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
        {
            var entity = await _context.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);
            return entity != null ? MapToAppointment(entity) : null;
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
        {
            var entities = await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Doctor)
                .ToListAsync();
            return entities.Select(MapToAppointment).ToList();
        }

        // UPDATE операції
        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            var entity = await _context.Doctors.FindAsync(doctor.Id);
            if (entity == null) return false;

            entity.FullName = doctor.FullName;
            entity.Specialization = doctor.Specialization;
            entity.Rating = doctor.Rating;
            entity.ConsultationFee = doctor.ConsultationFee;
            entity.PhotoUrl = doctor.PhotoUrl;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            var entity = await _context.Appointments.FindAsync(appointment.Id);
            if (entity == null) return false;

            entity.DoctorId = appointment.DoctorId;
            entity.DoctorFullName = appointment.DoctorFullName;
            entity.AppointmentTime = appointment.AppointmentTime;
            entity.AppointmentStatus = appointment.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE операції
        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            var entity = await _context.Doctors.FindAsync(id);
            if (entity == null) return false;

            _context.Doctors.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAppointmentAsync(Guid id)
        {
            var entity = await _context.Appointments.FindAsync(id);
            if (entity == null) return false;

            _context.Appointments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Допоміжні методи маппінгу
        private Doctor MapToDoctor(DoctorEntity entity)
        {
            return new Doctor
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Specialization = entity.Specialization,
                Rating = entity.Rating,
                ConsultationFee = entity.ConsultationFee,
                PhotoUrl = entity.PhotoUrl ?? string.Empty
            };
        }

        private Appointment MapToAppointment(AppointmentEntity entity)
        {
            return new Appointment
            {
                Id = entity.Id,
                DoctorId = entity.DoctorId,
                DoctorFullName = entity.DoctorFullName,
                AppointmentTime = entity.AppointmentTime,
                Status = entity.AppointmentStatus
            };
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
