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
    public class DoctorScheduleRepository : BaseRepository<DoctorSchedule>
    {
        public DoctorScheduleRepository(HospitalManagementContext context) : base(context) { }


        public async Task<IEnumerable<DoctorSchedule>> GetScheduleForDoctor(int DoctorId)
        {
            return await _dbSet
                .Include(ds => ds.Doctor)
                .Where(ds => ds.DoctorId == DoctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DoctorSchedule>> GetScheduleForDoctorByMonthSofar(int doctorId, int selectedMonth)
        {
            DateTime today = DateTime.Today;
            int currentYear = today.Year;

            return await _dbSet
                .Include(ds => ds.Doctor)
                .Where(ds =>
                    ds.DoctorId == doctorId &&
                    ds.WorkDate.Month == selectedMonth &&
                    ds.WorkDate.Year == currentYear &&
                    ds.WorkDate <= DateOnly.FromDateTime(today))
                .ToListAsync();
        }



        public async Task<IEnumerable<DoctorSchedule>> GetDoctorSchedulesByMonth(int DoctorId, int month, int year)
        {
            var startOfMonth = new DateOnly(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await _dbSet
                .Include(ds => ds.Doctor)
                .Where(ds => ds.DoctorId == DoctorId
                    && ds.WorkDate >= startOfMonth
                    && ds.WorkDate <= endOfMonth)
                .ToListAsync();
        }

        public async Task<IEnumerable<DoctorSchedule>> GetDoctorSchedulesByDoctorIdAndWorkDate(int userId, DateOnly today)
        {
            return await _dbSet
                .Include(ds => ds.Doctor)
                .Where(ds => ds.DoctorId == userId && ds.WorkDate == today)
                .ToListAsync();
        }

        public async Task<List<DateOnly>> GetWorkDatesForDoctorByMonthSofar(int doctorId, int selectedMonth)
        {
            DateTime today = DateTime.Today;
            int currentYear = today.Year;

            return await _dbSet
                .Where(ds =>
                    ds.DoctorId == doctorId &&
                    ds.WorkDate.Month == selectedMonth &&
                    ds.WorkDate.Year == currentYear &&
                    ds.WorkDate <= DateOnly.FromDateTime(today))
                .Select(ds => ds.WorkDate)
                .Distinct()
                .ToListAsync();
        }
    }
}
