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
    public class RewardPenaltyRepository : BaseRepository<RewardPenalty>
    {
        public RewardPenaltyRepository(HospitalManagementContext context) : base(context) { }

        public async Task<IEnumerable<RewardPenalty>> GetListRewardAndPenaltyByDoctorIdAndMonthSofar(int DoctorId, int SelectedMonth)
        {
            var today = DateTime.Now;
            int currentYear = today.Year;
            return await _dbSet
                .Include(rp => rp.User)
                .Where(rp => rp.UserId == DoctorId &&
                             rp.Rpdate.Month == SelectedMonth &&
                             rp.Rpdate.Year == currentYear &&
                             rp.Rpdate <= DateOnly.FromDateTime(today))
                .ToListAsync();
        }
    }
}
