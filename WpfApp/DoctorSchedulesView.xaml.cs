using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Service;
using Model;
using System.Threading.Tasks;
using System.Diagnostics;

namespace View
{
    public partial class DoctorSchedulesView : UserControl
    {
        private readonly DoctorScheduleService _doctorScheduleService;
        private int _doctorId;
        private int _selectedMonth;
        private int _selectedYear;
        private ObservableCollection<DoctorSchedule> _schedules;
        private ObservableCollection<CalendarDay> _calendarDays;
        private bool _isLoading;
        private bool _isInitialized = false;
        private CalendarDay _selectedDay;
        private SystemUser user;

        public DoctorSchedulesView(DoctorScheduleService DoctorScheduleService)
        {
            InitializeComponent();

            _doctorScheduleService = DoctorScheduleService;

            user = Application.Current.Properties["CurrentUser"] as SystemUser
                   ?? throw new InvalidOperationException("CurrentUser session is missing");

            _doctorId = user.UserId;

            _schedules = [];
            _calendarDays = [];
            _selectedMonth = DateTime.Now.Month;
            _selectedYear = DateTime.Now.Year;
            MonthComboBox.SelectedIndex = _selectedMonth - 1;
            YearTextBox.Text = _selectedYear.ToString();
            CalendarItemsControl.ItemsSource = _calendarDays;

            UpdateMonthYearDisplay();
            _isInitialized = true;
        }

        private async Task<(bool success, string errorMessage)> LoadSchedules()
        {
            if (_isLoading) return (false, "Already loading");
            _isLoading = true;
            LoadCalendarButton.IsEnabled = false;

            try
            {
                if (!int.TryParse(YearTextBox.Text, out _selectedYear) || _selectedYear < 2000)
                {
                    return (false, "Invalid Year");
                }

                _selectedMonth = MonthComboBox.SelectedIndex + 1;
                var schedules = await _doctorScheduleService.GetDoctorSchedulesByMonth(_doctorId, _selectedMonth, _selectedYear);
                _schedules.Clear();
                _calendarDays.Clear();

                foreach (var schedule in schedules)
                {
                    _schedules.Add(schedule);
                }

                var firstDayOfMonth = new DateTime(_selectedYear, _selectedMonth, 1);
                var daysInMonth = DateTime.DaysInMonth(_selectedYear, _selectedMonth);
                var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

                var prevMonth = firstDayOfMonth.AddMonths(-1);
                var daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
                for (int i = 0; i < firstDayOfWeek; i++)
                {
                    var day = daysInPrevMonth - firstDayOfWeek + i + 1;
                    _calendarDays.Add(new CalendarDay { Day = day, Schedules = new List<DoctorSchedule>() });
                }

                for (int day = 1; day <= daysInMonth; day++)
                {
                    var currentDate = new DateOnly(_selectedYear, _selectedMonth, day);
                    var daySchedules = _schedules.Where(s => s.WorkDate == currentDate).ToList();
                    _calendarDays.Add(new CalendarDay { Day = day, Schedules = daySchedules });
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
            finally
            {
                _isLoading = false;
                LoadCalendarButton.IsEnabled = true;
            }
        }

        private async Task ChangeMonth(int increment)
        {
            var currentDate = new DateTime(_selectedYear, _selectedMonth, 1).AddMonths(increment);
            _selectedMonth = currentDate.Month;
            _selectedYear = currentDate.Year;
            MonthComboBox.SelectedIndex = _selectedMonth - 1;
            YearTextBox.Text = _selectedYear.ToString();
            UpdateMonthYearDisplay();
            await LoadSchedules();
        }

        private void UpdateMonthYearDisplay()
        {
            MonthYearTextBlock.Text = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_selectedMonth)}, {_selectedYear}";
        }

        private async void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            await ChangeMonth(1);
        }

        private async void PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            await ChangeMonth(-1);
        }

        private async void LoadCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            var (success, error) = await LoadSchedules();
            if (success)
            {
                MessageBox.Show("Schedules loaded successfully!", "Success", MessageBoxButton.OK);
            }
            else if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Error", MessageBoxButton.OK);
            }
        }

        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var (success, error) = await LoadSchedules();
                if (success)
                {
                    MessageBox.Show("Schedules loaded successfully!", "Success", MessageBoxButton.OK);
                    UpdateMonthYearDisplay();
                }
                else if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error, "Error", MessageBoxButton.OK);
                }
            }
        }

        private async void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            await LoadSchedules();
            UpdateMonthYearDisplay();
        }

        private void CalendarDay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is CalendarDay calendarDay && calendarDay.Day != 0)
            {
                _selectedDay = calendarDay;
                PopupDateTextBlock.Text = $"{calendarDay.Day} {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_selectedMonth)}, {_selectedYear}";
                var scheduleItems = calendarDay.Schedules.Select(s => new
                {
                    s.StartTime,
                    s.EndTime,
                    Status = s.IsAvailable ? "Available" : "Absent",
                    Schedule = s
                }).ToList();
                PopupScheduleItemsControl.ItemsSource = scheduleItems;
                RequestFullDayOffButton.IsEnabled = calendarDay.HasSchedules && calendarDay.Schedules.Any(s => s.IsAvailable);
                SchedulePopup.IsOpen = true;
            }
        }

        private async void RequestSessionOff_Click(object sender, RoutedEventArgs e)
        {
            SchedulePopup.IsOpen = false;
            if (sender is Button button && button.Tag is DoctorSchedule schedule && schedule.IsAvailable)
            {
                var result = MessageBox.Show($"Request off for session {schedule.StartTime} - {schedule.EndTime}?", "Confirm", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _doctorScheduleService.RequestSessionOff(schedule);
                        MessageBox.Show("Session requested off successfully!", "Success", MessageBoxButton.OK);
                        await LoadSchedules();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error requesting session off: {ex.Message}", "Error", MessageBoxButton.OK);
                    }
                }
            }
        }

        private async void RequestFullDayOffButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDay != null && _selectedDay.HasSchedules && _selectedDay.Schedules.Any(s => s.IsAvailable))
            {
                SchedulePopup.IsOpen = false;
                var result = MessageBox.Show($"Request off for the entire day of {_selectedDay.Day} {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_selectedMonth)}, {_selectedYear}?", "Confirm", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Placeholder: Call service to request off all available sessions for the day
                        foreach (var schedule in _selectedDay.Schedules.Where(s => s.IsAvailable))
                        {
                            await _doctorScheduleService.RequestSessionOff(schedule);
                        }
                        MessageBox.Show("Full day requested off successfully!", "Success", MessageBoxButton.OK);
                        await LoadSchedules();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error requesting full day off: {ex.Message}", "Error", MessageBoxButton.OK);
                    }
                }
            }
        }

        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            SchedulePopup.IsOpen = false;
            _selectedDay = null;
        }
    }

    public class CalendarDay
    {
        public int Day { get; set; }
        public List<DoctorSchedule> Schedules { get; set; }
        public bool HasSchedules => Schedules != null && Schedules.Any();
        public bool HasOnlyAvailableSchedules => Schedules.Any() && Schedules.All(s => s.IsAvailable == true);
        public bool HasOnlyAbsentdSchedules => Schedules.Any() && Schedules.All(s => s.IsAvailable == false);
        public bool HasMixedSchedules => Schedules.Any(s => s.IsAvailable == true) && Schedules.Any(s => s.IsAvailable == false);
        public string ScheduleDetails => HasSchedules ? string.Join("\n", Schedules.Select(s => $"{s.StartTime} - {s.EndTime} ({(s.IsAvailable == true ? "Available" : "Absent")})")) : "";
    }

    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int month)
            {
                return month - 1; // Convert to 0-based index for ComboBox
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return index + 1; // Convert back to 1-based month number
            }
            return 1;
        }
    }

    public class MonthNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int month && month >= 1 && month <= 12)
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}