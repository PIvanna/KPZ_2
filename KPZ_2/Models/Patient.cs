using System;
using System.Collections.Generic;

namespace MedTeleHelp.WPF.Models
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        
        public List<DoctorPatient> DoctorPatients { get; set; } = new();
    }
}