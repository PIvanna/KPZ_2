using MedTeleHelp.WPF.ViewModels.Base;

namespace MedTeleHelp.WPF.Models
{
    public class Doctor : ViewModelBase
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => Set(ref _fullName, value);
        }

        private string _specialization;
        public string Specialization
        {
            get => _specialization;
            set => Set(ref _specialization, value);
        }

        private double _rating;
        public double Rating
        {
            get => _rating;
            set => Set(ref _rating, value);
        }
        
        private decimal _consultationFee;
        public decimal ConsultationFee
        {
            get => _consultationFee;
            set => Set(ref _consultationFee, value);
        }

        private string _photoUrl;
        public string PhotoUrl
        {
            get => _photoUrl;
            set => Set(ref _photoUrl, value);
        }
        
        private string _email;
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }
    }
}