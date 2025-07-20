using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using DTO.Response;
using Model;
using Repository;

namespace Service
{
    public class AttendanceService
    {
        TimeKeepingRepository _timeKeepingRepository;

        public AttendanceService(TimeKeepingRepository timeKeepingRepository)
        {
            _timeKeepingRepository = timeKeepingRepository;
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
}
