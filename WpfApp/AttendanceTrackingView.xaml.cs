using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataAccess;
using Model;
using Repository;

namespace View
{
    public partial class AttendanceTrackingView : UserControl
    {
        private readonly AttendanceRepository _attendanceRepository;
        public ObservableCollection<Timekeeping> AttendanceRecords { get; set; } = new();

        // Giờ làm việc chuẩn (có thể chỉnh theo quy định công ty)
        private static readonly TimeOnly StartWorkTime = new(8, 0);   // 8:00 sáng
        private static readonly TimeOnly EndWorkTime = new(17, 0);    // 17:00 chiều

        public AttendanceTrackingView()
        {
            InitializeComponent();
            var context = new HospitalManagementContext();
            _attendanceRepository = new AttendanceRepository(context);
            AttendanceDataGrid.ItemsSource = AttendanceRecords;
            LoadAttendance();
        }

        private async void LoadAttendance()
        {
            AttendanceRecords.Clear();
            var userId = 1; // TODO: Lấy userId hiện tại (hoặc truyền vào constructor)
            var records = await _attendanceRepository.GetByUser(userId);
            foreach (var record in records.OrderByDescending(r => r.WorkDate))
                AttendanceRecords.Add(record);
        }

        private async void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var userId = 1; // TODO: Lấy userId hiện tại
            var records = await _attendanceRepository.GetByUser(userId);
            var todayRecord = records.FirstOrDefault(r => r.WorkDate == today);

            if (todayRecord != null && todayRecord.CheckInTime != null)
            {
                MessageBox.Show("Bạn đã check-in hôm nay rồi!");
                return;
            }

            var now = TimeOnly.FromDateTime(DateTime.Now);
            string status = now <= StartWorkTime ? "ON TIME" : "Late";

            var newRecord = todayRecord ?? new Timekeeping { UserId = userId, WorkDate = today };
            newRecord.CheckInTime = now;
            newRecord.Status = status;

            if (todayRecord == null)
                await _attendanceRepository.Add(newRecord);
            else
                await _attendanceRepository.Update(newRecord);

            LoadAttendance();
        }

        private async void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var userId = 1; // TODO: Lấy userId hiện tại
            var records = await _attendanceRepository.GetByUser(userId);
            var todayRecord = records.FirstOrDefault(r => r.WorkDate == today);

            if (todayRecord == null || todayRecord.CheckInTime == null)
            {
                MessageBox.Show("Bạn chưa check-in hôm nay!");
                return;
            }
            if (todayRecord.CheckOutTime != null)
            {
                MessageBox.Show("Bạn đã check-out hôm nay rồi!");
                return;
            }

            var now = TimeOnly.FromDateTime(DateTime.Now);
            string status = todayRecord.Status;
            if (now < EndWorkTime)
                status = "Early"; // Về sớm

            todayRecord.CheckOutTime = now;
            todayRecord.Status = status;

            await _attendanceRepository.Update(todayRecord);
            LoadAttendance();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAttendance();
        }
    }
}