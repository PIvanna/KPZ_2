using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KPZ_2.Models;
using KPZ_2.Models.Enums;
using KPZ_2.Services;
using KPZ_2.ViewModels.Base;
using KPZ_2.Views;

namespace KPZ_2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly FileDataService _dataService;
        private Doctor _selectedDoctor;
        
        public ObservableCollection<Doctor> Doctors { get; } = new ObservableCollection<Doctor>();
        public ObservableCollection<Appointment> Appointments { get; } = new ObservableCollection<Appointment>();

        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set => Set(ref _selectedDoctor, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand OpenBookingWindowCommand { get; }

        public MainViewModel()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
            _dataService = new FileDataService(filePath);

            LoadDataCommand = new RelayCommand(async _ => await LoadData());
            SaveDataCommand = new RelayCommand(async _ => await SaveData());
            OpenBookingWindowCommand = new RelayCommand(OpenBookingWindow, _ => SelectedDoctor != null);

            LoadDataCommand.Execute(null);
        }

        private async Task LoadData()
        {
            var (loadedDoctors, loadedAppointments) = await _dataService.LoadDataAsync();

            Doctors.Clear();
            Appointments.Clear();

            if (!loadedDoctors.Any())
            {
                CreateInitialData();
            }
            else
            {
                foreach (var doctor in loadedDoctors) Doctors.Add(doctor);
                foreach (var app in loadedAppointments) Appointments.Add(app);
            }
            
            SelectedDoctor = Doctors.FirstOrDefault();
        }

        private async Task SaveData()
        {
            try
            {
                await _dataService.SaveDataAsync(Doctors, Appointments);
                MessageBox.Show("Дані успішно збережено!", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка під час збереження: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Appointments.Add(newAppointment);
            }
        }

        private void CreateInitialData()
        {
            Doctors.Add(new Doctor { Id = Guid.NewGuid(), FullName = "Петренко Ольга Іванівна", Specialization = "Терапевт", Rating = 4.8, ConsultationFee = 500, PhotoUrl="https://i.pravatar.cc/150?u=doc16" });
            Doctors.Add(new Doctor { Id = Guid.NewGuid(), FullName = "Ковальчук Андрій Петрович", Specialization = "Кардіолог", Rating = 4.9, ConsultationFee = 750, PhotoUrl="https://i.pravatar.cc/150?u=doc4" });
            Doctors.Add(new Doctor { Id = Guid.NewGuid(), FullName = "Шевченко Марія Василівна", Specialization = "Педіатр", Rating = 4.7, ConsultationFee = 600, PhotoUrl="https://i.pravatar.cc/150?u=doc15" });
        }
    }
}