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
    public class PatientRepository : BaseRepository<Patient>
    {
        public PatientRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<Patient?> GetPatientByPhone(string phoneNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.PhoneNumber == phoneNumber);
        }
    }
}
