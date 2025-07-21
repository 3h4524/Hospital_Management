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
    public partial class MedicalRecordsView : UserControl
    {
        private readonly MedicalRecordService _medicalRecordService;
        private readonly AppointmentService _appointmentService;
        private readonly DoctorDashboardView _parentView;

        public MedicalRecordsView(MedicalRecordService medicalRecordService, 
                                AppointmentService appointmentService,
                                DoctorDashboardView parentView)
        {
            InitializeComponent();
            _medicalRecordService = medicalRecordService;
            _appointmentService = appointmentService;
            _parentView = parentView;

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var currentUser = Application.Current.Properties["CurrentUser"] as SystemUser;
                if (currentUser == null) return;

                await LoadMedicalRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadMedicalRecords(string searchTerm = null)
        {
            try
            {
                var currentUser = Application.Current.Properties["CurrentUser"] as SystemUser;
                if (currentUser == null) return;

                // Lấy tất cả appointments để tất cả doctor có thể xem tất cả phiếu chỉ định
                var allAppointments = await _appointmentService.GetAllAppointments();
                var medicalRecordViews = new List<MedicalRecordViewModel>();

                foreach (var appointment in allAppointments)
                {
                    var medicalRecords = await _medicalRecordService.GetMedicalRecordsByAppointment(appointment.AppointmentId);
                    
                    foreach (var record in medicalRecords)
                    {
                        var patientName = appointment.Patient?.FullName ?? "Unknown";
                        var patientId = appointment.PatientId.ToString();
                        var patientPhone = appointment.Patient?.PhoneNumber ?? "";
                        
                        // Filter by search term if provided
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            if (!patientName.ToLower().Contains(searchTerm.ToLower()) && 
                                !patientId.Contains(searchTerm) &&
                                !patientPhone.Contains(searchTerm))
                                continue;
                        }
                        
                        medicalRecordViews.Add(new MedicalRecordViewModel
                        {
                            RecordId = record.RecordId,
                            AppointmentId = appointment.AppointmentId,
                            PatientName = patientName,
                            PatientId = appointment.PatientId,
                            PatientPhone = patientPhone,
                            SpecializationId = record.SpecializationId,
                            AppointmentDate = $"Ngày khám: {appointment.AppointmentDate:dd/MM/yyyy} {appointment.TimeSlot:HH:mm}",
                            SpecializationName = record.SpecializationId == 1 ? "Tổng hợp" : (record.Specialization?.Name ?? "Unknown"),
                            Diagnosis = record.Diagnosis ?? "",
                            Notes = record.Notes ?? "",
                            CreatedAt = $"Tạo lúc: {record.CreatedAt:dd/MM/yyyy HH:mm}"
                        });
                    }
                }

                // Sort by creation date (newest first)
                medicalRecordViews = medicalRecordViews.OrderByDescending(r => r.CreatedAt).ToList();

                MedicalRecordsItemsControl.ItemsSource = medicalRecordViews;
                
                // Show/hide no records message
                NoRecordsText.Visibility = medicalRecordViews.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading medical referrals: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = SearchTextBox.Text?.Trim();
            if (searchTerm == "Nhập tên hoặc SĐT bệnh nhân...")
                searchTerm = "";
                
            await LoadMedicalRecords(searchTerm);
            
            // Tự động xóa nội dung ô tìm kiếm sau khi tìm kiếm
            SearchTextBox.Text = "";
            SearchTextBox.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Nhập tên hoặc SĐT bệnh nhân...")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Nhập tên hoặc SĐT bệnh nhân...";
                SearchTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private async void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var record = button?.DataContext as MedicalRecordViewModel;
            if (record == null) return;

            // Create edit dialog
            var editDialog = new EditMedicalRecordDialog(record);
            var result = editDialog.ShowDialog();
            
            if (result == true)
            {
                // Update the record
                var success = await _medicalRecordService.UpdateMedicalRecord(
                    record.RecordId, 
                    editDialog.Diagnosis, 
                    editDialog.Notes
                );

                if (success)
                {
                    MessageBox.Show("Cập nhật phiếu chỉ định thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadMedicalRecords(); // Reload data
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật phiếu chỉ định!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void Finish_Click(object sender, RoutedEventArgs e)
        {
            var records = MedicalRecordsItemsControl.ItemsSource as List<MedicalRecordViewModel>;
            if (records == null || !records.Any())
            {
                MessageBox.Show("Không có phiếu chỉ định nào để tổng hợp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Group by patient
            var patientGroups = records.GroupBy(r => r.PatientId).ToList();
            if (patientGroups.Count > 1)
            {
                MessageBox.Show("Chỉ có thể tổng hợp phiếu của 1 bệnh nhân!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var patientGroup = patientGroups.First();
            var patientName = patientGroup.First().PatientName;
            // Chỉ lấy phiếu chuyên khoa (không phải tổng hợp)
            var specializedRecords = patientGroup.Where(r => r.SpecializationName != "Tổng hợp").ToList();
            
            if (!specializedRecords.Any())
            {
                MessageBox.Show("Không có phiếu chuyên khoa nào để tổng hợp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Tổng hợp chẩn đoán và ghi chú
            var combinedDiagnosis = string.Join("\n", specializedRecords
                .Where(r => !string.IsNullOrEmpty(r.Diagnosis))
                .Select(r => $"• {r.SpecializationName}: {r.Diagnosis}"));
                
            var combinedNotes = string.Join("\n", specializedRecords
                .Where(r => !string.IsNullOrEmpty(r.Notes))
                .Select(r => $"• {r.SpecializationName}: {r.Notes}"));

            var summaryRecord = new Model.MedicalRecord
            {
                AppointmentId = patientGroup.First().AppointmentId,
                SpecializationId = 1, // Tổng hợp
                Diagnosis = string.IsNullOrEmpty(combinedDiagnosis) ? "" : $"=== CHẨN ĐOÁN TỔNG HỢP ===\n{combinedDiagnosis}",
                Notes = string.IsNullOrEmpty(combinedNotes) ? "" : $"=== GHI CHÚ TỔNG HỢP ===\n{combinedNotes}",
                CreatedAt = DateTime.Now
            };

            var result = MessageBox.Show("Bạn có chắc chắn không?", "Xác nhận tổng hợp", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                var success = await _medicalRecordService.CreateSummaryAndDeleteOld(
                    new List<Model.MedicalRecord> { summaryRecord },
                    specializedRecords.Select(r => r.RecordId).ToList()
                );

                if (success)
                {
                    MessageBox.Show($"Đã tổng hợp thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadMedicalRecords();
                }
                else
                {
                    MessageBox.Show("Không thể tổng hợp phiếu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string CreateSummaryDiagnosis(List<MedicalRecordViewModel> records, string specializationName)
        {
            var diagnosis = $"=== CHẨN ĐOÁN TỔNG HỢP - {specializationName} ===\n\n";
            
            foreach (var record in records)
            {
                if (!string.IsNullOrEmpty(record.Diagnosis))
                {
                    diagnosis += $"• {record.Diagnosis}\n";
                }
            }
            
            return diagnosis;
        }

        private string CreateSummaryNotes(List<MedicalRecordViewModel> records, string specializationName)
        {
            var notes = $"=== GHI CHÚ TỔNG HỢP - {specializationName} ===\n\n";
            
            foreach (var record in records)
            {
                if (!string.IsNullOrEmpty(record.Notes))
                {
                    notes += $"• {record.Notes}\n";
                }
            }
            
            return notes;
        }
    }

    public class MedicalRecordViewModel
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int SpecializationId { get; set; }
        public string PatientName { get; set; } = "";
        public string PatientPhone { get; set; } = "";
        public string AppointmentDate { get; set; } = "";
        public string SpecializationName { get; set; } = "";
        public string Diagnosis { get; set; } = "";
        public string Notes { get; set; } = "";
        public string CreatedAt { get; set; } = "";
    }
} 