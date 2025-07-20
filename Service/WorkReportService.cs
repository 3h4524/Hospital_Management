using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;
using DTO.Response;
using Model;
using Repository;

namespace Service
{
    public class WorkReportService
    {
        DoctorRepository _doctorRepository;
        DoctorScheduleRepository _doctorScheduleRepository;
        SystemUserRepository _systemUserRepository;
        TimeKeepingRepository _timeKeepingRepository;
        RewardPenaltyRepository _rewardPenaltyRepository;

        public WorkReportService(DoctorRepository doctorRepository, DoctorScheduleRepository doctorScheduleRepository, SystemUserRepository systemUserRepository, TimeKeepingRepository timeKeepingRepository, RewardPenaltyRepository rewardPenaltyRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _systemUserRepository = systemUserRepository;
            _timeKeepingRepository = timeKeepingRepository;
            _rewardPenaltyRepository = rewardPenaltyRepository;
        }

        public async Task<IEnumerable<DoctorScheduleReportResponse>> GetDoctorScheduleReportsSoFar(int selectedMonth)
        {
            List<DoctorScheduleReportResponse> result = new();
            List<SystemUser> listDoctor = (await _systemUserRepository.Find(u => u.Role == "Doctor")).ToList();

            foreach (var doctor in listDoctor)
            {
                Debug.WriteLine($"Doctor: {doctor.UserId}");

                List<DoctorSchedule> schedules = (await _doctorScheduleRepository.GetScheduleForDoctorByMonthSofar(doctor.UserId, selectedMonth)).ToList();

                int totalDaysWorked = schedules
                     .GroupBy(s => s.WorkDate)
                     .Count();

                double totalWorkingHours = schedules
                    .Where(s => s.Status == "Working")
                    .Sum(s => (s.EndTime - s.StartTime).TotalHours);

                int totalDaysOff = schedules
                     .Where(s => s.Status == "Off")
                     .GroupBy(s => s.WorkDate)
                     .Count();

                List<Timekeeping> timekeepings = (await _timeKeepingRepository.GetTimekeepingStatusLateByDoctorIdByMonthSofar(doctor.UserId, selectedMonth)).ToList();

                double totalLateHours = timekeepings
                    .Where(t => t.CheckInTime.HasValue && t.Schedule != null)
                    .Sum(t => (t.CheckInTime.Value - t.Schedule.StartTime).TotalHours);


                List<RewardPenalty> rewardPenaltiesList = (await _rewardPenaltyRepository.GetListRewardAndPenaltyByDoctorIdAndMonthSofar(doctor.UserId, selectedMonth)).ToList();

                decimal totalRewardAmount = rewardPenaltiesList
                    .Where(rp => rp.Type == "Reward")
                    .Sum(rp => rp.Amount);

                decimal totalPenaltyAmount = rewardPenaltiesList
                    .Where(rp => rp.Type == "Penalty")
                    .Sum(rp => rp.Amount);

                decimal extraPenalty =
                    ((decimal)totalLateHours * (decimal)PenaltyType.LatePerHour);

                if (totalDaysOff > 2)
                {
                    extraPenalty += (decimal)PenaltyType.AttendanceBonus;
                }

                DoctorScheduleReportResponse response = new DoctorScheduleReportResponse
                {
                    DoctorId = doctor.UserId,
                    DoctorName = doctor.FullName,
                    TotalDaysWorked = totalDaysWorked,
                    TotalWorkingHours = ((float)totalWorkingHours),
                    TotalLateHours = ((float)totalLateHours),
                    TotalDaysOff = totalDaysOff,
                    TotalRewardAmount = totalRewardAmount,
                    TotalPenaltyAmount = totalPenaltyAmount + extraPenalty
                };

                result.Add(response);
            }
            return result;
        }


        public async Task<IEnumerable<DailyScheduleReportResponse>> GetDailyScheduleReportResponseSofarByDoctorId(int doctorId, int selectedMonth)
        {
            var result = new List<DailyScheduleReportResponse>();

            SystemUser doctor = await _systemUserRepository.FindByID(doctorId) ?? throw new Exception("Can not find doctor");
            List<DateOnly> uniqueWorkDates = (await _doctorScheduleRepository.GetWorkDatesForDoctorByMonthSofar(doctor.UserId, selectedMonth)).ToList();

            foreach (var date in uniqueWorkDates)
            {
                List<Timekeeping> timekeepings = (await _timeKeepingRepository.GetTimekeepingsForDate(doctorId, date)).ToList();

                float workingHours = 0;
                float lateHours = 0;
                string status = "Off";

                foreach (var timekeeping in timekeepings)
                {
                    if (timekeeping.Schedule.Status == "Working") status = "Working";

                    if (timekeeping.Status == "Late")
                    {
                        if (timekeeping.CheckInTime.HasValue)
                        {
                            workingHours += (float)(timekeeping.Schedule.EndTime.ToTimeSpan() - timekeeping.CheckInTime.Value.ToTimeSpan()).TotalHours;
                            lateHours += (float)(timekeeping.CheckInTime.Value.ToTimeSpan() - timekeeping.Schedule.StartTime.ToTimeSpan()).TotalHours;
                        }
                        else
                        {
                            throw new Exception("Checkin Time is null");
                        }
                    }
                    else
                    {
                        workingHours += (float)(timekeeping.Schedule.EndTime.ToTimeSpan() - timekeeping.Schedule.StartTime.ToTimeSpan()).TotalHours;
                    }
                }


                DailyScheduleReportResponse reportResponse = new DailyScheduleReportResponse
                {
                    DoctorId = doctorId,
                    DoctorName = doctor.FullName,
                    WorkDate = date,
                    WorkingHours = workingHours,
                    LateHours = lateHours,
                    Status = status
                };
                result.Add(reportResponse);
            }
            return result;
        }

    }
}
