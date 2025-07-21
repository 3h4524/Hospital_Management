using DataAccess;
using Model;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SpecializationRepository : BaseRepository<Specialization>
    {
        public SpecializationRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<List<Specialization>> GetAllSpecializationsAsync()
        {
            return await _context.Specializations.ToListAsync();
        }

        public async Task<Specialization?> GetSpecializationByIdAsync(int id)
        {
            return await _context.Specializations.FindAsync(id);
        }
    }
} 