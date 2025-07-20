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
        public DoctorScheduleRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<DoctorSchedule?> FindByDoctorIdAndDate(int doctorId, DateOnly date)
        {
            return _context.DoctorSchedules
                .Where(df => df.DoctorId == doctorId && df.WorkDate.Equals(date))
                .FirstOrDefault();
        }
    }
}
