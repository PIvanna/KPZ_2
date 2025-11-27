using System;
using System.Collections.Generic;

namespace KPZ_2.ModelsDbFirst;

public partial class Appointment
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public string DoctorFullName { get; set; } = null!;

    public DateTime AppointmentTime { get; set; }

    public int Status { get; set; }
}
