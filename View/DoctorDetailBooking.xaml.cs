using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO.Response;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;

namespace View
{
    /// <summary>
    /// Interaction logic for DoctorDetailBooking.xaml
    /// </summary>
    public partial class DoctorDetailBooking : UserControl, INotifyPropertyChanged
    {
        private AppointmentBookingService _bookingService;
        private DoctorInfomationResponse _doctorInformation;
        private ObservableCollection<DoctorAppointmentResponse> _doctorSchedule;
        private string _doctorName;
        private DateTime _selectedDate;
        private PatientRepository _patientRepository;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DoctorDetailBooking(DoctorInfomationResponse doctorInformation, DateTime date)
        {
            InitializeComponent();
            DataContext = this;
            _bookingService = App._serviceProvider.GetRequiredService<AppointmentBookingService>();
            _doctorInformation = doctorInformation;
            _doctorSchedule = new ObservableCollection<DoctorAppointmentResponse>();
            LoadDoctorSchedule(doctorInformation.DoctorId, DateOnly.FromDateTime(date)).ConfigureAwait(false);
            _doctorName = doctorInformation.DoctorName;
            _patientRepository = App._serviceProvider.GetRequiredService<PatientRepository>();
            _selectedDate = date;
        }

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private bool SetPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        public ObservableCollection<DoctorAppointmentResponse> DoctorSchedule
        {
            get => _doctorSchedule;
            set => SetPropertyChanged(ref _doctorSchedule, value);
        }

        public string DoctorName
        {
            get => _doctorName;
            set => SetPropertyChanged(ref _doctorName, value);
        } 
        
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetPropertyChanged(ref _selectedDate, value);
        }

        private async void Search_Doctor_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Seach Date: {_selectedDate}");
            await LoadDoctorSchedule(_doctorInformation.DoctorId, DateOnly.FromDateTime(_selectedDate));
        }

        private async Task LoadDoctorSchedule(int doctorId, DateOnly date)
        {
            var result = await _bookingService.GetScheduleForDoctor(doctorId, date);
            foreach(var item in result)
            {
                Debug.WriteLine($"item: {item.ToString()}");
            }
            DoctorSchedule = new ObservableCollection<DoctorAppointmentResponse>(result);
        }

        private void BookingAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is DoctorAppointmentResponse appointment)
            {
                var popup = new Window
                {
                    Title = "Đặt lịch khám",
                    Content = new PatientBookingPopup(appointment, DateOnly.FromDateTime(_selectedDate)),
                    Width = 500,
                    Height = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Window.GetWindow(this)
                };
                popup.Closed += (s, args) =>
                {
                    LoadDoctorSchedule(_doctorInformation.DoctorId, DateOnly.FromDateTime(_selectedDate)).ConfigureAwait(false);
                };
                popup.ShowDialog();
            }
        }
    }
}
