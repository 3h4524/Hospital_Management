using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace Repository
{
    public class AttendanceRepository : BaseRepository<Timekeeping>
    {
        public AttendanceRepository(HospitalManagementContext context) : base(context) { }

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
