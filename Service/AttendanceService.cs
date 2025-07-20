using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repository;

namespace Service
{
    public class AttendanceService
    {
        DoctorRepository _doctorRepository;
        DoctorScheduleRepository _doctorScheduleRepository;
        SystemUserRepository _systemUserRepository;
        TimeKeepingRepository _timeKeepingRepository;

        public AttendanceService(DoctorRepository doctorRepository, DoctorScheduleRepository doctorScheduleRepository, SystemUserRepository systemUserRepository, TimeKeepingRepository timeKeepingRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _systemUserRepository = systemUserRepository;
            _timeKeepingRepository = timeKeepingRepository;
        }

        //public async Task<IEnumerable<DoctorScheduleReportResponse>> Watch()
        public async void Watch()
        {
            List<DoctorScheduleReportResponse> result = new();
            List<SystemUser> listDoctor = (await _systemUserRepository.Find(u => u.Role == "Doctor")).ToList();

            foreach (var doctor in listDoctor)
            {
                Debug.WriteLine($"Doctor: {doctor.UserId}");

                List<DoctorSchedule> schedules = (await _doctorScheduleRepository.GetScheduleForDoctorThisMonth(doctor.UserId)).ToList();

                int TotalDaysWorked = schedules
                     .GroupBy(s => s.WorkDate)
                     .Count();

                double TotalWorkingHours = schedules
                    .Where(s => s.Status == "Working")
                    .Sum(s => (s.EndTime - s.StartTime).TotalHours);

                List<Timekeeping> timekeepings = (await _timeKeepingRepository.GetTimekeepingInCurrentMonthStatusLateByDoctorId(doctor.UserId)).ToList();

                double TotalLateHours = timekeepings
                    .Where(t => t.CheckInTime.HasValue && t.Schedule != null)
                    .Sum(t => (t.CheckInTime.Value - t.Schedule.StartTime).TotalHours);

                DoctorScheduleReportResponse response = new DoctorScheduleReportResponse
                {
                    DoctorId = doctor.UserId,
                    DoctorName = doctor.FullName,
                    TotalDaysWorked = TotalDaysWorked,
                    TotalWorkingHours = TotalWorkingHours,
                    TotalLateHours = TotalLateHours,
                };

                result.Add(response);
            }

            result.ForEach(a => Debug.WriteLine(a.ToString()));
        }


        public async Task<IEnumerable<Timekeeping>> GetByDate(DateOnly date)
        {
            return await _timeKeepingRepository.GetByDate(date);
        }

        public async Task<IEnumerable<Timekeeping>> GetByUser(int userId)
        {
            return await _timeKeepingRepository.GetByUser(userId);
        }


        public async Task Add(Timekeeping timekeeping)
        {
            await _timeKeepingRepository.Add(timekeeping);
        }

        public async Task<bool> HasCheckedInForSchedule(int userId, int scheduleId)
        {
            return await _timeKeepingRepository.HasCheckedInForSchedule(userId, scheduleId);
        }

        public async Task<Timekeeping> GetActiveTimeKeepingForUserAndDate(int userId, DateOnly today)
        {
            return await _timeKeepingRepository.GetActiveTimeKeepingForUserAndDate(userId, today);

        }

        public async Task Update(Timekeeping timeKeeping)
        {
            await _timeKeepingRepository.Update(timeKeeping);
        }
    }

    public class DoctorScheduleReportResponse()
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public int TotalDaysWorked { get; set; }

        public Double TotalWorkingHours { get; set; }

        public Double TotalLateHours { get; set; }

        public override string ToString()
        {
            return $"Doctor ID: {DoctorId}, Name: {DoctorName}, " +
                   $"Total Days Worked: {TotalDaysWorked}, " +
                   $"Total Working Hours: {TotalWorkingHours:F2}, " +
                   $"Total Late Hours: {TotalLateHours:F2}";
        }

    }
}
