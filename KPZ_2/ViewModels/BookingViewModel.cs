using System;
using System.Windows.Input;
using KPZ_2.Models;
using KPZ_2.ViewModels.Base;

namespace KPZ_2.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        public Doctor SelectedDoctor { get; }
        
        private DateTime _selectedDateTime = DateTime.Now.AddDays(1);
        public DateTime SelectedDateTime
        {
            get => _selectedDateTime;
            set => Set(ref _selectedDateTime, value);
        }

        public ICommand ConfirmBookingCommand { get; }
        public ICommand CancelBookingCommand { get; }

        public BookingViewModel(Doctor doctor)
        {
            SelectedDoctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            ConfirmBookingCommand = new RelayCommand(p => CloseWindow(p, true));
            CancelBookingCommand = new RelayCommand(p => CloseWindow(p, false));
        }

        private void CloseWindow(object parameter, bool dialogResult)
        {
            if (parameter is System.Windows.Window window)
            {
                window.DialogResult = dialogResult;
                window.Close();
            }
        }
    }
}