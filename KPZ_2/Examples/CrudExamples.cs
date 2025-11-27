using System;
using System.Threading.Tasks;
using MedTeleHelp.WPF.Models;
using MedTeleHelp.WPF.Models.Enums;
using MedTeleHelp.WPF.Services;

namespace MedTeleHelp.WPF.Examples
{
    /// <summary>
    /// Приклади використання CRUD операцій для обох підходів
    /// </summary>
    public class CrudExamples
    {
        /// <summary>
        /// Приклад використання Code First підходу
        /// </summary>
        public static async Task CodeFirstExample()
        {
            var service = new CodeFirstService();

            try
            {
                // CREATE - Створення лікаря
                var doctor = new Doctor
                {
                    Id = Guid.NewGuid(),
                    FullName = "Др. Іван Петренко",
                    Specialization = "Кардіолог",
                    Rating = 4.8,
                    ConsultationFee = 1500.00m,
                    PhotoUrl = "https://example.com/doctor1.jpg"
                };

                var createdDoctor = await service.CreateDoctorAsync(doctor);
                Console.WriteLine($"Створено лікаря: {createdDoctor.FullName}");

                // CREATE - Створення запису
                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    DoctorId = createdDoctor.Id,
                    DoctorFullName = createdDoctor.FullName,
                    AppointmentTime = DateTime.Now.AddDays(7),
                    Status = AppointmentStatus.Scheduled
                };

                var createdAppointment = await service.CreateAppointmentAsync(appointment);
                Console.WriteLine($"Створено запис: {createdAppointment.Id}");

                // READ - Отримання всіх лікарів
                var doctors = await service.GetAllDoctorsAsync();
                Console.WriteLine($"Всього лікарів: {doctors.Count}");

                // READ - Отримання лікаря за ID
                var doctorById = await service.GetDoctorByIdAsync(createdDoctor.Id);
                if (doctorById != null)
                {
                    Console.WriteLine($"Лікар знайдено: {doctorById.FullName}");
                }

                // UPDATE - Оновлення лікаря
                createdDoctor.Rating = 4.9;
                createdDoctor.ConsultationFee = 1600.00m;
                var updated = await service.UpdateDoctorAsync(createdDoctor);
                Console.WriteLine($"Лікар оновлено: {updated}");

                // UPDATE - Оновлення запису
                createdAppointment.Status = AppointmentStatus.Completed;
                var appointmentUpdated = await service.UpdateAppointmentAsync(createdAppointment);
                Console.WriteLine($"Запис оновлено: {appointmentUpdated}");

                // DELETE - Видалення запису
                var deleted = await service.DeleteAppointmentAsync(createdAppointment.Id);
                Console.WriteLine($"Запис видалено: {deleted}");

                // DELETE - Видалення лікаря
                var doctorDeleted = await service.DeleteDoctorAsync(createdDoctor.Id);
                Console.WriteLine($"Лікар видалено: {doctorDeleted}");
            }
            finally
            {
                service.Dispose();
            }
        }

        /// <summary>
        /// Приклад використання DB First підходу
        /// </summary>
        public static async Task DbFirstExample()
        {
            var service = new DbFirstService();

            try
            {
                // CREATE - Створення лікаря
                var doctor = new Doctor
                {
                    Id = Guid.NewGuid(),
                    FullName = "Др. Марія Коваленко",
                    Specialization = "Невролог",
                    Rating = 4.7,
                    ConsultationFee = 1400.00m,
                    PhotoUrl = "https://example.com/doctor2.jpg"
                };

                var createdDoctor = await service.CreateDoctorAsync(doctor);
                Console.WriteLine($"Створено лікаря (DB First): {createdDoctor.FullName}");

                // READ - Отримання всіх записів
                var appointments = await service.GetAllAppointmentsAsync();
                Console.WriteLine($"Всього записів: {appointments.Count}");

                // UPDATE - Оновлення лікаря
                createdDoctor.Specialization = "Невролог-епілептолог";
                await service.UpdateDoctorAsync(createdDoctor);
                Console.WriteLine("Лікар оновлено (DB First)");

                // DELETE - Видалення лікаря
                await service.DeleteDoctorAsync(createdDoctor.Id);
                Console.WriteLine("Лікар видалено (DB First)");
            }
            finally
            {
                service.Dispose();
            }
        }
    }
}
