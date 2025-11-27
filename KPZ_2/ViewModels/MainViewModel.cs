using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MedTeleHelp.WPF.Models;
using MedTeleHelp.WPF.Models.Enums;
using MedTeleHelp.WPF.Services;
using MedTeleHelp.WPF.ViewModels.Base;
using MedTeleHelp.WPF.Views;

namespace MedTeleHelp.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        
        public MainViewModel()
        {
            _dataService = new AdoNetDataService();
            // _dataService = new EfDataService();

            LoadDataCommand = new RelayCommand(async _ => await LoadData());
            OpenBookingWindowCommand = new RelayCommand(OpenBookingWindow, _ => SelectedDoctor != null);
            
            AddDoctorCommand = new RelayCommand(async _ => await AddDoctor());
            DeleteDoctorCommand = new RelayCommand(async _ => await DeleteDoctor(), _ => SelectedDoctor != null);
            DeleteAppointmentCommand = new RelayCommand(async p => await DeleteAppointment(p));

            LoadDataCommand.Execute(null);
        }

        public ObservableCollection<Doctor> Doctors { get; } = new ObservableCollection<Doctor>();
        public ObservableCollection<Appointment> Appointments { get; } = new ObservableCollection<Appointment>();

        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set => Set(ref _selectedDoctor, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand OpenBookingWindowCommand { get; }
        public ICommand AddDoctorCommand { get; }
        public ICommand DeleteDoctorCommand { get; }
        public ICommand DeleteAppointmentCommand { get; }

        private async Task LoadData()
        {
            Doctors.Clear();
            Appointments.Clear();

            var loadedDoctors = await _dataService.GetAllDoctorsAsync();
            var loadedAppointments = await _dataService.GetAllAppointmentsAsync();

            if (!loadedDoctors.Any())
            {
                await SeedData();
                loadedDoctors = await _dataService.GetAllDoctorsAsync();
            }

            foreach (var doc in loadedDoctors) Doctors.Add(doc);
            foreach (var app in loadedAppointments) Appointments.Add(app);
            
            SelectedDoctor = Doctors.FirstOrDefault();
        }

        private async Task AddDoctor()
        {
            var newDoc = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = $"Новий Лікар {Doctors.Count + 1}",
                Specialization = "Терапевт",
                Rating = 5.0,
                ConsultationFee = 400,
                PhotoUrl = "https://i.pravatar.cc/150",
                Email = "new.doctor@example.com",
            };
            
            await _dataService.AddDoctorAsync(newDoc);
            Doctors.Add(newDoc);
        }

        private async Task DeleteDoctor()
        {
            if (SelectedDoctor == null) return;
            
            var result = MessageBox.Show($"Видалити {SelectedDoctor.FullName}?", "Підтвердження", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _dataService.DeleteDoctorAsync(SelectedDoctor.Id);
                Doctors.Remove(SelectedDoctor);
                SelectedDoctor = null;
            }
        }

        private async Task DeleteAppointment(object parameter)
        {
            if (parameter is Appointment app)
            {
                var result = MessageBox.Show("Скасувати запис?", "Підтвердження", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await _dataService.DeleteAppointmentAsync(app.Id);
                    Appointments.Remove(app);
                }
            }
        }

        private void OpenBookingWindow(object parameter)
        {
            var bookingViewModel = new BookingViewModel(SelectedDoctor);
            var bookingWindow = new BookingWindow { DataContext = bookingViewModel };
            
            if (bookingWindow.ShowDialog() == true)
            {
                var newAppointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    DoctorId = SelectedDoctor.Id,
                    DoctorFullName = SelectedDoctor.FullName,
                    AppointmentTime = bookingViewModel.SelectedDateTime,
                    Status = AppointmentStatus.Scheduled
                };
                
                _TaskHelper.FireAndForget(async () => 
                {
                    await _dataService.AddAppointmentAsync(newAppointment);
                    Application.Current.Dispatcher.Invoke(() => Appointments.Add(newAppointment));
                });
            }
        }

        private async Task SeedData()
        {
             var doc1 = new Doctor { Id = Guid.NewGuid(), FullName = "Петренко Ольга", Specialization = "Терапевт", Rating = 4.8, ConsultationFee = 500, Email = "petrenko@example.com",   PhotoUrl = "https://i.pravatar.cc/150?u=1"  };
             await _dataService.AddDoctorAsync(doc1);
        }
    }
    
    public static class _TaskHelper {
        public static async void FireAndForget(Func<Task> func) {
            try { await func(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}