using System;

namespace MedTeleHelp.WPF.DTOs
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public double Rating { get; set; }
        public decimal ConsultationFee { get; set; }
        public string PhotoUrl { get; set; }
    }
}