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
        Task<T> GetById(Guid id, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Get(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetByAll();
        Task Insert(T entity);
        Task<List<T>> Insert(List<T> entities);
        Task Update(T entity);
        Task Delete(Guid id);
        Task Update(List<T> entities);
        Task Add(T entity);
    }
}
