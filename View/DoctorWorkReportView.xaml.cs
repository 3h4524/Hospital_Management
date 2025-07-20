using System.Windows;
using System.Windows.Controls;
using DTO.Response;
using System.Collections.ObjectModel;
using Service;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace View
{
    public partial class DoctorWorkReportView : UserControl, INotifyPropertyChanged
    {
        private WorkReportService _workReportService;
        private ObservableCollection<DoctorScheduleReportResponse> _doctorScheduleReports;
        private int _selectedMonth;
        private ObservableCollection<DailyScheduleReportResponse> _dailyScheduleReportResponses;
        private String _selectedDoctorName;

        public DoctorWorkReportView(WorkReportService workReportService)
        {
            InitializeComponent();
            _workReportService = workReportService;
            _doctorScheduleReports = new ObservableCollection<DoctorScheduleReportResponse>();
            _dailyScheduleReportResponses = new ObservableCollection<DailyScheduleReportResponse>();
            DataContext = this;
            _selectedMonth = DateTime.Now.Month;
            MonthComboBox.SelectedIndex = _selectedMonth - 1;
        }

        public ObservableCollection<DoctorScheduleReportResponse> DoctorScheduleReports
        {
            get => _doctorScheduleReports;
            set => SetPropertyChanged(ref _doctorScheduleReports, value);
        }

        public ObservableCollection<DailyScheduleReportResponse> DailyScheduleReportResponses
        {
            get => _dailyScheduleReportResponses;
            set => SetPropertyChanged(ref _dailyScheduleReportResponses, value);
        }

        public string SelectedDoctorName
        {
            get => _selectedDoctorName;
            set => SetPropertyChanged(ref _selectedDoctorName, value);
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

        public async Task LoadScheduleReports()
        {
            var result = await _workReportService.GetDoctorScheduleReportsSoFar(_selectedMonth);
            DoctorScheduleReports = [.. result];
        }

        private async void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedMonth = MonthComboBox.SelectedIndex + 1;
            await LoadScheduleReports();
        }

        private async void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            DoctorScheduleReportResponse? report = button.DataContext as DoctorScheduleReportResponse;
            if (report != null)
            {
                SelectedDoctorName = report.DoctorName;
                var result = await _workReportService.GetDailyScheduleReportResponseSofarByDoctorId(report.DoctorId, _selectedMonth);
                DailyScheduleReportResponses = [.. result];
                Debug.WriteLine($"haha: {DailyScheduleReportResponses}");
                ViewDetailReports.IsOpen = true;
            }
        }

        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            ViewDetailReports.IsOpen = false;
        }
    }
}