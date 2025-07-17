using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service
{
    public class TimekeepingService
    {
        private readonly TimekeepingService _repo;

        public TimekeepingService(TimekeepingService repo)
        {
            _repo = repo;
        }
        public IEnumerable<Timekeeping> GetByDate(DateOnly date)
        {
            return _repo.GetByDate(date);
        }

        public IEnumerable<Timekeeping> GetByUser(int userId)
        {
            return _repo.GetByUser(userId);
        }
    }
}

