using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repository;

namespace Service
{
    public class AttendanceService
    {
        private readonly AttendanceRepository _repo;

        public AttendanceService(AttendanceRepository repo)
        {
            _repo = repo;
        }
        public Task<IEnumerable<Timekeeping>> GetByDate(DateOnly date)
        {
            return _repo.GetByDate(date);
        }

        public Task<IEnumerable<Timekeeping>> GetByUser(int userId)
        {
            return _repo.GetByUser(userId);
        }
    }
}

