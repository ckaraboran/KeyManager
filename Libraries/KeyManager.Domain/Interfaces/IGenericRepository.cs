using System.Linq.Expressions;
using KeyManager.Domain.Entities;

namespace KeyManager.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();

    Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);

    Task<T> GetAsync(int id);

    Task<T> GetAsync(Expression<Func<T, bool>> expression);

    Task<T> AddAsync(T entity);

    Task SoftDeleteAsync(T entity);

    Task DeleteAsync(T entity);

    Task<T> UpdateAsync(T entity);
}