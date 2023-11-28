using Domain;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RepositoryAndServices.Context;

namespace RepositoryAndServices.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        #region property  
        private readonly ApplicationDBContext _applicationDbContext;
        private readonly DbSet<T> entities;
        #endregion

        #region Constructor
        public Repository(ApplicationDBContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            entities = _applicationDbContext.Set<T>();
        }
        #endregion

        public async Task<bool> Delete(T entity)
        {
            entities.Remove(entity);
            var result = await _applicationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<T> Get(Guid Id)
        {
            return await entities.FindAsync(Id);
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public async Task<bool> Insert(T entity)
        {
            await entities.AddAsync(entity);
            var result = await _applicationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
       
        public T GetLast()
        {
            if (entities.ToList() != null)
            {
                return entities.ToList().LastOrDefault();
            }
            else
            {
                return entities.ToList().LastOrDefault();
            }

        }
        public async Task<bool> Update(T entity)
        {
            entities.Update(entity);
            var result = await _applicationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<T> Find(Expression<Func<T, bool>> match)
        {
            return await entities.FirstOrDefaultAsync(match);
        }

        public async Task<ICollection<T>> FindAll(Expression<Func<T, bool>> match)
        {
            return await entities.Where(match).ToListAsync();
        }
    }
}
