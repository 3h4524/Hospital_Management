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
    public class SystemUserRepository : BaseRepository<SystemUser>
    {   
        public SystemUserRepository(HospitalManagementContext context) : base(context){}

        public async Task<SystemUser?> GetByUsernameAndPassword(string username, string password)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.)
        }
    }
}
