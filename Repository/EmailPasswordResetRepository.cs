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
    public class EmailPasswordResetRepository : BaseRepository<EmailResetPassword>
    {
        public EmailPasswordResetRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<EmailResetPassword?> GetValidResetCode(string email, string code)
        {
            return await _context.EmailResetPassword
                .Include(e => e.User)
                .Where(e => e.User.Email == email &&
                            e.ResetCode == code &&
                            !e.IsUsed &&
                            e.ExpiredAt > DateTime.Now)
                .FirstOrDefaultAsync();
        }

        public async Task MarkAsUsed(EmailResetPassword entry)
        {
            entry.IsUsed = true;
            _context.EmailResetPassword.Update(entry);
            await _context.SaveChangesAsync();
        }
    }
}
