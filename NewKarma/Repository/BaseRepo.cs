using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewKarma.Repository
{
    public class BaseRepo<T, Context> : IBaseRepo<T> where T : class where Context : DbContext
    {
        protected Context _context { get; set; }
        private DbSet<T> dbSet;
        public BaseRepo(Context context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> FindAllAsync() => await dbSet.ToListAsync();
        public IEnumerable<T> FindAll() => dbSet.ToList();
        public async Task<T> FindByIdAsync(object id) => await dbSet.FindAsync(id);
        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();
        }
        public async Task Create(T entity) => await dbSet.AddAsync(entity);
        public void Update(T entity) => dbSet.Update(entity);
        public void Delete(T entity) => dbSet.Remove(entity);
        public async Task CreateRange(IEnumerable<T> entities) => await dbSet.AddRangeAsync(entities);
        public void UpdateRange(IEnumerable<T> entities) => dbSet.UpdateRange(entities);
        public void DeleteRange(IEnumerable<T> entities) => dbSet.RemoveRange(entities);
    }
}
