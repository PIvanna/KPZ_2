using System;
using MedTeleHelp.WPF.Models.Enums;
using MedTeleHelp.WPF.ViewModels.Base;

namespace MedTeleHelp.WPF.Models
{
    public class Appointment : ViewModelBase
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        private Guid _doctorId;
        public Guid DoctorId
        {
            get => _doctorId;
            set => Set(ref _doctorId, value);
        }

        private Doctor _doctor;
        public Doctor Doctor
        {
            get => _doctor;
            set => Set(ref _doctor, value);
        }

        private string _doctorFullName;
        public string DoctorFullName
        {
            get => _doctorFullName;
            set => Set(ref _doctorFullName, value);
        }

        private DateTime _appointmentTime;
        public DateTime AppointmentTime
        {
            get => _appointmentTime;
            set => Set(ref _appointmentTime, value);
        }

        private AppointmentStatus _status;
        public AppointmentStatus Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
    }
}