using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Model;

namespace Repository
{
    public class DoctorRepository : BaseRepository<DoctorProfile>
    {
        private readonly HospitalManagementContext _context;

        public DoctorRepository(HospitalManagementContext context) : base(context)
        {
            _context = context;
        }

    }
}
