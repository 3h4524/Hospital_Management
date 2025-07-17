using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository;

namespace Service
{
    public class AttendanceService
    {
        private readonly AttendanceRepository _repo;

        public AttendanceService(AttendanceRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Timekeeping>> GetByDate(DateOnly date)
        {
            return await _repo.GetByDate(date);
        }

        public async Task<IEnumerable<Timekeeping>> GetByUser(int userId)
        {
            return await _repo.GetByUser(userId);
        }


        public async Task Add(Timekeeping timekeeping)
        {
            await _repo.Add(timekeeping);
        }

        public async Task<bool> HasCheckedInForSchedule(int userId, int scheduleId)
        {
           return await _repo.HasCheckedInForSchedule(userId, scheduleId);
        }

        public async Task<Timekeeping> GetActiveTimeKeepingForUserAndDate(int userId, DateOnly today)
        {
            return await _repo.GetActiveTimeKeepingForUserAndDate(userId, today);

        }

        public async Task Update(Timekeeping timeKeeping)
        {
            await _repo.Update(timeKeeping);
        }
    }
}

