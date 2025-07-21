using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Service;
using Model;
using Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace View
{
    public partial class CreateAppointmentView : UserControl
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;
        private readonly SystemUserRepository _userRepository;
        private readonly DoctorScheduleRepository _doctorScheduleRepository;
        private readonly ReceptionistDashboardView _parentView;

        public CreateAppointmentView(AppointmentService appointmentService, 
                                   PatientService patientService,
                                   SystemUserRepository userRepository,
                                   DoctorScheduleRepository doctorScheduleRepository,
                                   ReceptionistDashboardView parentView)
        {
            InitializeComponent();
            _appointmentService = appointmentService;
            _patientService = patientService;
            _userRepository = userRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _parentView = parentView;

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Load patients
                var patients = await _patientService.GetAllPatients();
                PatientComboBox.ItemsSource = patients;

                // Load doctors
                var doctors = await _userRepository.Find(d => d.Role == "Doctor" && d.IsActive == true);
                DoctorComboBox.ItemsSource = doctors;

                // Set default date to today
                AppointmentDatePicker.SelectedDate = DateTime.Today;

                // Load time slots
                LoadTimeSlots();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTimeSlots()
        {
            var timeSlots = new List<TimeOnly>
            {
                new TimeOnly(8, 0),   // 8:00 AM
                new TimeOnly(8, 30),  // 8:30 AM
                new TimeOnly(9, 0),   // 9:00 AM
                new TimeOnly(9, 30),  // 9:30 AM
                new TimeOnly(10, 0),  // 10:00 AM
                new TimeOnly(10, 30), // 10:30 AM
                new TimeOnly(11, 0),  // 11:00 AM
                new TimeOnly(11, 30), // 11:30 AM
                new TimeOnly(14, 0),  // 2:00 PM
                new TimeOnly(14, 30), // 2:30 PM
                new TimeOnly(15, 0),  // 3:00 PM
                new TimeOnly(15, 30), // 3:30 PM
                new TimeOnly(16, 0),  // 4:00 PM
                new TimeOnly(16, 30), // 4:30 PM
                new TimeOnly(17, 0),  // 5:00 PM
                new TimeOnly(17, 30)  // 5:30 PM
            };

            // Convert to display format
            var displayTimeSlots = timeSlots.Select(t => new
            {
                Time = t,
                DisplayText = t.ToString("HH:mm")
            }).ToList();

            TimeSlotComboBox.ItemsSource = displayTimeSlots;
            TimeSlotComboBox.DisplayMemberPath = "DisplayText";
            TimeSlotComboBox.SelectedValuePath = "Time";
        }

        private async void CreateAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs
                if (PatientComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a patient", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DoctorComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a doctor", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (AppointmentDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please select a date", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (TimeSlotComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a time slot", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ReasonTextBox.Text))
                {
                    MessageBox.Show("Please enter a reason for the appointment", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Get selected values
                var patient = (Patient)PatientComboBox.SelectedItem;
                var doctor = (SystemUser)DoctorComboBox.SelectedItem;
                var appointmentDate = DateOnly.FromDateTime(AppointmentDatePicker.SelectedDate.Value);
                var selectedTimeSlot = TimeSlotComboBox.SelectedItem;
                if (selectedTimeSlot == null)
                {
                    MessageBox.Show("Please select a time slot", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var timeProperty = selectedTimeSlot.GetType().GetProperty("Time");
                if (timeProperty?.GetValue(selectedTimeSlot) is not TimeOnly timeSlot)
                {
                    MessageBox.Show("Invalid time slot selected", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var reason = ReasonTextBox.Text.Trim();

                // Create appointment
                var success = await _appointmentService.CreateAppointment(
                    patient.PatientId,
                    doctor.UserId,
                    appointmentDate,
                    timeSlot,
                    reason
                );

                if (success)
                {
                    MessageBox.Show("Appointment created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Navigate back to receptionist dashboard
                    Window main = Window.GetWindow(this);
                    main.Content = _parentView;
                }
                else
                {
                    MessageBox.Show("Failed to create appointment. Please check:\n" +
                                  "1. Doctor is available at selected time\n" +
                                  "2. Time slot is not already booked\n" +
                                  "3. Selected date is valid", 
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to receptionist dashboard
            Window main = Window.GetWindow(this);
            main.Content = _parentView;
        }

        private async void AddNewPatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var patientWindow = new PatientInputWindow();
                if (patientWindow.ShowDialog() == true)
                {
                    var patient = patientWindow.Patient;
                    if (patient != null)
                    {
                        var success = await _patientService.CreatePatient(
                            patient.FullName,
                            patient.PhoneNumber ?? "",
                            patient.Email ?? "",
                            patient.Gender ?? "Nam",
                            patient.Address ?? ""
                        );

                        if (success)
                        {
                            MessageBox.Show("Bệnh nhân đã được tạo thành công!", "Thành công", 
                                           MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            // Refresh patient list
                            await LoadPatients();
                            
                            // Select the newly created patient
                            var newPatient = await _patientService.GetPatientByEmail(patient.Email ?? "");
                            if (newPatient != null)
                            {
                                PatientComboBox.SelectedItem = newPatient;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không thể tạo bệnh nhân. Có thể email đã tồn tại.", 
                                           "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Lỗi khi tạo bệnh nhân: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nChi tiết: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadPatients()
        {
            try
            {
                var patients = await _patientService.GetAllPatients();
                PatientComboBox.ItemsSource = patients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patients: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 