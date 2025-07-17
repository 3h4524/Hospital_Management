using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class BaseRepository<T> : GenericRepository<T> where T : class
    {
        protected readonly HospitalManagementContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(HospitalManagementContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task Add(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> FindByID(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
