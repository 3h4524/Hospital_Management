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

        public async Task<bool> CheckExistedEmail(string email)
        {
            return await _dbSet.AnyAsync(x => x.Email == email);
        }

        public async Task<SystemUser?> FindByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
