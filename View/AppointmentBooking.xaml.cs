using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DTO.Request;
using DTO.Response;
using Service;

namespace View
{
    public partial class AppointmentBooking : UserControl, INotifyPropertyChanged
    {
        private readonly AppointmentBookingService _service;
        private ObservableCollection<DoctorInfomationResponse> _availableDoctors;
        private DateTime _selectedDate;
        private string _doctorName;

        public AppointmentBooking(AppointmentBookingService service)
        {
            InitializeComponent();
            _service = service;
            DataContext = this;
            _availableDoctors = new ObservableCollection<DoctorInfomationResponse>();
            DoctorName = string.Empty;
            SelectedDate = DateTime.Today;
            // Load initial data
            LoadAvailableDoctor(0, 10, new DoctorInformationSearch()).ConfigureAwait(false);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetPropertyChanged(ref _selectedDate, value);
        }

        public string DoctorName
        {
            get => _doctorName;
            set => SetPropertyChanged(ref _doctorName, value);
        }


        public ObservableCollection<DoctorInfomationResponse> AvailableDoctors
        {
            get => _availableDoctors;
            set => SetPropertyChanged(ref _availableDoctors, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public bool SetPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        private async Task LoadAvailableDoctor(int page, int size, DoctorInformationSearch search)
        {
            try
            {
                var doctors = await _service.fetchDoctorInformation(page, size, search);
                AvailableDoctors = new ObservableCollection<DoctorInfomationResponse>(doctors);
                ResultsHeader.Text = doctors.Any()
                    ? $"Danh sách bác sĩ có lịch ({doctors.Count()})"
                    : "Không tìm thấy bác sĩ phù hợp";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách bác sĩ: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await SearchAvailableDoctor();
        }

        private async Task SearchAvailableDoctor()
        {
            try
            {
                var search = new DoctorInformationSearch
                {
                    FullName = DoctorName?.Trim(),
                    Date = DateOnly.FromDateTime(SelectedDate)
                };

                await LoadAvailableDoctor(0, 10, search);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm bác sĩ: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is DoctorInfomationResponse selectedDoctor)
            {
                Debug.WriteLine($"Date: {_selectedDate}");
                Window window = Window.GetWindow(this);
                if(window is MainWindow mainWindow)
                {
                    mainWindow.MainContent.Content = new DoctorDetailBooking(selectedDoctor, _selectedDate);
                }
            }
        }
    }
}