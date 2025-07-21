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
    public partial class CreateMedicalRecordView : UserControl
    {
        private readonly MedicalRecordService _medicalRecordService;
        private readonly AppointmentService _appointmentService;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly SpecializationRepository _specializationRepository;
        private readonly DoctorDashboardView _parentView;

        public CreateMedicalRecordView(MedicalRecordService medicalRecordService, 
                                     AppointmentService appointmentService,
                                     AppointmentRepository appointmentRepository,
                                     SpecializationRepository specializationRepository,
                                     DoctorDashboardView parentView)
        {
            InitializeComponent();
            _medicalRecordService = medicalRecordService;
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
            _specializationRepository = specializationRepository;
            _parentView = parentView;

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var currentUser = Application.Current.Properties["CurrentUser"] as SystemUser;
                            if (currentUser == null)
            {
                return;
            }

                // Load appointments for current doctor - chỉ lấy appointments của ngày hôm nay
                var today = DateOnly.FromDateTime(DateTime.Today);
                var appointments = await _appointmentService.GetAppointmentsByDoctor(currentUser.UserId, today);
                
                // Convert to display format with medical record count
                var displayAppointments = new List<object>();
                foreach (var appointment in appointments)
                {
                    var medicalRecords = await _medicalRecordService.GetMedicalRecordsByAppointment(appointment.AppointmentId);
                    var recordCount = medicalRecords.Count();
                    
                    displayAppointments.Add(new
                    {
                        AppointmentId = appointment.AppointmentId,
                        DisplayText = $"ID: {appointment.AppointmentId} - {appointment.Patient?.FullName} (ID: {appointment.PatientId}) - {appointment.AppointmentDate:dd/MM/yyyy} {appointment.TimeSlot:HH:mm} (Đã có {recordCount} phiếu)"
                    });
                }

                AppointmentComboBox.ItemsSource = displayAppointments;
                AppointmentComboBox.SelectionChanged += AppointmentComboBox_SelectionChanged;

                // Load specializations
                var specializations = await _specializationRepository.GetAllSpecializationsAsync();
                SpecializationComboBox.ItemsSource = specializations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}\n\nStackTrace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AppointmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AppointmentComboBox.SelectedItem == null)
                {
                    PatientInfoTextBox.Text = "";
                    return;
                }

                var selectedAppointment = AppointmentComboBox.SelectedItem;
                var appointmentId = (int)selectedAppointment.GetType().GetProperty("AppointmentId").GetValue(selectedAppointment);

                var appointment = await _appointmentService.GetAppointmentById(appointmentId);
                if (appointment != null && appointment.Patient != null)
                {
                    var patient = appointment.Patient;
                    PatientInfoTextBox.Text = $"ID: {patient.PatientId}\n" +
                                             $"Họ tên: {patient.FullName}\n" +
                                             $"SĐT: {patient.PhoneNumber}\n" +
                                             $"Email: {patient.Email}\n" +
                                             $"Giới tính: {patient.Gender}\n" +
                                             $"Địa chỉ: {patient.Address}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patient info: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CreateMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs
                if (AppointmentComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn lịch hẹn", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SpecializationComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn chuyên khoa", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Get selected appointment and specialization
                var selectedAppointment = AppointmentComboBox.SelectedItem;
                var appointmentId = (int)selectedAppointment.GetType().GetProperty("AppointmentId").GetValue(selectedAppointment);
                
                var selectedSpecialization = SpecializationComboBox.SelectedItem as Specialization;
                var specializationId = selectedSpecialization.Id;

                // Create medical record request
                var request = new MedicalRecordRequest
                {
                    AppointmentId = appointmentId,
                    SpecId = specializationId
                };

                // Create medical record
                var success = await _medicalRecordService.CreateMedicalRecord(request);

                if (success)
                {
                    MessageBox.Show("Medical referral created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Clear form
                    ClearForm();
                    
                    // Navigate back to doctor dashboard
                    if (_parentView != null)
                    {
                        _parentView.BtnDashboard_Click(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to create medical referral. Please try again.", 
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating medical referral: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (_parentView != null)
            {
                _parentView.BtnDashboard_Click(sender, e);
            }
        }

        private void ClearForm()
        {
            AppointmentComboBox.SelectedItem = null;
            SpecializationComboBox.SelectedItem = null;
            PatientInfoTextBox.Text = "";
        }
    }
} 