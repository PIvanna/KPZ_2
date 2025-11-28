using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MedTeleHelp.WPF.Models;
using MedTeleHelp.WPF.Models.Enums;
using Microsoft.Data.SqlClient;

namespace MedTeleHelp.WPF.Services
{
    public class AdoNetDataService : IDataService
    {
        private readonly string _connectionString = "Server=MSI\\SQLEXPRESS;Database=MedTeleHelpDb;Trusted_Connection=True;TrustServerCertificate=True;";

        private Doctor ReadDoctor(SqlDataReader reader)
        {
            return new Doctor
            {
                Id = reader.GetGuid(0),
                FullName = reader.GetString(1),
                Specialization = reader.GetString(2),
                Rating = reader.GetDouble(3),
                ConsultationFee = reader.GetDecimal(4),
                PhotoUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
                Email = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            var list = new List<Doctor>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            
            var cmd = new SqlCommand("SELECT Id, FullName, Specialization, Rating, ConsultationFee, PhotoUrl, Email FROM Doctors", conn);
            
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(ReadDoctor(reader));
            }
            return list;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "INSERT INTO Doctors (Id, FullName, Specialization, Rating, ConsultationFee, PhotoUrl, Email) VALUES (@Id, @Name, @Spec, @Rate, @Fee, @Photo, @Email)";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", doctor.Id);
            cmd.Parameters.AddWithValue("@Name", doctor.FullName);
            cmd.Parameters.AddWithValue("@Spec", doctor.Specialization);
            cmd.Parameters.AddWithValue("@Rate", doctor.Rating);
            cmd.Parameters.AddWithValue("@Fee", doctor.ConsultationFee);
            cmd.Parameters.AddWithValue("@Photo", (object)doctor.PhotoUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)doctor.Email ?? DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "UPDATE Doctors SET FullName=@Name, Specialization=@Spec, Rating=@Rate, ConsultationFee=@Fee, PhotoUrl=@Photo, Email=@Email WHERE Id=@Id";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", doctor.Id);
            cmd.Parameters.AddWithValue("@Name", doctor.FullName);
            cmd.Parameters.AddWithValue("@Spec", doctor.Specialization);
            cmd.Parameters.AddWithValue("@Rate", doctor.Rating);
            cmd.Parameters.AddWithValue("@Fee", doctor.ConsultationFee);
            cmd.Parameters.AddWithValue("@Photo", (object)doctor.PhotoUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)doctor.Email ?? DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteDoctorAsync(Guid id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("DELETE FROM Doctors WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            var list = new List<Appointment>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT Id, DoctorId, DoctorFullName, AppointmentTime, Status FROM Appointments", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Appointment
                {
                    Id = reader.GetGuid(0),
                    DoctorId = reader.GetGuid(1),
                    DoctorFullName = reader.GetString(2),
                    AppointmentTime = reader.GetDateTime(3),
                    Status = (AppointmentStatus)reader.GetInt32(4)
                });
            }
            return list;
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "INSERT INTO Appointments (Id, DoctorId, DoctorFullName, AppointmentTime, Status) VALUES (@Id, @DocId, @DocName, @Time, @Status)";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", appointment.Id);
            cmd.Parameters.AddWithValue("@DocId", appointment.DoctorId);
            cmd.Parameters.AddWithValue("@DocName", appointment.DoctorFullName);
            cmd.Parameters.AddWithValue("@Time", appointment.AppointmentTime);
            cmd.Parameters.AddWithValue("@Status", (int)appointment.Status);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAppointmentAsync(Guid id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("DELETE FROM Appointments WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Doctor>> GetDoctorsByRatingProcedure(double minRating)
        {
            var list = new List<Doctor>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("GetDoctorsByMinRating", conn);
            cmd.CommandType = CommandType.StoredProcedure; 
            cmd.Parameters.AddWithValue("@MinRating", minRating);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(ReadDoctor(reader));
            }
            return list;
        }
    }
}