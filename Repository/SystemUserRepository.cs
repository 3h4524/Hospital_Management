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
        public SystemUserRepository(HospitalManagementContext context) : base(context) { }

        public async Task<bool> CheckExistedEmail(string email)
        {
            return await _dbSet.AnyAsync(x => x.Email == email);
        }

        public async Task<SystemUser?> FindByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }

        public bool UpdateUserRole(int userId, string newRole)
        {
            var user = _context.SystemUsers.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.Role = newRole;
            user.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return true;
        }
        public List<SystemUser> GetAllSystemUsers()
        {
            return _context.SystemUsers.ToList();
        }
    }
}
