using System;
using System.Collections.Generic;

namespace KPZ_2.ModelsDbFirst;

public partial class Doctor
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public double Rating { get; set; }

    public decimal ConsultationFee { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public string Email { get; set; } = null!;
}
