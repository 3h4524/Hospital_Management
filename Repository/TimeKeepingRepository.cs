using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class TimeKeepingRepository : BaseRepository<Timekeeping>
    {
        public TimeKeepingRepository(HospitalManagementContext context) : base(context) { }

        public async Task<IEnumerable<Timekeeping>> GetTimekeepingInCurrentMonthStatusLateByDoctorId(int DoctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int currentMonth = today.Month;
            int currentYear = today.Year;

            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Schedule)
                .Where(t => t.UserId == DoctorId)
                .Where(t => t.WorkDate.Month == currentMonth && t.WorkDate.Year == currentYear)
                .Where(t => t.Status == "Late")
                .ToListAsync();
        }
        public async Task<Timekeeping?> GetActiveTimeKeepingForUserAndDate(int userId, DateOnly date)
        {
            return await _context.Timekeepings
                .Where(t => t.UserId == userId
                            && t.WorkDate == date
                            && t.CheckInTime != null
                    && t.CheckOutTime == new TimeOnly(0, 0))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Timekeeping>> GetByDate(DateOnly date)
        {
            return await Find(t => t.WorkDate == date);
        }
        public async Task<IEnumerable<Timekeeping>> GetByUser(int userId)
        {
            return await Find(t => t.UserId == userId);
        }

        public async Task<bool> HasCheckedInForSchedule(int userId, int scheduleId)
        {
            return await _context.Timekeepings
                           .AnyAsync(t => t.UserId == userId && t.ScheduleId == scheduleId);
        }

    }
}


