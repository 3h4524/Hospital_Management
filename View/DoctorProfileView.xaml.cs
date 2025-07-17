using DataAccess;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System;

namespace View
{
    public partial class DoctorProfileView : UserControl
    {
        public DoctorProfileView()
        {
            InitializeComponent();
            DataContext = new DoctorProfileViewModel();
        }
    }

    public class DoctorProfileViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DoctorProfileItem> _allDoctors = new();
        private ObservableCollection<DoctorProfileItem> _doctors = new();
        public ObservableCollection<DoctorProfileItem> Doctors
        {
            get => _doctors;
            set { _doctors = value; OnPropertyChanged(nameof(Doctors)); }
        }
        public ObservableCollection<string> Specializations { get; set; } = new();
        private string _selectedSpecialization = "Tất cả";
        public string SelectedSpecialization
        {
            get => _selectedSpecialization;
            set { _selectedSpecialization = value; OnPropertyChanged(nameof(SelectedSpecialization)); ApplyFilter(); }
        }
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); ApplyFilter(); }
        }
        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value ?? string.Empty; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        public DoctorProfileViewModel()
        {
            LoadDoctors();
        }
        private void LoadDoctors()
        {
            try
            {
                using var context = new HospitalManagementContext();
                var doctors = context.SystemUsers
                    .Where(u => u.Role == "Doctor" && u.DoctorProfile != null)
                    .Select(u => new DoctorProfileItem
                    {
                        FullName = u.FullName ?? string.Empty,
                        Email = u.Email ?? string.Empty,
                        PhoneNumber = u.PhoneNumber ?? string.Empty,
                        Specialization = u.DoctorProfile.Specialization ?? string.Empty,
                        BaseSalary = u.DoctorProfile.BaseSalary ?? 0,
                        CreatedAt = u.CreatedAt,
                        IsActive = u.IsActive ?? true,
                        Gender = u.DoctorProfile.Gender ?? string.Empty,
                        Degree = u.DoctorProfile.Degree ?? string.Empty,
                        Address = u.DoctorProfile.Address ?? string.Empty,
                    }).ToList();
                foreach (var d in doctors)
                {
                    if (d.CreatedAt.HasValue)
                        d.YearsOfExperience = (int)((DateTime.Now - d.CreatedAt.Value).TotalDays / 365);
                    else
                        d.YearsOfExperience = 0;
                }
                _allDoctors = new ObservableCollection<DoctorProfileItem>(doctors);
                Specializations = new ObservableCollection<string>(new[] { "Tất cả" }.Concat(_allDoctors.Select(d => d.Specialization).Distinct().Where(s => !string.IsNullOrWhiteSpace(s))));
                SelectedSpecialization = "Tất cả";
                ApplyFilter();
                ErrorMessage = "";
            }
            catch (System.Exception ex)
            {
                ErrorMessage = "Lỗi khi tải danh sách bác sĩ: " + ex.Message;
                Doctors = new ObservableCollection<DoctorProfileItem>();
            }
        }
        private void ApplyFilter()
        {
            var filtered = _allDoctors.Where(d =>
                (SelectedSpecialization == "Tất cả" || d.Specialization == SelectedSpecialization) &&
                (string.IsNullOrWhiteSpace(SearchText) ||
                 (d.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                 (d.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                 (d.PhoneNumber?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
            ).ToList();
            Doctors = new ObservableCollection<DoctorProfileItem>(filtered);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string? name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class DoctorProfileItem
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public decimal BaseSalary { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int YearsOfExperience { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
} 