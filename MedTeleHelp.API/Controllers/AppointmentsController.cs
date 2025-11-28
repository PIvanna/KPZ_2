using AutoMapper;
using MedTeleHelp.API.Data;
using MedTeleHelp.API.Models;
using MedTeleHelp.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTeleHelp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Appointments
        /// <summary>
        /// Отримати всі записи на прийом
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointments/5
        /// <summary>
        /// Отримати конкретний запис за ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST: api/Appointments
        /// <summary>
        /// Створити новий запис до лікаря
        /// </summary>
        /// <remarks>
        /// Потрібно передати ID лікаря та час. Ім'я лікаря підтягнеться автоматично.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentCreateVm appointmentVm)
        {
            var doctor = await _context.Doctors.FindAsync(appointmentVm.DoctorId);
            
            if (doctor == null)
            {
                return BadRequest("Лікаря з таким ID не знайдено.");
            }

            var appointment = _mapper.Map<Appointment>(appointmentVm);

            appointment.DoctorFullName = doctor.FullName;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // DELETE: api/Appointments/5
        /// <summary>
        /// Скасувати (видалити) запис
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}