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
using BCrypt;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Service
{
    public class AuthenticationService
    {   
        private readonly SystemUserRepository _userRepository;
        private readonly EmailPasswordResetRepository _emailPasswordResetRepository;
        private readonly EmailService _emailService;

        public AuthenticationService(HospitalManagementContext context)
        {
            _userRepository = new SystemUserRepository(context);
            _emailPasswordResetRepository = new EmailPasswordResetRepository(context);
            _emailService = new EmailService();
        }

        public async Task<SystemUser?> Login(String email, String password)
        {
            var user = await _userRepository.FindByEmail(email);
            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.HashPassword);
            return isValid ? user : null;
        }

        public async Task<bool> Register(String email, String password)
        {
            if (await _userRepository.CheckExistedEmail(email))
                return false;

            string hashPassword = HashPassword(password);

            var newUser = new SystemUser
            {
                Email = email,
                HashPassword = hashPassword
            };

            await _userRepository.Add(newUser);
            return true;
        }

        public async Task<bool> ForgetPassword(string email)
        {
            Debug.WriteLine("Checking email for send code");

            var user = await _userRepository.FindByEmail(email);
            if (user == null)
            {
                Debug.WriteLine("Email not found");
                return false;
            } 


            string resetCode = Guid.NewGuid().ToString("N").Substring(0, 8);

            var resetEntry = new EmailResetPassword
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                ResetCode = resetCode,
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddMinutes(10),
                IsUsed = false
            };

            await _emailPasswordResetRepository.Add(resetEntry);

            string subject = "Reset Password Request";
            string body = $"Hello,\n\n Your password reset code is: {resetCode}\nThis code will expire in 10 minutes.\n\nRegards,\nHospital Support.";
            bool emailSend = _emailService.sendEmail(email, subject, body);

            Debug.WriteLine("Reset code sent!");
            return emailSend;
        }

        public async Task<bool> ResetPassword(string email, string resetCode, string newPassword)
        {
            var user = await _userRepository.FindByEmail(email);
            if (user == null)
                return false;

            var resetEntry = await _emailPasswordResetRepository.GetValidResetCode(email, resetCode);
            if (resetEntry == null)
                return false;

            user.HashPassword = HashPassword(newPassword);
            await _userRepository.Update(user);
            await _emailPasswordResetRepository.MarkAsUsed(resetEntry);
            return true;
        }

        private string HashPassword(String password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }


    }


}
