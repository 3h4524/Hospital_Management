using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Model;

namespace Repository
{
    public class PatientRepository : BaseRepository<Patient>
    {
        private readonly HospitalManagementContext _context;
        public PatientRepository(HospitalManagementContext context) : base(context)
        {
            _context = context;
        }
    }
}
