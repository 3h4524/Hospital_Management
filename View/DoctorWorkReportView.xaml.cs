using System.Windows;
using System.Windows.Controls;
using DTO.Response;
using System.Collections.ObjectModel;
using Service;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Model;

namespace View
{
    public partial class DoctorWorkReportView : UserControl, INotifyPropertyChanged
    {
        private WorkReportService _workReportService;
        private UserRoleService _userRoleService;

        private ObservableCollection<DoctorScheduleReportResponse> _doctorScheduleReports;
        private int _selectedMonth;
        private ObservableCollection<DailyScheduleReportResponse> _dailyScheduleReportResponses;
        private SystemUser _selectedDoctor;


        public DoctorWorkReportView(WorkReportService workReportService, UserRoleService userRoleService)
        {
            InitializeComponent();
            _workReportService = workReportService;
            _userRoleService = userRoleService;
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

        public SystemUser SelectedDoctor
        {
            get => _selectedDoctor;
            set => SetPropertyChanged(ref _selectedDoctor, value);
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
                SelectedDoctor = await _userRoleService.GetUserById(report.DoctorId);
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

        private void RewardPenaltyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.Tag?.ToString() == "Reward") TypeComboBox.SelectedIndex = 0; else TypeComboBox.SelectedIndex = 1;

            RewardPenaltyForm.Visibility = Visibility.Visible;
        }

        private void SaveRewardPenaltyButton_Click(object sender, RoutedEventArgs e)
        {
            RewardPenalty rp = new RewardPenalty
            {
                UserId = SelectedDoctor.UserId,
                Rpdate = DateOnly.FromDateTime(RpDatePicker.SelectedDate ?? DateTime.Today),
                Type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString(),
                Amount = decimal.TryParse(AmountTextBox.Text, out var amount) ? amount : 0,
                Reason = ReasonTextBox.Text

            };

            _workReportService.SaveRewardPenalty(rp);
            RewardPenaltyForm.Visibility = Visibility.Collapsed;
        }

        private void CancelRewardPenaltyButton_Click(object sender, RoutedEventArgs e)
        {
            RewardPenaltyForm.Visibility = Visibility.Collapsed;
        }
    }
}