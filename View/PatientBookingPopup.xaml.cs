using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DTO.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Model;
using Repository;
using Service;

namespace View
{
    public partial class PatientBookingPopup : UserControl, INotifyPropertyChanged
    {
        private readonly PatientRepository _patientRepository;
        private readonly AppointmentBookingService _bookingService;
        private readonly DoctorAppointmentResponse _appointment;
        private string _phoneNumber;
        private string _patientName;
        private string _gender;
        private ObservableCollection<string> _genders = new ObservableCollection<string> { "MALE", "FEMALE", "OTHER" };
        private TimeOnly _selectedStartTime;
        private TimeOnly _selectedEndTime;
        private Visibility _patientInfoVisibility = Visibility.Collapsed;
        private int? _patientId;
        private DateOnly _date;
        private AppointmentRepository _appointmentRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetPropertyChanged(ref _phoneNumber, value);
        }

        public string PatientName
        {
            get => _patientName;
            set => SetPropertyChanged(ref _patientName, value);
        }

        public string Gender
        {
            get => _gender;
            set => SetPropertyChanged(ref _gender, value);
        }

        public ObservableCollection<string> Genders
        {
            get => _genders;
            set => SetPropertyChanged(ref _genders, value);
        }

        public TimeOnly SelectedStartTime
        {
            get => _selectedStartTime;
            set => SetPropertyChanged(ref _selectedStartTime, value);
        }

        public TimeOnly SelectedEndTime
        {
            get => _selectedEndTime;
            set => SetPropertyChanged(ref _selectedEndTime, value);
        }

        public Visibility PatientInfoVisibility
        {
            get => _patientInfoVisibility;
            set => SetPropertyChanged(ref _patientInfoVisibility, value);
        }

   

        public PatientBookingPopup(DoctorAppointmentResponse appointment, DateOnly date)
        {
            InitializeComponent();
            DataContext = this;
            _patientRepository = App._serviceProvider.GetRequiredService<PatientRepository>();
            _bookingService = App._serviceProvider.GetRequiredService<AppointmentBookingService>();
            _appointment = appointment;
            SelectedStartTime = appointment.StartTime;
            SelectedEndTime = appointment.EndTime;
            _date = date;
            LoadInitializeComponent(appointment).ConfigureAwait(false);
            _appointmentRepository = App._serviceProvider.GetRequiredService<AppointmentRepository>();
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetPropertyChanged<T>(ref T field, T value,[CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private async void CheckPatient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!");
                return;
            }

            var patient = await _patientRepository.GetPatientByPhone(PhoneNumber);
            if (patient != null)
            {
                Debug.WriteLine($"Patient: {patient.Gender.Trim()}");
                PatientName = patient.FullName;
                Gender = patient.Gender;
                _patientId = patient.PatientId;
                PatientInfoVisibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("không tìm thấy bệnh nhân vui lòng nhập thông tin đầy đủ");
                PatientName = "";
                Gender = null;
                _patientId = null;
                PatientInfoVisibility = Visibility.Visible;
            }
        }

        private async void RegisterAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!");
                return;
            }

            int patientId;
            if (_patientId.HasValue)
            {
                patientId = _patientId.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(PatientName) || string.IsNullOrEmpty(Gender))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin bệnh nhân!");
                    return;
                }

                var newPatient = new Patient
                {
                    FullName = PatientName,
                    PhoneNumber = PhoneNumber,
                    Gender = Gender.ToUpper(),
                };
                await _patientRepository.Add(newPatient);
                var patient = await _patientRepository.GetPatientByPhone(PhoneNumber);
                if(patient == null)
                {
                    MessageBox.Show("Lỗi không thể tạo bệnh nhân. Vui lòng thử lại");
                    return;
                } 
                patientId = patient.PatientId;
            }

            Appointment appointment;

            if(_appointment.PatientId == 0)
            {
                appointment = new Appointment
                {
                    DoctorId = _appointment.DoctorId,
                    PatientId = patientId,
                    StartTime = SelectedStartTime,
                    EndTime = SelectedEndTime,
                    AppointmentDate = _date,
                };
            } else
            {
                appointment = await _appointmentRepository.FindByID(_appointment.AppointmentId);
                appointment.PatientId = patientId;
                appointment.StartTime = SelectedStartTime;
                appointment.EndTime = SelectedEndTime;
            }

                bool success = await _bookingService.CreateOrUpdateAppointment(appointment);
            if (success)
            {   
                _appointment.PatientId = patientId;
                _appointment.PatientName = PatientName;
                MessageBox.Show("Đăng ký lịch khám thành công!");
                var window = Window.GetWindow(this);
                window?.Close();
            }
            else
            {
                MessageBox.Show("Đăng ký lịch khám thất bại!");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window?.Close();
        }

        private async Task LoadInitializeComponent(DoctorAppointmentResponse appointment)
        {
            var patient = await _patientRepository.FindByID(appointment.PatientId);
            if(patient != null)
            {
                PhoneNumber = patient.PhoneNumber;
                PatientName = patient.FullName;
                Gender = patient.Gender;
                PatientInfoVisibility = Visibility;
            }
        }

    }

}
