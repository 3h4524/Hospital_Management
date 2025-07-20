using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DTO.Request;
using Microsoft.EntityFrameworkCore;
using Model;
using DTO.Response;
using System.Diagnostics;


namespace Repository
{
    public class DoctorRepository : BaseRepository<DoctorProfile>
    {

        public DoctorRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<List<DoctorInfomationResponse>> FindAllBySpecificationWithSubquery(int pageNumber, int pageSize, DoctorInformationSearch search)
        {
            int page = pageNumber > 0 ? pageNumber : 1;
            int size = pageSize > 0 ? pageSize : 10;


            IQueryable<DoctorProfile> query = _dbSet
                .Include(d => d.User)
                .ThenInclude(u => u.DoctorSchedules);

            if (!string.IsNullOrWhiteSpace(search.FullName))
            {
                query = query.Where(d => d.User.FullName.Contains(search.FullName));
            }

            if (!string.IsNullOrWhiteSpace(search.Specification))
            {
                query = query.Where(d => d.Specialization == search.Specification);
            }

            IQueryable<int> availableDoctorIds = !search.Date.Equals(DateTime.Now)
                ? _context.DoctorSchedules
                    .Where(s => s.WorkDate.Equals(search.Date))
                    .Select(s => s.DoctorId)
                    .Distinct()
                : _context.DoctorSchedules
                    .Where(s => s.WorkDate.Equals(DateOnly.FromDateTime(DateTime.Now)))
                    .Select(s => s.DoctorId)
                    .Distinct();

            query = query.Where(d => availableDoctorIds.Contains(d.UserId));

           

            var pageDate = await query
                .OrderBy(d => d.User.FullName)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return pageDate
                .Select(d =>
                {
                    var schedule = d.User.DoctorSchedules
                        .FirstOrDefault(s => {
                            Debug.WriteLine($"WorkDate: {s.WorkDate}");
                            return s.WorkDate.Equals(search.Date);
                            });

                    Debug.WriteLine($"Schedule: {schedule?.WorkDate}, search date: {search.Date}");
                    
                    string availableDateTime = schedule != null
                        ? $"{schedule.WorkDate:dd/MM/yyyy} : {schedule.StartTime:hh\\:mm}-{schedule.EndTime:hh\\:mm}"
                        : "N/A";

                    return new DoctorInfomationResponse
                    {
                        DoctorId = d.UserId,
                        DoctorName = d.User.FullName,
                        Specialization = d.Specialization,
                        AvailableDateTime = availableDateTime
                    };
                })
                .ToList();
        }
    }
}
