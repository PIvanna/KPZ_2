namespace MedTeleHelp.API.Models
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public double Rating { get; set; }
        public decimal ConsultationFee { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Email { get; set; }
    }
}