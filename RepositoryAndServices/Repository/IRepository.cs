using System.Linq.Expressions;

namespace RepositoryAndServices.Repository
{
    public interface IRepository<T>
    {
        Task<ICollection<T>> GetAll();

        Task<T> Get(Guid id);

        T GetLast();

        Task<bool> Insert(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(T entity);

        Task<T> Find(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAll(Expression<Func<T, bool>> match);
    }
}
