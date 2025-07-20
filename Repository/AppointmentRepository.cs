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
    public class AppointmentRepository : BaseRepository<Appointment>
    {
        public AppointmentRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<List<Appointment>> GetAllByDoctorIdAndWorkDate(int doctorId, DateOnly date)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Equals(date))
                .Include(a => a.Patient)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }
    }
}
