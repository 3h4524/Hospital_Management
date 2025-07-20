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

        public AttendanceService(DoctorRepository doctorRepository, DoctorScheduleRepository doctorScheduleRepository, SystemUserRepository systemUserRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _systemUserRepository = systemUserRepository;
        }

        //public async List<DoctorScheduleReportResponse> Watch()
        public async void Watch()
        {
            int TotalDaysWorked = 0;
            Double TotalWorkingHours = 0;

            List<SystemUser> listDoctor = (await _systemUserRepository.Find(u => u.Role == "Doctor")).ToList();

            foreach (var doctor in listDoctor)
            {
                Debug.WriteLine($"Doctor: {doctor.UserId}");

                List<DoctorSchedule> schedules = (await _doctorScheduleRepository.GetScheduleForDoctorThisMonth(doctor.UserId)).ToList();

                TotalDaysWorked = schedules
                    .GroupBy(s => s.WorkDate)
                    .Count();

                TotalWorkingHours = schedules
                    .Where(s => s.Status == "Working")
                    .Sum(s => (s.EndTime - s.StartTime).TotalHours);

                List<Timekeeping> timekeepings = doctor.Timekeepings;
            }

        }

    }

    public class DoctorScheduleReportResponse()
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public int TotalDaysWorked { get; set; }

        public Double TotalWorkingHours { get; set; }

        public Double TotalLateHours { get; set; }

        public string Status { get; set; }

    }
}
