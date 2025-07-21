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
        private readonly HospitalManagementContext _context;
        public PatientRepository(HospitalManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Patient?> FindByEmail(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Patient?> FindByPhone(string phoneNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsWithAppointments()
        {
            return await _dbSet
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .Include(p => p.Appointments)
                .ThenInclude(a => a.MedicalRecords)
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }

        public async Task<Patient?> GetPatientWithDetails(int patientId)
        {
            return await _dbSet
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .Include(p => p.Appointments)
                .ThenInclude(a => a.MedicalRecords)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        public async Task<IEnumerable<Patient>> SearchPatients(string searchTerm)
        {
            return await _dbSet
                .Include(p => p.Appointments)
                .Where(p => p.FullName.Contains(searchTerm) || 
                           p.PhoneNumber.Contains(searchTerm) || 
                           p.Email.Contains(searchTerm))
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }
    }
}
