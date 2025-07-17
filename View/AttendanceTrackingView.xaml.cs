using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Repository;
using Service;

namespace View
{
    public partial class AttendanceTrackingView : UserControl
    {
        private readonly AttendanceService _attendanceService;

        private readonly DoctorScheduleService _doctorScheduleService;

        public ObservableCollection<Timekeeping> AttendanceRecords { get; set; } = new();

        private SystemUser user;

        public AttendanceTrackingView(AttendanceService attendanceService)
        {
            InitializeComponent();
            _attendanceService = attendanceService;
            _doctorScheduleService = App._serviceProvider.GetRequiredService<DoctorScheduleService>();
            AttendanceDataGrid.ItemsSource = AttendanceRecords;

            user = Application.Current.Properties["CurrentUser"] as SystemUser
                   ?? throw new InvalidOperationException("CurrentUser session is missing");

            LoadAttendance();
        }

        private async Task LoadAttendance()
        {

            Debug.WriteLine("debug" + user.UserId);
            AttendanceRecords.Clear();
            var userId = user.UserId;
            var records = await _attendanceService.GetByUser(userId);
            foreach (var record in records.OrderByDescending(r => r.WorkDate))
                AttendanceRecords.Add(record);
        }

        private async void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var checkInTime = TimeOnly.FromDateTime(DateTime.Now);
            var checkOutTime = new TimeOnly(0, 0);
            var userId = user.UserId;

            var schedules = await _doctorScheduleService.GetDoctorSchedulesByDoctorIdAndWorkDate(userId, today);

            if (schedules.Any()) {

                DoctorSchedule? thisSchedule = schedules
                    .FirstOrDefault(s => checkInTime >= s.StartTime.AddHours(-1) && checkInTime <= s.EndTime);
       
                if (thisSchedule == null)
                {
                    MessageBox.Show("Chưa tới lịch làm của bạn!");
                    return;
                }  else {
                    bool alreadyCheckedIn = await _attendanceService.HasCheckedInForSchedule(userId, thisSchedule.ScheduleId);
                    if (alreadyCheckedIn)
                    {
                        MessageBox.Show("Bạn đã điểm danh cho ca làm này rồi!");
                        return;
                    }

                    Timekeeping timeKeeping = new Timekeeping
                    {
                        UserId = userId,
                        ScheduleId = thisSchedule.ScheduleId,
                        WorkDate = today,
                        CheckInTime = checkInTime,
                        CheckOutTime = checkOutTime,
                        Status = checkInTime <= thisSchedule.StartTime ? "ON TIME" : "Late"
                    };

                    await _attendanceService.Add(timeKeeping);

                    await LoadAttendance();
                }
                    
            } else
            {
                MessageBox.Show("Bạn không có lịch làm hôm nay!");
            }
        }

        private async void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var checkOutTime = TimeOnly.FromDateTime(DateTime.Now);
            var userId = user.UserId;

            var timeKeeping = await _attendanceService.GetActiveTimeKeepingForUserAndDate(userId, today);

            if (timeKeeping == null)
            {
                MessageBox.Show("Không có ca làm nào cần checkout!");
                return;
            }

            // Lấy thông tin lịch làm việc tương ứng
            var schedule = await _doctorScheduleService.GetDoctorScheduleById(timeKeeping.ScheduleId);

            if (schedule == null)
            {
                MessageBox.Show("Không tìm thấy lịch làm việc!");
                return;
            }

            // Kiểm tra thời gian checkout
            if (checkOutTime < schedule.EndTime)
            {
                MessageBox.Show("Chưa tới giờ checkout!");
                return;
            }

            if (checkOutTime > schedule.EndTime.AddHours(1))
            {
                MessageBox.Show("Đã quá thời gian checkout!");
                return;
            }

            // Cập nhật thời gian checkout
            timeKeeping.CheckOutTime = checkOutTime;
            await _attendanceService.Update(timeKeeping);
            await LoadAttendance();
            MessageBox.Show("Checkout thành công!");
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

            await LoadAttendance();

        }
    }
}
