using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Repository
{
    public class DoctorScheduleRepository : BaseRepository<DoctorScheduleRepository>
    {
        public DoctorScheduleRepository(HospitalManagementContext context) : base(context)
        {
        }

    }
}
