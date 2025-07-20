using System;
using System.Collections.Generic;
using System.Linq;
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

        public DoctorWorkReportView(WorkReportService workReportService)
        {
            InitializeComponent();


            _workReportService = workReportService;
            _doctorScheduleReports = [];
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

            var result = (await _workReportService.GetDailyScheduleReportResponseSofarByDoctorId(report.DoctorId, _selectedMonth)).ToList();
            _dailyScheduleReportResponses = [.. result];

            ViewDetailReports.IsOpen = true;
        }
    }
}
