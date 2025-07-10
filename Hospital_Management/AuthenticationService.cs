using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Model;
using Repository;

namespace Service
{
    public class AuthenticationService
    {   
        private readonly SystemUserRepository _userRepository;

        public AuthenticationService(HospitalManagementContext context)
        {
            _userRepository = new SystemUserRepository(context);
        }

        public Task<SystemUser?> Login(String account, String password)
        {
            return 
        }
    }
}
