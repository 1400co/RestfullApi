using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
     public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(Guid id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsNoTracking(Guid id, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Get(Expression<Func<T, bool>>? additionalConditions = null, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAsNoTracking(Expression<Func<T, bool>>? additionalConditions = null, params Expression<Func<T, object>>[] includes);
        Task<T> Insert(T entity);
        Task<List<T>> Insert(List<T> entities);
        Task Update(T entity);
        Task Update(List<T> entities);
        Task Delete(Guid id);
        Task Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        void Detach(T entity);
    }
}
