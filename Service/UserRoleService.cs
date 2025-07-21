using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Repository;

namespace Service
{
    public class UserRoleService
    {
        private readonly SystemUserRepository _userRepo;

        public UserRoleService(SystemUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<bool> AssignRoleToUser(int userId, string newRole)
        {
            var validRoles = new[] { "Admin", "Receptionist", "Doctor" };
            if (!validRoles.Contains(newRole)) return false;
            return await Task.Run(() => _userRepo.UpdateUserRole(userId, newRole));
        }

        public List<SystemUser> GetAllUsers()
        {
            return _userRepo.GetAllSystemUsers();
        }

        public async Task<SystemUser> GetUserById(int userId)
        {
            return await _userRepo.FindByID(userId) ?? throw new ArgumentException("User not found");
        }
    }
}
