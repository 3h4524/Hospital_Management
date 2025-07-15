//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Service;
//using Model;

//namespace ViewModel
//{
//    public class DoctorSchedulesViewModel : ViewModelBase
//    {
//        private readonly DoctorScheduleService _doctorScheduleService;
//        private readonly INavigateService _navigateService;

//        private int _doctorId;
//        private int _selectedMonth;
//        private int _selectedYear;
//        private string _message = "";
//        private bool _isLoading;
//        private ObservableCollection<DoctorSchedule> _schedules;
//        private ObservableCollection<CalendarDay> _calendarDays;

//        public DoctorSchedulesViewModel(DoctorScheduleService doctorScheduleService, INavigateService navigateService)
//        {
//            _doctorScheduleService = doctorScheduleService ?? throw new ArgumentNullException(nameof(doctorScheduleService));
//            _navigateService = navigateService ?? throw new ArgumentNullException(nameof(navigateService));
//            _schedules = new ObservableCollection<DoctorSchedule>();
//            _calendarDays = new ObservableCollection<CalendarDay>();
//            _doctorId = 1;
//            _selectedMonth = DateTime.Now.Month;
//            _selectedYear = DateTime.Now.Year;
//            LoadSchedulesCommand = new RelayCommand(async parameter => await LoadSchedules(), CanLoadSchedules);
//            PreviousMonthCommand = new RelayCommand(_ => ChangeMonth(-1), _ => !IsLoading);
//            NextMonthCommand = new RelayCommand(_ => ChangeMonth(1), _ => !IsLoading);
//        }

//        public int DoctorId
//        {
//            get => _doctorId;
//            set => SetProperty(ref _doctorId, value);
//        }

//        public int SelectedMonth
//        {
//            get => _selectedMonth;
//            set
//            {
//                if (SetProperty(ref _selectedMonth, value))
//                {
//                    ((RelayCommand)LoadSchedulesCommand).RaiseCanExecuteChanged();
//                }
//            }
//        }

//        public int SelectedYear
//        {
//            get => _selectedYear;
//            set
//            {
//                if (SetProperty(ref _selectedYear, value))
//                {
//                    ((RelayCommand)LoadSchedulesCommand).RaiseCanExecuteChanged();
//                }
//            }
//        }

//        public string Message
//        {
//            get => _message;
//            set => SetProperty(ref _message, value);
//        }

//        public bool IsLoading
//        {
//            get => _isLoading;
//            private set
//            {
//                if (SetProperty(ref _isLoading, value))
//                {
//                    ((RelayCommand)LoadSchedulesCommand).RaiseCanExecuteChanged();
//                }
//            }
//        }

//        public ObservableCollection<DoctorSchedule> Schedules
//        {
//            get => _schedules;
//            set => SetProperty(ref _schedules, value);
//        }

//        public ObservableCollection<CalendarDay> CalendarDays
//        {
//            get => _calendarDays;
//            set => SetProperty(ref _calendarDays, value);
//        }

//        public ICommand LoadSchedulesCommand { get; }
//        public ICommand PreviousMonthCommand { get; }
//        public ICommand NextMonthCommand { get; }

//        private async Task LoadSchedules()
//        {
//            if (IsLoading) return;
//            IsLoading = true;
//            Message = string.Empty;

//            try
//            {
//                var schedules = await _doctorScheduleService.GetDoctorSchedulesByMonth(DoctorId, SelectedMonth, SelectedYear);
//                Schedules.Clear();
//                CalendarDays.Clear();

//                foreach (var schedule in schedules)
//                {
//                    Schedules.Add(schedule);
//                }

//                var firstDayOfMonth = new DateTime(SelectedYear, SelectedMonth, 1);
//                var daysInMonth = DateTime.DaysInMonth(SelectedYear, SelectedMonth);
//                var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

//                // Tính toán ngày của tháng trước
//                var prevMonth = firstDayOfMonth.AddMonths(-1);
//                var daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
//                for (int i = 0; i < firstDayOfWeek; i++)
//                {
//                    var day = daysInPrevMonth - firstDayOfWeek + i + 1;
//                    CalendarDays.Add(new CalendarDay { Day = day, Schedules = new List<DoctorSchedule>() });
//                }

//                // Thêm các ngày của tháng hiện tại
//                for (int day = 1; day <= daysInMonth; day++)
//                {
//                    var currentDate = new DateOnly(SelectedYear, SelectedMonth, day);
//                    var daySchedules = Schedules.Where(s => s.WorkDate == currentDate).ToList();
//                    CalendarDays.Add(new CalendarDay { Day = day, Schedules = daySchedules });
//                }

//                Message = schedules.Any() ? "Schedules loaded successfully!" : "No schedules found for the selected period.";
//            }
//            catch (Exception ex)
//            {
//                Message = $"Error: {ex.Message}";
//            }
//            finally
//            {
//                IsLoading = false;
//            }
//        }

//        private bool CanLoadSchedules(object parameter)
//        {
//            return !IsLoading && DoctorId > 0 && SelectedMonth >= 1 && SelectedMonth <= 12 && SelectedYear >= 2000;
//        }

//        private void ChangeMonth(int increment)
//        {
//            var currentDate = new DateTime(SelectedYear, SelectedMonth, 1).AddMonths(increment);
//            SelectedMonth = currentDate.Month;
//            SelectedYear = currentDate.Year;
//            LoadSchedulesCommand.Execute(null);
//        }
//    }

//    public class CalendarDay
//    {
//        public int Day { get; set; }
//        public List<DoctorSchedule> Schedules { get; set; }
//        public bool HasSchedules => Schedules != null && Schedules.Any();
//        public bool HasOnlyAvailableSchedules => Schedules.Any() && Schedules.All(s => s.IsAvailable == true);
//        public bool HasOnlyBookedSchedules => Schedules.Any() && Schedules.All(s => s.IsAvailable == false);
//        public bool HasMixedSchedules => Schedules.Any(s => s.IsAvailable == true) && Schedules.Any(s => s.IsAvailable == false);
//        public string ScheduleDetails => HasSchedules ? string.Join("\n", Schedules.Select(s => $"{s.StartTime} - {s.EndTime} ({(s.IsAvailable == true ? "Available" : "Booked")})")) : "";
//    }
//}