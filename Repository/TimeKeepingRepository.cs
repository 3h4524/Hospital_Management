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
                .Where(t => t.UserId == DoctorId)
                .Where(t => t.WorkDate.Month == currentMonth && t.WorkDate.Year == currentYear)
                .Where(t => t.Status == "Late")
                .ToListAsync();
        }


    }
}


